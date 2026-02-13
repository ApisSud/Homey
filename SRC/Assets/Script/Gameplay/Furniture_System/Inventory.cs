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
               
                Instantiate(FurniturePF[currentClickIndex], spawnPoint.position, Quaternion.identity);

                
                if (spawneffect != null)
                {
                    Instantiate(spawneffect, spawnPoint.position, Quaternion.identity);
                }

                currentClickIndex++; 
            }
        }
        else
        {
            Debug.Log("Empty Items");
        }
    }
}
