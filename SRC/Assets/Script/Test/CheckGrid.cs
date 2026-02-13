using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckGrid : MonoBehaviour
{
    public Grid Grid;
    public Dictionary<Vector3Int, string> occupiedTiles = new Dictionary<Vector3Int, string>();
    public Dictionary<Vector3Int, bool> CheckFurniture = new Dictionary<Vector3Int, bool>();
    public static CheckGrid instance;
    [SerializeField]
    private Scrollbar processFurnitureBar;
    [SerializeField]
    private GameObject FinishButton;

    public int FurnitureInScene = 0;
    public int FurnitureSpawn = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        FinishButton.gameObject.SetActive(false);
    }
    

    public void PlaceObject(Vector3Int gridPos, string type)
    {
        GameObject[] furnitureItems = GameObject.FindGameObjectsWithTag("Furniture");
        if (!occupiedTiles.ContainsKey(gridPos))
        {
            occupiedTiles.Add(gridPos, type);
            Debug.Log($"Grid {gridPos} status is: {occupiedTiles[gridPos]}");
            //Debug.Log($"Num Fur : {furnitureItems.Length}");

        }
        UpdateFurnitureBar(furnitureItems.Length);

        if (FurnitureInScene + FurnitureSpawn == furnitureItems.Length)
        {
            FinishButton.gameObject.SetActive(true);
        }


    }
    public void addFurnitureInScene(Vector3Int gridPos)
    {
        if(!CheckFurniture.ContainsKey(gridPos))
        {
            CheckFurniture.Add(gridPos, true);
            Debug.Log($"Num Fur In Scene: {CheckFurniture.Count}");
        }
    }

    public void RemoveObject(Vector3Int gridPos)
    {
        if (occupiedTiles.ContainsKey(gridPos))
        {
            occupiedTiles.Remove(gridPos);
        }
    }

    public void removeFurnitureInScene(Vector3Int gridPos)
    {
        if (CheckFurniture.ContainsKey(gridPos))
        { CheckFurniture.Add(gridPos, true); }
    }

    public void CheckEmpty(Vector3Int gridPos)
    {
        if (!occupiedTiles.ContainsKey(gridPos))
        {
            //Debug.Log($"{gridPos} empty");
        }
        else if (occupiedTiles.ContainsKey(gridPos))
        {
            //Debug.Log($"{gridPos} not empty");
        }
    
    }

    public void UpdateFurnitureBar(float NumFur)
    {
        processFurnitureBar.size = (NumFur - FurnitureInScene) / FurnitureSpawn;
    }
}
