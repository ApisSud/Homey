using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    
    public GameObject objectPrefab;
    public GameObject spawnEffect;

    void OnMouseDown()
    {
        if (objectPrefab != null)
        {
            
            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPos.z = 0;

            if (spawnEffect != null)
            {
                Instantiate(spawnEffect, spawnPos, Quaternion.identity);
            }
            Instantiate(objectPrefab, spawnPos, Quaternion.identity);
        }
    }
}
