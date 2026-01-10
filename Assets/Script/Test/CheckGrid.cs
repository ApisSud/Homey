using System.Collections.Generic;
using UnityEngine;

public class CheckGrid : MonoBehaviour
{
    public Grid Grid;
    private Dictionary<Vector3Int, bool> occupiedTiles = new Dictionary<Vector3Int, bool>();
    void Start()
    {
        
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
        }
    }

    public void RemoveObject(Vector3Int gridPos)
    {
        if (occupiedTiles.ContainsKey(gridPos))
        {
            occupiedTiles.Remove(gridPos);
        }
    }
}
