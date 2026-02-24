using UnityEngine;
using UnityEngine.EventSystems;

public class BroomController : MonoBehaviour
{
    [Header("Settings")]
    public bool isEquipped = false; // สถานะว่าถือไม้กวาดอยู่ไหม
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false; // ซ่อนไม้กวาดตอนเริ่มเกม
    }

    void Update()
    {
        // ถ้าถือไม้กวาดอยู่ ให้ไม้กวาดตามตำแหน่งเมาส์
        if (isEquipped)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
        }

        // ออปชั่นเสริม: คลิกขวาเพื่อ "วาง" ไม้กวาด (เลิกถือ)
        if (isEquipped && Input.GetMouseButtonDown(1))
        {
            UnequipBroom();
        }
    }

    // ฟังก์ชันนี้จะถูกเรียกตอนกดปุ่ม UI
    public void EquipBroom()
    {
        isEquipped = true;
        spriteRenderer.enabled = true; // โชว์ไม้กวาด

        // ซ่อนเมาส์ปกติของระบบ (ให้เห็นแต่ไม้กวาด)
        Cursor.visible = false;
    }

    public void UnequipBroom()
    {
        isEquipped = false;
        spriteRenderer.enabled = false; // ซ่อนไม้กวาด

        // โชว์เมาส์ปกติกลับมา
        Cursor.visible = true;
    }
}
