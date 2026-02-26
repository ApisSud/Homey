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

public class Buiding_Test : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler
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
    [SerializeField] private SpriteRenderer bodyColor;
    private Color32 originalColor;
    private bool Flip;
    private bool Draged;



    [SerializeField]
    private SizeFurniture Size;
    [SerializeField]
    private TypeFurniture Type;


    [SerializeField]  int minX = -6, maxX = 2;
    [SerializeField]  int minY = -6, maxY = 2;




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector3 worldPos = transform.position;
        Vector3Int cellPosition = layoutGrid.WorldToCell(worldPos);
        Debug.Log(cellPosition);

        CheckGrid.instance.PlaceObject(cellPosition, $"{Type}");
        CheckGrid.instance.addFurnitureInScene(cellPosition);
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
        Debug.Log($"BeginDrag{cellPosition}");
        offset = transform.position - mousePos;
        CheckGrid.instance.RemoveObject(cellPosition);
        //CheckGrid.instance.removeFurnitureInScene(cellPosition);
        if (Size == SizeFurniture.large)
        {
            if (Flip == false)
                CheckGrid.instance.RemoveObject(cellPosition + new Vector3Int(0, 1, 0));
            else
                CheckGrid.instance.RemoveObject(cellPosition + new Vector3Int(1, 0, 0));
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPos = transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(eventData.position);
        mousePos.z = 0;
        Vector3Int cellPosition = layoutGrid.WorldToCell(mousePos);
        Vector3 snapPos = layoutGrid.GetCellCenterWorld(cellPosition);

        /*transform.position = eventData.position;
        transform.position = mousePos + offset;*/

        Draged = true;
        if (Size == SizeFurniture.smaller)
        {

            if (CheckGrid.instance.occupiedTiles.TryGetValue(cellPosition, out string itemName))
            {
                Vector3 finalPos = worldPos;
                sr.sortingOrder = 3;
                if (itemName == "woodlarge")
                {
                    Debug.Log("in Storage");
                    finalPos = Isogrid.Instance.GetClosestSnapPoint(worldPos);
                    
                }
                transform.position = finalPos;
                Debug.Log(finalPos);

                /*if (CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition) || !IsWithinBounds(cellPosition))
                {
                    bodyColor.color = new Color32(255, 0, 0, 255);
                }
                else if (!CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition))
                {
                    bodyColor.color = originalColor;
                }*/
            }
            else
            {
                transform.position = snapPos;
                Debug.Log("Not in Storage");
            }

        }

        if (Size != SizeFurniture.smaller)
        {
            transform.position = snapPos;
            if (cellPosition != previousCellPos && IsWithinBounds(cellPosition))
            {
                UpdateHighlight(cellPosition, FloorSelect);

            }
            if (!IsWithinBounds(cellPosition))
            {
                FloorSelect.SetTile(previousCellPos, null);
            }
            if (CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition) || !IsWithinBounds(cellPosition))
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
        if (!CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition) && IsWithinBounds(cellPosition))
        {
            transform.position = snapPos;
            CheckGrid.instance.PlaceObject(cellPosition, $"{Type}");
            //CheckGrid.instance.addFurnitureInScene(cellPosition);
            if (Size == SizeFurniture.large)
            {
                if (Flip == false)
                    CheckGrid.instance.PlaceObject(cellPosition + new Vector3Int(0, 1, 0), $"{Type}");
                else
                    CheckGrid.instance.PlaceObject(cellPosition + new Vector3Int(1, 0, 0), $"{Type}");
            }
            //Debug.Log($"{cellPosition} empty");
        }
        else
        {
            transform.position = OriginalPosition;
            cellPosition = layoutGrid.WorldToCell(OriginalPosition);
            CheckGrid.instance.PlaceObject(cellPosition, $"{Type}");
            //CheckGrid.instance.addFurnitureInScene(cellPosition);
            if (Size == SizeFurniture.large)
            {
                if (Flip == false)
                    CheckGrid.instance.PlaceObject(cellPosition + new Vector3Int(0, 1, 0), $"{Type}");
                else
                    CheckGrid.instance.PlaceObject(cellPosition + new Vector3Int(1, 0, 0), $"{Type}");
            }
            //Debug.Log($"{cellPosition} not empty");
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

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPoint");
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

    public bool IsWithinBounds(Vector3Int gridPosition)
    {

        return (gridPosition.x >= minX && gridPosition.x <= maxX &&
                gridPosition.y >= minY && gridPosition.y <= maxY);
    }

    
}
