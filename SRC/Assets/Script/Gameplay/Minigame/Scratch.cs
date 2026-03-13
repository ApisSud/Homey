using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Scratch : MonoBehaviour
{


    [Header("Tool Reference")]
    public BroomController broom;

    [Header("Settings")]
    public SpriteRenderer dirtSpriteRenderer;
    public int brushSize = 30;

    [Header("Win Condition")]
    [Range(0f, 1f)]
    public float winPercentage = 0.8f;

    [Header("UI Management")]
    public TextMeshProUGUI progress;

    private Texture2D tex;
    private Color32[] pixels;
    private int totalPixels;
    private int clearedPixels = 0;
    private Vector2 lastMousePos;
    private bool isGameActive = true;

    void Start()
    {
        Texture2D originalTex = dirtSpriteRenderer.sprite.texture;
        tex = new Texture2D(originalTex.width, originalTex.height, TextureFormat.RGBA32, false);
        tex.SetPixels32(originalTex.GetPixels32());
        tex.Apply();

        Sprite newSprite = Sprite.Create(tex, dirtSpriteRenderer.sprite.rect, new Vector2(0.5f, 0.5f), dirtSpriteRenderer.sprite.pixelsPerUnit);
        dirtSpriteRenderer.sprite = newSprite;

        pixels = tex.GetPixels32();

      
        totalPixels = 0;
        Rect rect = dirtSpriteRenderer.sprite.textureRect;
        int minX = Mathf.FloorToInt(rect.xMin);
        int maxX = Mathf.FloorToInt(rect.xMax);
        int minY = Mathf.FloorToInt(rect.yMin);
        int maxY = Mathf.FloorToInt(rect.yMax);

        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                if (x >= 0 && x < tex.width && y >= 0 && y < tex.height)
                {
                    int index = y * tex.width + x;
                    
                    if (pixels[index].a > 0)
                    {
                        totalPixels++;
                    }
                }
            }
        }

       
        if (totalPixels == 0) totalPixels = 1;
        UpdateProgressUI(0);
    }

    void Update()
    {
        if (!isGameActive || !broom.isEquipped) return;

     
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            broom.StopScrubbing();
            return;
        }

      
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

           
            broom.StartScrubbing();
        }
     
        else if (Input.GetMouseButton(0))
        {
            Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            EraseLine(lastMousePos, currentMousePos);
            lastMousePos = currentMousePos;
        }
       
        else if (Input.GetMouseButtonUp(0))
        {
            // สั่งให้ไม้กวาดหยุดเล่นอนิเมชันถู!
            broom.StopScrubbing();
        }
    }

    void EraseLine(Vector2 startWorld, Vector2 endWorld)
    {
        Vector2 startPixel = WorldToPixel(startWorld);
        Vector2 endPixel = WorldToPixel(endWorld);

        float distance = Vector2.Distance(startPixel, endPixel);
        int steps = Mathf.Max(1, Mathf.CeilToInt(distance / (brushSize / 4f)));

        bool changed = false;

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

    Vector2 WorldToPixel(Vector2 worldPos)
    {
        Vector2 localPos = dirtSpriteRenderer.transform.InverseTransformPoint(worldPos);
        Sprite sprite = dirtSpriteRenderer.sprite;

        
        float pixelX = (localPos.x * sprite.pixelsPerUnit) + sprite.pivot.x + sprite.textureRect.x;
        float pixelY = (localPos.y * sprite.pixelsPerUnit) + sprite.pivot.y + sprite.textureRect.y;

        return new Vector2(pixelX, pixelY);
    }

    void CheckWin()
    {
        float percentCleared = (float)clearedPixels / totalPixels;

        int displayPercent = Mathf.Clamp(Mathf.RoundToInt(percentCleared * 100), 0, 100);
        UpdateProgressUI(displayPercent);

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

        if (progress != null)
        {
            progress.text = "Progress : Done!";
        }

        dirtSpriteRenderer.gameObject.SetActive(false);
        broom.UnequipBroom();

        broom.StopScrubbing();

        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.CompleteDirtTask();
        }
    }

    void UpdateProgressUI(int percent)
    {
        if (progress != null && isGameActive)
        {
            progress.text = "Progress : " + percent + "%";
        }
    }

}
