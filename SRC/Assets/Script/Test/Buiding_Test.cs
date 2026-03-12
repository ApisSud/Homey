using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;

public enum SizeFurniture
{
    small,
    large,
    smaller
}

public enum TypeFurniture
{
    woodSmall,
    woodlarge,
    glass,
    pot
}

public enum Level
{
    Level1, Level2,
}

public class Buiding_Test : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Grid layoutGrid;
    [SerializeField] private Tilemap FloorSelect;
    [SerializeField] private TileBase highlightTile;
    [SerializeField] private Grid TableGrid;
    [SerializeField] private Tilemap FloorTableSelect;

    private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    private Vector3 offset;
    private Vector3 OriginalPosition;
    private Vector3Int previousCellPos;
    private Vector3Int previousCellPos2;
    private Vector3 finalPos;
    [SerializeField] private SpriteRenderer bodyColor;
    private Color32 originalColor;
    private bool Flip;
    private bool Draged;
    private bool onStorageFur;

    
    [SerializeField]
    private SizeFurniture Size;
    [SerializeField]
    private TypeFurniture Type;

    private Isogrid currentTargetGrid;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector3 worldPos = transform.position;
        Vector3Int cellPosition = layoutGrid.WorldToCell(worldPos);
        Debug.Log(cellPosition);
        onStorageFur = false;
        CheckGrid.instance.PlaceObject(cellPosition, $"{Type}");
        if (GameManager.instance.IsWithinBounds(cellPosition))
        {
            CheckGrid.instance.addFurnitureInScene(cellPosition);
        }
        if (Size == SizeFurniture.large)
        {
            CheckGrid.instance.PlaceObject(cellPosition + new Vector3Int(0, 1, 0), $"{Type}");
        }

        originalColor = bodyColor.color;
    }

    private void Update()
    {
        if (Draged)
        {
            if (Input.GetKeyDown(KeyCode.E) && Flip == false)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                Flip = true;
                UpdateHighlight(layoutGrid.WorldToCell(transform.position), FloorSelect);
            }
            else if (Input.GetKeyDown(KeyCode.E) && Flip == true)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                Flip = false;
                UpdateHighlight(layoutGrid.WorldToCell(transform.position), FloorSelect);
            }
        }


    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        OriginalPosition = transform.position;
        Vector3 worldPos = transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector3Int cellPosition = layoutGrid.WorldToCell(worldPos);
        //Debug.Log($"BeginDrag{cellPosition} mousePos :{mousePos}");
        offset = transform.position - mousePos;
        CheckGrid.instance.RemoveObject(cellPosition);
        CheckGrid.instance.removeFurnitureInScene(cellPosition);
        if (Size == SizeFurniture.large)
        {
            if (Flip == false)
                CheckGrid.instance.RemoveObject(cellPosition + new Vector3Int(0, 1, 0));
            else
                CheckGrid.instance.RemoveObject(cellPosition + new Vector3Int(1, 0, 0));
        }
        if(Size == SizeFurniture.smaller && onStorageFur)
        {
            //finalPos = currentTargetGrid.GetClosestSnapPoint(mousePos);
            //Debug.Log($"Remove {finalPos}");
            CheckGrid.instance.RemoveObject(finalPos);
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 screenPos = eventData.position;
        screenPos.z = 10f;

        Vector3 worldPos = transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(screenPos);
        mousePos.z = 0;
        Vector3Int cellPosition = layoutGrid.WorldToCell(mousePos);
        Vector3 snapPos = layoutGrid.GetCellCenterWorld(cellPosition);

        /*transform.position = eventData.position;
        transform.position = mousePos + offset;*/

        Draged = true;
        if (Size == SizeFurniture.smaller)
        { 
            sr.sortingOrder = 3;
            if(!CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition))
            {
                transform.position = mousePos;
            }
            if (onStorageFur)
            {
                //Debug.Log("inStorage");
                //Debug.Log($"worldPos1 : {mousePos}");

                finalPos = currentTargetGrid.GetClosestSnapPoint(mousePos);
                transform.position = finalPos;
                //Debug.Log($"fur will snap : {finalPos}");
            }
            else if(!onStorageFur)
            {
                transform.position = snapPos;
            }

           /* if (CheckGrid.instance.occupiedTiles.TryGetValue(cellPosition, out string itemName))
            {
                Debug.Log(worldPos);
                Vector3 finalPos = worldPos;
                sr.sortingOrder = 3;
                if (itemName == "woodlarge")
                {
                    Debug.Log("in Storage");
                    finalPos = Isogrid.Instance.GetClosestSnapPoint(worldPos);
                    
                }
                transform.position = finalPos;
                Debug.Log(finalPos);

                *//*if (CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition) || !IsWithinBounds(cellPosition))
                {
                    bodyColor.color = new Color32(255, 0, 0, 255);
                }
                else if (!CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition))
                {
                    bodyColor.color = originalColor;
                }*//*
            }
            else
            {
                transform.position = snapPos;
                Debug.Log("Not in Storage");
            }
*/
        }

        if (Size != SizeFurniture.smaller)
        {
            transform.position = snapPos;
            if (cellPosition != previousCellPos && GameManager.instance.IsWithinBounds(cellPosition))
            {
                UpdateHighlight(cellPosition, FloorSelect);

            }
            if (!GameManager.instance.IsWithinBounds(cellPosition))
            {
                FloorSelect.SetTile(previousCellPos, null);
            }
            if (CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition) || !GameManager.instance.IsWithinBounds(cellPosition))
            {
                bodyColor.color = new Color32(255, 0, 0, 255);
            }
            else if (!CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition))
            {
                bodyColor.color = originalColor;
            }

        }


        //Debug.Log($"OnDrag{cellPosition}");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 worldPos = transform.position;
        Vector3Int cellPosition = layoutGrid.WorldToCell(worldPos);
        Vector3 snapPos = layoutGrid.GetCellCenterWorld(cellPosition);
        //Debug.Log("EndDrag");
        bodyColor.color = originalColor;
        FloorSelect.SetTile(cellPosition, null);

        if (Flip == false)
            FloorSelect.SetTile(cellPosition + new Vector3Int(0, 1, 0), null);
        else
            FloorSelect.SetTile(cellPosition + new Vector3Int(1, 0, 0), null);

        Draged = false;
        if (!CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition) && GameManager.instance.IsWithinBounds(cellPosition))
        {
            transform.position = snapPos;
            CheckGrid.instance.PlaceObject(cellPosition, $"{Type}");
            CheckGrid.instance.addFurnitureInScene(cellPosition);
            if (Size == SizeFurniture.large)
            {
                if (Flip == false)
                    CheckGrid.instance.PlaceObject(cellPosition + new Vector3Int(0, 1, 0), $"{Type}");
                else
                    CheckGrid.instance.PlaceObject(cellPosition + new Vector3Int(1, 0, 0), $"{Type}");
            }
            Debug.Log($"{cellPosition} empty");
        }
        else
        {
            transform.position = OriginalPosition;
            cellPosition = layoutGrid.WorldToCell(OriginalPosition);
            CheckGrid.instance.PlaceObject(cellPosition, $"{Type}");
    
            if (Size == SizeFurniture.large)
            {
                if (Flip == false)
                    CheckGrid.instance.PlaceObject(cellPosition + new Vector3Int(0, 1, 0), $"{Type}");
                else
                    CheckGrid.instance.PlaceObject(cellPosition + new Vector3Int(1, 0, 0), $"{Type}");
            }
            if(GameManager.instance.IsWithinBounds(cellPosition))
            {
                Debug.Log("inRoom");
                CheckGrid.instance.addFurnitureInScene(cellPosition);
            }
            Debug.Log($"{cellPosition} not empty");
        }

        if(onStorageFur)
        {  if (!CheckGrid.instance.occupiedTiles.ContainsKey(finalPos))
            {
                transform.position = finalPos;
                CheckGrid.instance.PlaceObject(finalPos, $"{Type}");
            }
            else if(CheckGrid.instance.occupiedTiles.ContainsKey(finalPos))
            {
                transform.position = OriginalPosition;
                cellPosition = layoutGrid.WorldToCell(OriginalPosition);
                CheckGrid.instance.PlaceObject(cellPosition, $"{Type}");
            }
        }

        if (Type == TypeFurniture.woodlarge)
        {
            SoundManager.instance.playSFX(SoundManager.instance.PutDownHeavyFur);
        }
        if (Type == TypeFurniture.woodSmall)
        {
            SoundManager.instance.playSFX(SoundManager.instance.PutDownFur);
        }
        if (Type == TypeFurniture.pot)
        {
            SoundManager.instance.playSFX(SoundManager.instance.PutDownPotWitch);
        }
        if (Type == TypeFurniture.glass)
        {
            SoundManager.instance.playSFX(SoundManager.instance.PutDownGlassBottle);
        }

    }

   
    void UpdateHighlight(Vector3Int cellPosition, Tilemap highlight_Tile)
    {
        highlight_Tile.SetTile(previousCellPos, null);
        highlight_Tile.SetTile(cellPosition, highlightTile);
        previousCellPos = cellPosition;

        if (Size == SizeFurniture.large && Flip == false)
        {

            highlight_Tile.SetTile(previousCellPos, null);
            highlight_Tile.SetTile(previousCellPos2, null);
            highlight_Tile.SetTile(cellPosition, highlightTile);
            highlight_Tile.SetTile(cellPosition + new Vector3Int(0, 1, 0), highlightTile);
            previousCellPos = cellPosition;
            previousCellPos2 = cellPosition + new Vector3Int(0, 1, 0);
        }
        else if (Size == SizeFurniture.large && Flip == true)
        {
            highlight_Tile.SetTile(previousCellPos, null);
            highlight_Tile.SetTile(previousCellPos2, null);
            highlight_Tile.SetTile(cellPosition, highlightTile);
            highlight_Tile  .SetTile(cellPosition + new Vector3Int(1, 0, 0), highlightTile);
            previousCellPos = cellPosition;
            previousCellPos2 = cellPosition + new Vector3Int(1, 0, 0);
        }
    }

    

    private void OnTriggerStay2D(Collider2D Furniture)
    {
        int furnitureLayer = LayerMask.NameToLayer("FurnitureStorage");

        if (Furniture.gameObject.layer == furnitureLayer)
        {
            currentTargetGrid = Furniture.GetComponent<Isogrid>();
            onStorageFur = true;
        }

    }

    private void OnTriggerExit2D(Collider2D Furniture)
    {
        currentTargetGrid = null;
        onStorageFur = false;
    }

}
