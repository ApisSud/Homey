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
        //GridSelect.gameObject.SetActive(false);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(eventData.position);
        Debug.Log("BeginDrag");
        offset = transform.position - mousePos;

    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPos = transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(eventData.position);
        Debug.Log("OnDrag");
        transform.position = eventData.position;
        transform.position = mousePos + offset;

        Vector3Int cellPosition = layoutGrid.WorldToCell(worldPos);

        if (cellPosition != previousCellPos)
        {
            FloorSelect.SetTile(previousCellPos, null);
            FloorSelect.SetTile(cellPosition, highlightTile);
            previousCellPos = cellPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 worldPos = transform.position;
        Vector3Int cellPosition = layoutGrid.WorldToCell(worldPos);
        Vector3 snapPos = layoutGrid.GetCellCenterWorld(cellPosition);
        transform.position = snapPos;
        FloorSelect.SetTile(cellPosition, null);
        Debug.Log("EndDrag");
        //transform.position = offset;


    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPoint");
    }
}
