using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public enum SizeFurniture
{
    small,
    large
}
public class Buiding_Test : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler
{
    [SerializeField] private Grid layoutGrid;
    [SerializeField] private Tilemap FloorSelect;
    [SerializeField] private TileBase highlightTile;
    private Rigidbody2D rb;
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector3 worldPos = transform.position;
        Vector3Int cellPosition = layoutGrid.WorldToCell(worldPos);
        Debug.Log(cellPosition);

        CheckGrid.instance.PlaceObject(cellPosition);
        if (Size == SizeFurniture.large)
        {
            CheckGrid.instance.PlaceObject(cellPosition + new Vector3Int(0, 1, 0));
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
                UpdateHighlight(layoutGrid.WorldToCell(transform.position));
            }
            else if (Input.GetKeyDown(KeyCode.E) && Flip == true)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                Flip = false;
                UpdateHighlight(layoutGrid.WorldToCell(transform.position));
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

        transform.position = eventData.position;
        transform.position = mousePos + offset;
        Vector3Int cellPosition = layoutGrid.WorldToCell(worldPos);
        CheckGrid.instance.CheckEmpty(cellPosition);
        Draged = true;


        if (cellPosition != previousCellPos && cellPosition.y < 3 && cellPosition.y > -8 && cellPosition.x <= 2 && cellPosition.x > -7)
        {
            UpdateHighlight(cellPosition);

        }
        if (cellPosition.y > 2 || cellPosition.y > 2 || cellPosition.y <= -8 || cellPosition.x > 2 || cellPosition.x <= -7)
        {
            FloorSelect.SetTile(previousCellPos, null);
        }
        if (CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition) || cellPosition.y > 2 || cellPosition.y <= -8 || cellPosition.x > 2 || cellPosition.x <= -7)
        {
            bodyColor.color = new Color32(255, 0, 0, 255);
        }
        else if (!CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition))
        {
            bodyColor.color = originalColor;
        }


        //Debug.Log($"OnDrag{cellPosition}");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 worldPos = transform.position;
        Vector3Int cellPosition = layoutGrid.WorldToCell(worldPos);
        Vector3 snapPos = layoutGrid.GetCellCenterWorld(cellPosition);
        Debug.Log("EndDrag");
        bodyColor.color = originalColor;
        FloorSelect.SetTile(cellPosition, null);

        if (Flip == false)
            FloorSelect.SetTile(cellPosition + new Vector3Int(0, 1, 0), null);
        else
            FloorSelect.SetTile(cellPosition + new Vector3Int(1, 0, 0), null);

        Draged = false;
        Debug.Log(cellPosition.y);
        if (!CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition) && cellPosition.y <= 2 && cellPosition.y > -8 && cellPosition.x <= 2 && cellPosition.x > -7)
        {
            transform.position = snapPos;
            CheckGrid.instance.PlaceObject(cellPosition);
            if (Size == SizeFurniture.large)
            {
                if (Flip == false)
                    CheckGrid.instance.PlaceObject(cellPosition + new Vector3Int(0, 1, 0));
                else
                    CheckGrid.instance.PlaceObject(cellPosition + new Vector3Int(1, 0, 0));
            }
            Debug.Log($"{cellPosition} empty");
        }
        else
        {
            transform.position = OriginalPosition;
            cellPosition = layoutGrid.WorldToCell(OriginalPosition);
            CheckGrid.instance.PlaceObject(cellPosition);
            if (Size == SizeFurniture.large)
            {
                if (Flip == false)
                    CheckGrid.instance.PlaceObject(cellPosition + new Vector3Int(0, 1, 0));
                else
                    CheckGrid.instance.PlaceObject(cellPosition + new Vector3Int(1, 0, 0));
            }
            Debug.Log($"{cellPosition} not empty");
        }

        if (Size == SizeFurniture.large)
        {
            SoundManager.instance.playSFX(SoundManager.instance.PutDownHeavyFur);
        }
        if (Size == SizeFurniture.small)
        {
            SoundManager.instance.playSFX(SoundManager.instance.PutDownFur);
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPoint");
    }

    void UpdateHighlight(Vector3Int cellPosition)
    {
        FloorSelect.SetTile(previousCellPos, null);
        FloorSelect.SetTile(cellPosition, highlightTile);
        previousCellPos = cellPosition;

        if (Size == SizeFurniture.large && Flip == false)
        {

            FloorSelect.SetTile(previousCellPos, null);
            FloorSelect.SetTile(previousCellPos2, null);
            FloorSelect.SetTile(cellPosition, highlightTile);
            FloorSelect.SetTile(cellPosition + new Vector3Int(0, 1, 0), highlightTile);
            previousCellPos = cellPosition;
            previousCellPos2 = cellPosition + new Vector3Int(0, 1, 0);
        }
        else if (Size == SizeFurniture.large && Flip == true)
        {
            FloorSelect.SetTile(previousCellPos, null);
            FloorSelect.SetTile(previousCellPos2, null);
            FloorSelect.SetTile(cellPosition, highlightTile);
            FloorSelect.SetTile(cellPosition + new Vector3Int(1, 0, 0), highlightTile);
            previousCellPos = cellPosition;
            previousCellPos2 = cellPosition + new Vector3Int(1, 0, 0);
        }
    }

}
