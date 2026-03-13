using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckGrid : MonoBehaviour
{
    public Grid Grid;
    public Dictionary<Vector3, string> occupiedTiles = new Dictionary<Vector3, string>();
    public Dictionary<Vector3, bool> CheckFurnitureinScene = new Dictionary<Vector3, bool>();
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
    

    public void PlaceObject(Vector3 gridPos, string type)
    {
        GameObject[] furnitureItems = GameObject.FindGameObjectsWithTag("Furniture");
        if (!occupiedTiles.ContainsKey(gridPos))
        {
            occupiedTiles.Add(gridPos, type);
            Debug.Log($"Grid {gridPos} status is: {occupiedTiles[gridPos]}");
            //Debug.Log($"Num Fur : {furnitureItems.Length}");

        }

      


    }
    public void addFurnitureInScene(Vector3 gridPos)
    {
        if(!CheckFurnitureinScene.ContainsKey(gridPos))
        {
            CheckFurnitureinScene.Add(gridPos, true);
            //Debug.Log($"Num Fur In Scene: {CheckFurnitureinScene.Count}");
        }
        UpdateFurnitureBar(CheckFurnitureinScene.Count);

        if (FurnitureInScene + FurnitureSpawn == CheckFurnitureinScene.Count)
        {
            FinishButton.gameObject.SetActive(true);
        }
    }

    public void RemoveObject(Vector3 gridPos)
    {
        if (occupiedTiles.ContainsKey(gridPos))
        {
            occupiedTiles.Remove(gridPos);
        }
    }

    public void removeFurnitureInScene(Vector3 gridPos)
    {
        if (CheckFurnitureinScene.ContainsKey(gridPos))
        { CheckFurnitureinScene.Remove(gridPos); }
    }

    public void CheckEmpty(Vector3 gridPos)
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

    public void UpdateFurnitureBar(float NumFur)
    {
        processFurnitureBar.size = (NumFur - FurnitureInScene) / FurnitureSpawn;
    }

    public void FurinRoom()
    {
        
    }
}
