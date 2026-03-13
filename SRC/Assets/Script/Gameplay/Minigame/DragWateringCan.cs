using UnityEngine;

public class DragWateringCan : MonoBehaviour
{
    private Vector3 offset;
    private FlowerObject[] allFlowers;

    void Start()
    {
        // ตอนเริ่มเกม ให้หา Object ทั้งหมดที่มีสคริปต์ Flower แปะอยู่ มาเก็บไว้
        allFlowers = FindObjectsOfType<FlowerObject>();
    }

    void OnMouseDown()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        offset = transform.position - Camera.main.ScreenToWorldPoint(mousePosition);

        // --- เมื่อคลิกหยิบบัวรดน้ำ ให้สั่งดอกไม้ทุกต้นเริ่มไฮไลต์ ---
        foreach (FlowerObject flower in allFlowers)
        {
            if (flower != null)
            {
                flower.StartHighlight();
            }
        }
    }

    void OnMouseDrag()
    {
        Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        transform.position = Camera.main.ScreenToWorldPoint(newPosition) + offset;
    }

    void OnMouseUp()
    {
        // --- เมื่อปล่อยเมาส์ (วางบัวรดน้ำ) ให้สั่งดอกไม้ทุกต้นหยุดไฮไลต์ ---
        foreach (FlowerObject flower in allFlowers)
        {
            if (flower != null)
            {
                flower.StopHighlight();
            }
        }
    }
}
