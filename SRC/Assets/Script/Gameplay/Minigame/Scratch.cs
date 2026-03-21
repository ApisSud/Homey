using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Scratch : MonoBehaviour
{


    [Header("Tool Reference")]
    public BroomController broom;

    [Header("Settings")]
    public SpriteRenderer[] dirtSprites;
    public int brushSize = 30;

    [Header("Win Condition")]
    [Range(0f, 1f)]
    public float winPercentage = 0.8f;

    [Header("UI Management")]
    public TextMeshProUGUI progress;

    private Texture2D[] textures;
    private Color32[][] pixelsArray;
    private int[] totalPixelsArray;
    private int[] clearedPixelsArray;
    private bool[] isCleanedArray;

    private Vector2 lastMousePos;
    private bool isGameActive = true;

    void Start()
    {
        int dirtCount = dirtSprites.Length;
        textures = new Texture2D[dirtCount];
        pixelsArray = new Color32[dirtCount][];
        totalPixelsArray = new int[dirtCount];
        clearedPixelsArray = new int[dirtCount];
        isCleanedArray = new bool[dirtCount];

        // วนลูปตั้งค่าให้กับคราบสกปรกทุกชิ้น
        for (int i = 0; i < dirtCount; i++)
        {
            SpriteRenderer sr = dirtSprites[i];
            Texture2D originalTex = sr.sprite.texture;

            // สร้าง Texture โคลนนิ่ง
            Texture2D tex = new Texture2D(originalTex.width, originalTex.height, TextureFormat.RGBA32, false);
            tex.SetPixels32(originalTex.GetPixels32());
            tex.Apply();

            Sprite newSprite = Sprite.Create(tex, sr.sprite.rect, new Vector2(0.5f, 0.5f), sr.sprite.pixelsPerUnit);
            sr.sprite = newSprite;

            textures[i] = tex;
            pixelsArray[i] = tex.GetPixels32();

            // นับจำนวนพิกเซลที่มีสีของแต่ละคราบ
            int tPixels = 0;
            Rect rect = sr.sprite.textureRect;
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
                        if (pixelsArray[i][index].a > 0)
                        {
                            tPixels++;
                        }
                    }
                }
            }
            if (tPixels == 0) tPixels = 1; // กัน Error หาร 0
            totalPixelsArray[i] = tPixels;
        }

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
            broom.StopScrubbing();
        }
    }

    void EraseLine(Vector2 startWorld, Vector2 endWorld)
    {
        bool anyChanged = false;

        // วนลูปเช็คคราบสกปรกทุกชิ้นว่าเมาส์ถูโดนชิ้นไหนบ้าง
        for (int d = 0; d < dirtSprites.Length; d++)
        {
            if (isCleanedArray[d]) continue; // ถ้าคราบชิ้นนี้สะอาดแล้ว ให้ข้ามไปไม่ต้องเช็ค

            Vector2 startPixel = WorldToPixel(startWorld, dirtSprites[d]);
            Vector2 endPixel = WorldToPixel(endWorld, dirtSprites[d]);

            float distance = Vector2.Distance(startPixel, endPixel);
            int steps = Mathf.Max(1, Mathf.CeilToInt(distance / (brushSize / 4f)));

            bool changed = false;

            for (int i = 0; i <= steps; i++)
            {
                Vector2 point = Vector2.Lerp(startPixel, endPixel, (float)i / steps);
                if (EraseCircle((int)point.x, (int)point.y, d))
                {
                    changed = true;
                    anyChanged = true;
                }
            }

            if (changed)
            {
                textures[d].SetPixels32(pixelsArray[d]);
                textures[d].Apply();
            }
        }

        if (anyChanged)
        {
            CheckWin();
        }
    }

    bool EraseCircle(int centerX, int centerY, int dirtIndex)
    {
        bool changed = false;
        int radiusSq = brushSize * brushSize;
        Texture2D tex = textures[dirtIndex];
        Color32[] pixels = pixelsArray[dirtIndex];

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
                            clearedPixelsArray[dirtIndex]++;
                            changed = true;
                        }
                    }
                }
            }
        }
        return changed;
    }

    Vector2 WorldToPixel(Vector2 worldPos, SpriteRenderer sr)
    {
        Vector2 localPos = sr.transform.InverseTransformPoint(worldPos);
        Sprite sprite = sr.sprite;

        float pixelX = (localPos.x * sprite.pixelsPerUnit) + sprite.pivot.x + sprite.textureRect.x;
        float pixelY = (localPos.y * sprite.pixelsPerUnit) + sprite.pivot.y + sprite.textureRect.y;

        return new Vector2(pixelX, pixelY);
    }

    void CheckWin()
    {
        int globalCleared = 0;
        int globalTotal = 0;

        // รวมคะแนนจากทุกคราบ
        for (int i = 0; i < dirtSprites.Length; i++)
        {
            globalCleared += clearedPixelsArray[i];
            globalTotal += totalPixelsArray[i];

            // เช็คว่าคราบ "ชิ้นย่อย" ชิ้นนี้ สะอาดถึงเกณฑ์หรือยัง (ถ้าถึงแล้วให้ซ่อนชิ้นนี้ไปก่อน)
            if (!isCleanedArray[i])
            {
                float localPercent = (float)clearedPixelsArray[i] / totalPixelsArray[i];
                if (localPercent >= winPercentage)
                {
                    isCleanedArray[i] = true;
                    dirtSprites[i].gameObject.SetActive(false);
                }
            }
        }

        // คำนวณเปอร์เซ็นต์ "รวมทั้งหมด" เพื่อแสดงขึ้น UI
        float globalPercent = (float)globalCleared / globalTotal;
        int displayPercent = Mathf.Clamp(Mathf.RoundToInt(globalPercent * 100), 0, 100);
        UpdateProgressUI(displayPercent);

        // ถ้าค่ารวมทั้งหมดถึงเกณฑ์ ชนะเกม!
        if (globalPercent >= winPercentage)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        if (!isGameActive) return;

        isGameActive = false;
        Debug.Log("Cleaned All! You Win!");

        if (progress != null)
        {
            progress.text = "Progress : Done!";
        }

        // ปิดการแสดงผลคราบทุกชิ้นที่อาจจะยังหลงเหลืออยู่
        for (int i = 0; i < dirtSprites.Length; i++)
        {
            if (dirtSprites[i] != null) dirtSprites[i].gameObject.SetActive(false);
        }

        broom.StopScrubbing();
        broom.UnequipBroom();

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
