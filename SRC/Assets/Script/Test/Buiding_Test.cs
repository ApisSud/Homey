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
    private Vector3 offset;
    private Vector3Int previousCellPos;

    void Start()
    {
        Vector3 worldPos = transform.position;
        Vector3Int cellPosition = layoutGrid.WorldToCell(worldPos);
        Debug.Log(worldPos);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {

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

        if (cellPosition != previousCellPos)
        {
            FloorSelect.SetTile(previousCellPos, null);
            FloorSelect.SetTile(cellPosition, highlightTile);
            previousCellPos = cellPosition;
        }

        //Debug.Log($"OnDrag{cellPosition}");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 worldPos = transform.position;
        Vector3Int cellPosition = layoutGrid.WorldToCell(worldPos);
        Vector3 snapPos = layoutGrid.GetCellCenterWorld(cellPosition);
        Debug.Log("EndDrag");

        if(CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition) == null)
        {
            transform.position = snapPos;
            FloorSelect.SetTile(cellPosition, null);
            CheckGrid.instance.PlaceObject(cellPosition);
            Debug.Log($"{cellPosition} empty");
        }
        else if (CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition) != null)
        {
            Debug.Log($"{cellPosition} not empty");
        }


    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPoint");
    }
}
