using UnityEngine;

public class CleaningGame : MonoBehaviour
{
    [Header("Settings")]
    public GameObject eraserPrefab; // ลาก Prefab 'EraserBrush' มาใส่
    public Transform maskHolder;    // ลาก 'MaskHolder' มาใส่
    public float brushSize = 5f;  // ระยะห่างขั้นต่ำในการเสก Brush (กันเสกเยอะเกิน)

    [Header("Win Condition")]
    public int dirtAmountToWin = 100; // จำนวนครั้งที่ต้องถูถึงจะชนะ (กะเอา)
    private int currentCleanCount = 0;
    private Vector2 lastPos;
    private bool isGameActive = true;

    void Update()
    {
        if (!isGameActive) return;

        // เช็คว่ากดคลิกซ้ายค้างไว้ไหม
        if (Input.GetMouseButton(0))
        {
            CleanDirt();
        }

        // รีเซ็ตตำแหน่งเมาส์เมื่อปล่อยมือ (เพื่อให้เริ่มจุดใหม่ได้)
        if (Input.GetMouseButtonUp(0))
        {
            lastPos = Vector2.zero;
        }
    }

    void CleanDirt()
    {
        // แปลงตำแหน่งเมาส์จาก Screen เป็น World Point
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // ตรวจสอบระยะห่างจากจุดล่าสุด (Optimization: ไม่ให้เสกถี่เกินไปจนเกมกระตุก)
        if (Vector2.Distance(mousePos, lastPos) > brushSize)
        {
            SpawnEraser(mousePos);
            lastPos = mousePos;
        }
    }

    void SpawnEraser(Vector2 pos)
    {
        // เสกตัว Mask ออกมาที่ตำแหน่งเมาส์
        Instantiate(eraserPrefab, pos, Quaternion.identity, maskHolder);

        // นับจำนวนครั้งที่ถู
        currentCleanCount++;
        CheckWin();
    }

    void CheckWin()
    {
        // ถ้าถูจนครบจำนวนที่ตั้งไว้
        if (currentCleanCount >= dirtAmountToWin)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        isGameActive = false;
        Debug.Log("Cleaned! You Win!");

        Invoke("CloseGame", 2f);
    }

    void CloseGame()
    {
        gameObject.SetActive(false);
    }
}
