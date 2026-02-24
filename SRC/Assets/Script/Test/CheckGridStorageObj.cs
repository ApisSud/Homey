using System.Collections.Generic;
using UnityEngine;

public class CheckGridStorageObj : MonoBehaviour
{
    public Grid Grid;
    public Dictionary<Vector3Int, string> occupiedTilesStorage = new Dictionary<Vector3Int, string>();
    public static CheckGridStorageObj instance;

    [SerializeField] int minX = 0, maxX = 5;
    [SerializeField] int minY = 0, maxY = 2;

    void Start()
    {
        instance = this;
    }

   
    public void PlaceObject(Vector3Int gridPos, string type)
    {
        GameObject[] furnitureItems = GameObject.FindGameObjectsWithTag("Furniture");
        if (!occupiedTilesStorage.ContainsKey(gridPos))
        {
            occupiedTilesStorage.Add(gridPos, type);
            Debug.Log($"Grid {gridPos} status is: {occupiedTilesStorage[gridPos]}");
            //Debug.Log($"Num Fur : {furnitureItems.Length}");

        }

     
    }

    public void RemoveObject(Vector3Int gridPos)
    {
        if (occupiedTilesStorage.ContainsKey(gridPos))
        {
            occupiedTilesStorage.Remove(gridPos);
        }
    }

    public bool IsWithinBoundsStorage(Vector3Int gridPosition)
    {

        return (gridPosition.x >= minX && gridPosition.x <= maxX &&
                gridPosition.y >= minY && gridPosition.y <= maxY);
    }
}
