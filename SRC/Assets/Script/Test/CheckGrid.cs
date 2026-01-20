using System.Collections.Generic;
using UnityEngine;

public class CheckGrid : MonoBehaviour
{
    public Grid Grid;
    public Dictionary<Vector3Int, bool> occupiedTiles = new Dictionary<Vector3Int, bool>();
    public static CheckGrid instance;
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
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
            occupiedTiles.Remove(gridPos);
            Debug.Log($"Grid {gridPos} status is: {occupiedTiles[gridPos]}");
        }
    }

    public void CheckEmpty(Vector3Int gridPos)
    {
        if (!occupiedTiles.ContainsKey(gridPos))
        {
            occupiedTiles.Add(gridPos, true);
            Debug.Log($"Grid {gridPos} status is: {occupiedTiles[gridPos]}");
        }
    }
}
