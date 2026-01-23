using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] FurniturePF;
    public int maxSpawnLimit = 10;
    public GameObject spawneffect;
    private int currentClickIndex = 0;

    void OnMouseDown()
    {

        if (currentClickIndex < maxSpawnLimit && currentClickIndex < FurniturePF.Length)
        {

            if (FurniturePF[currentClickIndex] != null)
            {

                Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                spawnPos.z = 0;


                Instantiate(FurniturePF[currentClickIndex], spawnPos, Quaternion.identity);

                if (spawneffect != null)
                {
                    Instantiate(spawneffect, spawnPos, Quaternion.identity);
                }
                currentClickIndex++;


            }
        }
       
    }
}
