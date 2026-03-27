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
    Grid1X1,
    Grid1X2,
    wall
}

public enum FurnitureType
{
    light,   
    Heavy,  
    Glass  
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
    private Vector3 finalPos;
    [SerializeField] private SpriteRenderer bodyColor;
    private Color32 originalColor;
    private bool Flip;
    private bool Draged;
    private bool onStorageFur;
    private bool onWall;
    [SerializeField] private bool canMove;
    
    [SerializeField]
    private SizeFurniture Size;
    [SerializeField]
    private FurnitureType Type;


    private Isogrid currentTargetGrid;

    [SerializeField]
    private int rowGrid;
    [SerializeField]
    private int columnGrid;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector3 worldPos = transform.position;
        Vector3Int cellPosition = layoutGrid.WorldToCell(worldPos);
        Debug.Log(cellPosition);
        onStorageFur = false;
        onWall = false;
        CheckGrid.instance.PlaceObject(cellPosition, $"{Type}");
        if (GameManager.instance.IsWithinBounds(cellPosition))
        {
            CheckGrid.instance.addFurnitureInScene(cellPosition);
        }
        if (Size == SizeFurniture.Grid1X2)
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
        //Debug.Log($"BeginDrag{cellPosition} mousePos :{mousePos}");
        offset = transform.position - mousePos;
        CheckGrid.instance.RemoveObject(cellPosition);
        CheckGrid.instance.removeFurnitureInScene(cellPosition);
        
        if (Size == SizeFurniture.small && onStorageFur)
        {
            //finalPos = currentTargetGrid.GetClosestSnapPoint(mousePos);
            //Debug.Log($"Remove {finalPos}");
            CheckGrid.instance.RemoveObject(finalPos);
            CheckGrid.instance.removeFurnitureInScene(finalPos);
        }
        if (Size == SizeFurniture.wall && onWall)
        {
            //finalPos = currentTargetGrid.GetClosestSnapPoint(mousePos);
            //Debug.Log($"Remove {finalPos}");
            CheckGrid.instance.RemoveObject(finalPos);
            CheckGrid.instance.removeFurnitureInScene(finalPos);
        }
        if (rowGrid > 1 | columnGrid > 1)
        {
            if (Flip == false)
            {
                combineRemoveGrid(rowGrid, columnGrid, cellPosition); 
            }
            if (Flip == true)
            {
                combineRemoveGrid(columnGrid, rowGrid, cellPosition);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canMove)
        {
            Vector3 screenPos = eventData.position;
            screenPos.z = 10f;

            Vector3 worldPos = transform.position;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(screenPos);
            mousePos.z = 0;
            Vector3Int cellPosition = layoutGrid.WorldToCell(mousePos);
            Vector3 snapPos = layoutGrid.GetCellCenterWorld(cellPosition);
            Color tempColor = sr.color;
            /*transform.position = eventData.position;
            transform.position = mousePos + offset;*/

            Draged = true;
            if (Size == SizeFurniture.small)
            {
                sr.sortingOrder = 3;
                if (!CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition))
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
                else if (!onStorageFur)
                {
                    transform.position = snapPos;
                }

            }
            if (Size == SizeFurniture.wall)
            {
                sr.sortingOrder = 3;
                if (!CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition))
                {
                    transform.position = mousePos;
                }
                if (onWall)
                {
                    Debug.Log("onWall");
                    Debug.Log($"worldPos1 : {mousePos}");

                    finalPos = currentTargetGrid.GetClosestSnapPoint(mousePos);
                    transform.position = finalPos;
                    Debug.Log($"fur will snap : {finalPos}");
                }
                else if (!onWall)
                {
                    transform.position = snapPos;
                }

            }
          
            if (Size == SizeFurniture.Grid1X1)
            {
                offset = new Vector3(layoutGrid.cellSize.x / 2f, 0, layoutGrid.cellSize.y / 2f);
                transform.position = snapPos + offset;
                finalPos = snapPos + offset;
            }
            if (Size == SizeFurniture.Grid1X2)
            {
                // 2. หาตำแหน่งกึ่งกลางของช่องแรกนั้น
                Vector3 baseSnapPos = layoutGrid.GetCellCenterWorld(cellPosition);

                float offsetX = ((rowGrid - 1) * layoutGrid.cellSize.x) / 2f;
                float offsetZ = ((columnGrid - 1) * layoutGrid.cellSize.y) / 2f;
                Vector3 totalOffset = new Vector3(offsetX, 0, offsetZ);
                transform.position = baseSnapPos + totalOffset;
                finalPos = baseSnapPos + totalOffset;
            }

            if (!CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition))
            {
                sr.sortingOrder = 2;
                tempColor.a = 1f;
                sr.color = tempColor;
            }
            else if (CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition))
            {
                sr.sortingOrder = 3;
                tempColor.a = 0.5f;
                sr.color = tempColor;
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
     

        Draged = false;
        if (!CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition) && GameManager.instance.IsWithinBounds(cellPosition))
        {
            //transform.position = snapPos;
            CheckGrid.instance.PlaceObject(cellPosition, $"{Type}");
            CheckGrid.instance.addFurnitureInScene(cellPosition);

            if(rowGrid > 1 | columnGrid > 1)
            {
            
                //offset = new Vector3(layoutGrid.cellSize.x / 2f, 0, layoutGrid.cellSize.y / 2f);
                transform.position = finalPos;
                combinePlaceGrid(rowGrid, columnGrid, cellPosition);
                if(Flip == true)
                {
                    combinePlaceGrid(columnGrid, rowGrid, cellPosition);
                }
            }
           
            Debug.Log($"{cellPosition} empty");
        } 
        else
        {
            transform.position = OriginalPosition;
            cellPosition = layoutGrid.WorldToCell(OriginalPosition);
            CheckGrid.instance.PlaceObject(cellPosition, $"{Type}");
    
            
            if(GameManager.instance.IsWithinBounds(cellPosition))
            {
                CheckGrid.instance.addFurnitureInScene(cellPosition);
            }
            Debug.Log($"{cellPosition} not empty");
        }

        if(onStorageFur | onWall)
        {  if (!CheckGrid.instance.occupiedTiles.ContainsKey(finalPos))
            {
                transform.position = finalPos;
                CheckGrid.instance.PlaceObject(finalPos, $"{Type}");
                CheckGrid.instance.addFurnitureInScene(finalPos);
            }
            else if(CheckGrid.instance.occupiedTiles.ContainsKey(finalPos))
            {
                transform.position = OriginalPosition;
                cellPosition = layoutGrid.WorldToCell(OriginalPosition);
                CheckGrid.instance.PlaceObject(cellPosition, $"{Type}");
            }
        }



        if (SoundManage.Instance != null)
        {
            SoundManage.Instance.PlayFurnitureSFX(Type);
        }
        /*
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
        */
    }

   
  
    private void OnTriggerStay2D(Collider2D Furniture)
    {

        if (Furniture.gameObject.layer == LayerMask.NameToLayer("FurnitureStorage"))
        {
            currentTargetGrid = Furniture.GetComponent<Isogrid>();
            onStorageFur = true;
        }
        if (Furniture.gameObject.layer == LayerMask.NameToLayer("wall"))
        {
            currentTargetGrid = Furniture.GetComponent<Isogrid>();
            onWall = true;
        }

    }

    private void OnTriggerExit2D(Collider2D Furniture)
    {
        currentTargetGrid = null;
        onStorageFur = false;
    }

    private void combinePlaceGrid(int rows, int columns, Vector3Int cellPosition)
    {
        Debug.Log($"input : {cellPosition}");
        for (int c = 0; c < columns; c++)
        {
            // วนลูปแนวแถว
            for (int r = 0; r < rows; r++)
            {
                Vector3Int targetPos = new Vector3Int(cellPosition.x - c, cellPosition.y - r, 0);

                CheckGrid.instance.PlaceObject(targetPos, $"{Type}");
            }
        }
    }
    private void combineRemoveGrid(int rows, int columns, Vector3Int cellPosition)
    {
        Debug.Log($"input : {cellPosition}");
        for (int c = 0; c < columns; c++)
        {
            // วนลูปแนวแถว
            for (int r = 0; r < rows; r++)
            {
                Vector3Int targetPos = new Vector3Int(cellPosition.x - c, cellPosition.y - r, 0);

                CheckGrid.instance.RemoveObject(targetPos);
            }
        }
    }

  

}
