using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Scratch : MonoBehaviour
{
    [Header("Tool Reference")]
    public BroomController broom; // ลาก Object 'BroomTool' มาใส่ช่องนี้

    [Header("Settings")]
    public SpriteRenderer dirtSpriteRenderer;
    public int brushSize = 30;



    [Header("Win Condition")]
    [Range(0f, 1f)]
    public float winPercentage = 0.8f;

    [Header("Win UI Settings")]
    public GameObject winPanelObject;     
    public CanvasGroup winCanvasGroup;   
    public RectTransform winPanelRect;    
    public float uiAnimDuration = 0.8f;   
    public float showDelay = 2f;
    public float offScreenPosY = 1000f;
    public float onScreenPosY = 0f;


    private Texture2D tex;
    private Color32[] pixels;
    private int totalPixels;
    private int clearedPixels = 0;
    private Vector2 lastMousePos;
    private bool isGameActive = true;
    private Vector2 originalPanelPos;


    void Start()
    {
        Texture2D originalTex = dirtSpriteRenderer.sprite.texture;
        tex = new Texture2D(originalTex.width, originalTex.height, TextureFormat.RGBA32, false);
        tex.SetPixels32(originalTex.GetPixels32());
        tex.Apply();

        
        Sprite newSprite = Sprite.Create(tex, dirtSpriteRenderer.sprite.rect, new Vector2(0.5f, 0.5f), dirtSpriteRenderer.sprite.pixelsPerUnit);
        dirtSpriteRenderer.sprite = newSprite;

    
        pixels = tex.GetPixels32();
        totalPixels = pixels.Length;
    }

    void Update()
    {

        if (!isGameActive || !broom.isEquipped) return; 

        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            EraseLine(lastMousePos, currentMousePos);
            lastMousePos = currentMousePos;
        }
    }

    void EraseLine(Vector2 startWorld, Vector2 endWorld)
    {
        Vector2 startPixel = WorldToPixel(startWorld);
        Vector2 endPixel = WorldToPixel(endWorld);

        float distance = Vector2.Distance(startPixel, endPixel);
        int steps = Mathf.Max(1, Mathf.CeilToInt(distance / (brushSize / 4f)));

        bool changed = false;

        // วนลูปวาดวงกลมลบรูปภาพ เรียงต่อกันเป็นเส้น
        for (int i = 0; i <= steps; i++)
        {
            Vector2 point = Vector2.Lerp(startPixel, endPixel, (float)i / steps);
            if (EraseCircle((int)point.x, (int)point.y))
            {
                changed = true;
            }
        }

       
        if (changed)
        {
            tex.SetPixels32(pixels);
            tex.Apply();
            CheckWin();
        }
    }

    // ฟังก์ชันเจาะรูวงกลมบนรูปภาพ
    bool EraseCircle(int centerX, int centerY)
    {
        bool changed = false;
        int radiusSq = brushSize * brushSize;

        for (int x = centerX - brushSize; x <= centerX + brushSize; x++)
        {
            for (int y = centerY - brushSize; y <= centerY + brushSize; y++)
            {
                if (x >= 0 && x < tex.width && y >= 0 && y < tex.height)
                {
                    int distSq = (x - centerX) * (x - centerX) + (y - centerY) * (y - centerY);
                    if (distSq <= radiusSq)
                    {
                        int index = y * tex.width + x;
                 
                        if (pixels[index].a > 0)
                        {
                            pixels[index].a = 0; 
                            clearedPixels++;    
                            changed = true;
                        }
                    }
                }
            }
        }
        return changed;
    }

    // แปลงพิกัดโลก (World Space) เป็นพิกัดของรูปภาพ (Pixel)
    Vector2 WorldToPixel(Vector2 worldPos)
    {
        Vector2 localPos = dirtSpriteRenderer.transform.InverseTransformPoint(worldPos);
        Sprite sprite = dirtSpriteRenderer.sprite;

        float pixelX = (localPos.x * sprite.pixelsPerUnit) + (sprite.textureRect.width * 0.5f);
        float pixelY = (localPos.y * sprite.pixelsPerUnit) + (sprite.textureRect.height * 0.5f);

        return new Vector2(pixelX, pixelY);
    }

    void CheckWin()
    {
    
        float percentCleared = (float)clearedPixels / totalPixels;

        if (percentCleared >= winPercentage)
        {
            WinGame();
        }
    }
    void WinGame()
    {
        if (!isGameActive) return;

        isGameActive = false;
        Debug.Log("Cleaned! You Win!");

        dirtSpriteRenderer.gameObject.SetActive(false);
        broom.UnequipBroom();

        // --- เริ่มต้นเปิด Panel ---
        winPanelObject.SetActive(true);

        // 1. เซ็ตตำแหน่งเริ่มต้นให้อยู่ "นอกจอ" (offScreenPosY)
        winPanelRect.anchoredPosition = new Vector2(winPanelRect.anchoredPosition.x, offScreenPosY);
        winCanvasGroup.alpha = 0f;

        // --- Animation ขาเข้า (มาจากข้างบน ลงมาตรงกลาง) ---
        Sequence sequence = DOTween.Sequence();

        // ใช้ DOAnchorPosY ดึงลงมาที่ onScreenPosY (ตั้ง .SetUpdate(true) ให้เล่นได้แม้เกม Pause)
        sequence.Append(winPanelRect.DOAnchorPosY(onScreenPosY, uiAnimDuration).SetEase(Ease.OutBack).SetUpdate(true));
        sequence.Join(winCanvasGroup.DOFade(1f, uiAnimDuration).SetUpdate(true));

        Invoke("CloseWinPanel", showDelay + uiAnimDuration);
    }

    void CloseWinPanel()
    {
        // --- Animation ขาออก (จากตรงกลาง เด้งกลับขึ้นไปข้างบน) ---
        Sequence sequence = DOTween.Sequence();

        // ใช้ DOAnchorPosY ดันกลับขึ้นไปที่ offScreenPosY
        sequence.Append(winPanelRect.DOAnchorPosY(offScreenPosY, uiAnimDuration).SetEase(Ease.InBack).SetUpdate(true));
        sequence.Join(winCanvasGroup.DOFade(0f, uiAnimDuration).SetUpdate(true));

        sequence.OnComplete(() => {
            winPanelObject.SetActive(false);
        });
    }
}
