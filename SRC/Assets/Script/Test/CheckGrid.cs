using System.Collections.Generic;
using UnityEngine;

public class CheckGrid : MonoBehaviour
{
    public Grid Grid;
    public Dictionary<Vector3Int, bool> occupiedTiles = new Dictionary<Vector3Int, bool>();
    public static CheckGrid instance;
    void Awake()
    {
        instance = this;
    }

    void Start()
    { 

    }
    

    public void PlaceObject(Vector3Int gridPos)
    {
        if (!occupiedTiles.ContainsKey(gridPos))
        {
            occupiedTiles.Add(gridPos, true);
            Debug.Log($"Grid {gridPos} status is: {occupiedTiles[gridPos]}");
        }
    }

    public void RemoveObject(Vector3Int gridPos)
    {
        if (occupiedTiles.ContainsKey(gridPos))
        {
            Debug.Log($"Grid {gridPos} status is: remove");
            occupiedTiles.Remove(gridPos);
        }
    }

    public void CheckEmpty(Vector3Int gridPos)
    {
        if (!occupiedTiles.ContainsKey(gridPos))
        {
            Debug.Log($"{gridPos} empty");
        }
        else if (occupiedTiles.ContainsKey(gridPos))
        {
            Debug.Log($"{gridPos} not empty");
        }
    
    }
}
