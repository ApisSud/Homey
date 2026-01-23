using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
public class Buiding_Test : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler
{
    [SerializeField] private Grid layoutGrid;
    [SerializeField] private Tilemap FloorSelect;
    [SerializeField] private TileBase highlightTile;
    private Rigidbody2D rb;
    private Vector3 offset;
    private Vector3 OriginalPosition;
    private Vector3Int previousCellPos;
    [SerializeField] private SpriteRenderer bodyColor;
    private Color32 originalColor;
    private bool Flip;
    private bool Draged;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector3 worldPos = transform.position;
        Vector3Int cellPosition = layoutGrid.WorldToCell(worldPos);
        Debug.Log(cellPosition);
        CheckGrid.instance.PlaceObject(cellPosition);
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
            }
            else if (Input.GetKeyDown(KeyCode.E) && Flip == true)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                Flip = false;
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
        if (cellPosition != previousCellPos && cellPosition.y < 3)
        {
            FloorSelect.SetTile(previousCellPos, null);
            FloorSelect.SetTile(cellPosition, highlightTile);
            previousCellPos = cellPosition;
        }
        if (cellPosition.y > 2)
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
        Draged = false;
        Debug.Log(cellPosition.y);
        if (!CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition) && cellPosition.y <= 2 && cellPosition.y > -8 && cellPosition.x <= 2 && cellPosition.x > -7)
        {
            transform.position = snapPos;
            CheckGrid.instance.PlaceObject(cellPosition);
            Debug.Log($"{cellPosition} empty");
        }
        else 
        {
            transform.position = OriginalPosition;
            cellPosition = layoutGrid.WorldToCell(OriginalPosition);
            CheckGrid.instance.PlaceObject(cellPosition);
            Debug.Log($"{cellPosition} not empty");
        }


    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPoint");
    }
}
