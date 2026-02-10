using UnityEngine;


public class Inventory : MonoBehaviour
{
    
    public GameObject[] FurniturePF;
    public int maxSpawnLimit = 10;
    public GameObject spawneffect;
    public Transform spawnPoint;

    private int currentClickIndex = 0;

   
    public void SpawnItem()
    {
        if (currentClickIndex < maxSpawnLimit && currentClickIndex < FurniturePF.Length)
        {
            if (FurniturePF[currentClickIndex] != null)
            {
                // *** จุดสำคัญ *** // ใช้ spawnPoint.position (ตำแหน่งของจุดที่เราวางไว้) แทน MousePosition
                Instantiate(FurniturePF[currentClickIndex], spawnPoint.position, Quaternion.identity);

                // เล่นเอฟเฟกต์ที่จุดเดียวกัน
                if (spawneffect != null)
                {
                    Instantiate(spawneffect, spawnPoint.position, Quaternion.identity);
                }

                currentClickIndex++; // เลื่อนไปชิ้นถัดไป
            }
        }
        else
        {
            Debug.Log("Empty Items");
        }
    }
}
