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

    private Texture2D tex;
    private Color32[] pixels;
    private int totalPixels;
    private int clearedPixels = 0;
    private Vector2 lastMousePos;
    private bool isGameActive = true;


    void Start()
    {
        // สร้าง Texture ใหม่โคลนจากรูปเดิม เพื่อไม่ให้ไฟล์ภาพต้นฉบับพัง
        Texture2D originalTex = dirtSpriteRenderer.sprite.texture;
        tex = new Texture2D(originalTex.width, originalTex.height, TextureFormat.RGBA32, false);
        tex.SetPixels32(originalTex.GetPixels32());
        tex.Apply();

        // นำ Texture ที่โคลนมาใส่กลับเข้าไปใน Sprite
        Sprite newSprite = Sprite.Create(tex, dirtSpriteRenderer.sprite.rect, new Vector2(0.5f, 0.5f), dirtSpriteRenderer.sprite.pixelsPerUnit);
        dirtSpriteRenderer.sprite = newSprite;

        // ดึงข้อมูล Pixel ทั้งหมดมาเตรียมคำนวณ
        pixels = tex.GetPixels32();
        totalPixels = pixels.Length;
    }

    void Update()
    {
        // เช็ค 2 อย่าง: เกมยังไม่จบ ใช่ไหม? และ ถือไม้กวาดอยู่ ใช่ไหม?
        if (!isGameActive || !broom.isEquipped) return; // <--- เพิ่มเช็คไม้กวาดตรงนี้

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
        int steps = Mathf.Max(1, Mathf.CeilToInt(distance / (brushSize / 4f))); // คำนวณความถี่ของจุดในเส้น

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

        // ถ้ามีการลบเกิดขึ้น ให้ Update ภาพ และเช็คว่าชนะหรือยัง
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
                        // ถ้าพิกเซลนี้ยังไม่โปร่งใส (Alpha > 0)
                        if (pixels[index].a > 0)
                        {
                            pixels[index].a = 0; // ทำให้โปร่งใส (ลบออก)
                            clearedPixels++;     // นับจำนวนที่ลบไปแล้ว
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
        // คำนวณหา % ที่ขูดไปแล้ว
        float percentCleared = (float)clearedPixels / totalPixels;

        if (percentCleared >= winPercentage)
        {
            WinGame();
        }
    }
    void WinGame()
    {
        isGameActive = false;
        Debug.Log("Cleaned! You Win!");

        dirtSpriteRenderer.gameObject.SetActive(false);

        // เมื่อถูเสร็จ บังคับให้วางไม้กวาดอัตโนมัติ
        broom.UnequipBroom();
    }
}
