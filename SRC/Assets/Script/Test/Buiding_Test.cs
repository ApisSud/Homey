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
    Grid1X2
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
    private Vector3 finalPos;
    [SerializeField] private SpriteRenderer bodyColor;
    private Color32 originalColor;
    private bool Flip;
    private bool Draged;
    private bool onStorageFur;
    [SerializeField] private bool canMove;
    
    [SerializeField]
    private SizeFurniture Size;
    [SerializeField]
    private TypeFurniture Type;

    private Isogrid currentTargetGrid;

    [SerializeField]
    private int rowGrid;
    [SerializeField]
    private int columnGrid;

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
        if (Size == SizeFurniture.Grid1X2)
        {
            if (Flip == false)
                CheckGrid.instance.RemoveObject(cellPosition + new Vector3Int(0, 1, 0));
            else
                CheckGrid.instance.RemoveObject(cellPosition + new Vector3Int(1, 0, 0));
        }
        if (Size == SizeFurniture.small && onStorageFur)
        {
            //finalPos = currentTargetGrid.GetClosestSnapPoint(mousePos);
            //Debug.Log($"Remove {finalPos}");
            CheckGrid.instance.RemoveObject(finalPos);
        }
        if (Size == SizeFurniture.Grid1X1)
        {
            combineRemoveGrid(2,2,cellPosition);
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
        if (Size == SizeFurniture.small)
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
           
        }
        if (Size != SizeFurniture.small)
        {
            transform.position = snapPos;
            if (cellPosition != previousCellPos && GameManager.instance.IsWithinBounds(cellPosition))
            {
                //UpdateHighlight(cellPosition, FloorSelect);

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
        if (Size == SizeFurniture.Grid1X1)
        {
            offset = new Vector3(layoutGrid.cellSize.x / 2f, 0, layoutGrid.cellSize.y / 2f);
            transform.position = snapPos + offset;
            finalPos = snapPos + offset;
            //combineGridUpdateHighlight(2, 2, cellPosition);
        }
        if (Size == SizeFurniture.Grid1X2)
        {
            // 2. หาตำแหน่งกึ่งกลางของช่องแรกนั้น
            Vector3 baseSnapPos = layoutGrid.GetCellCenterWorld(cellPosition);

            float offsetX = ((2 - 1) * layoutGrid.cellSize.x) / 2f;
            float offsetZ = ((8 - 1) * layoutGrid.cellSize.y) / 2f; 
            Vector3 totalOffset = new Vector3(offsetX, 0, offsetZ);
            transform.position = baseSnapPos + totalOffset;
            finalPos = baseSnapPos + totalOffset;
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
        //FloorSelect.SetTile(cellPosition, null);

     /*   if (Flip == false)
            FloorSelect.SetTile(cellPosition + new Vector3Int(0, 1, 0), null);
        else
            FloorSelect.SetTile(cellPosition + new Vector3Int(1, 0, 0), null);*/

        Draged = false;
        if (!CheckGrid.instance.occupiedTiles.ContainsKey(cellPosition) && GameManager.instance.IsWithinBounds(cellPosition))
        {
            //transform.position = snapPos;
            CheckGrid.instance.PlaceObject(cellPosition, $"{Type}");
            CheckGrid.instance.addFurnitureInScene(cellPosition);

            if(Size == SizeFurniture.Grid1X1)
            {
            
                //offset = new Vector3(layoutGrid.cellSize.x / 2f, 0, layoutGrid.cellSize.y / 2f);
                transform.position = finalPos;
                combinePlaceGrid(2, 2, cellPosition);
            }
            if (Size == SizeFurniture.Grid1X2)
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
    
            if (Size == SizeFurniture.Grid1X2)
            {
                if (Flip == false)
                    CheckGrid.instance.PlaceObject(cellPosition + new Vector3Int(0, 1, 0), $"{Type}");
                else
                    CheckGrid.instance.PlaceObject(cellPosition + new Vector3Int(1, 0, 0), $"{Type}");
            }
            if(GameManager.instance.IsWithinBounds(cellPosition))
            {
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

    private void combinePlaceGrid(int rows, int columns, Vector3Int cellPosition)
    {
        Debug.Log($"input : {cellPosition}");
        for (int c = 0; c < columns; c++)
        {
            // วนลูปแนวแถว
            for (int r = 0; r < rows; r++)
            {
                Vector3Int targetPos = new Vector3Int(cellPosition.x - c, cellPosition.y - r, cellPosition.z);

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
                Vector3Int targetPos = new Vector3Int(cellPosition.x - c, cellPosition.y - r, cellPosition.z);

                CheckGrid.instance.RemoveObject(targetPos);
            }
        }
    }

    /* void UpdateHighlight(Vector3Int cellPosition, Tilemap highlight_Tile)
   {
       //highlight_Tile.SetTile(previousCellPos, null);
       highlight_Tile.SetTile(cellPosition, highlightTile);
       previousCellPos = cellPosition;


       if (Size == SizeFurniture.Grid1X2 && Flip == false)
       {

           highlight_Tile.SetTile(previousCellPos, null);
           highlight_Tile.SetTile(previousCellPos2, null);
           highlight_Tile.SetTile(cellPosition, highlightTile);
           highlight_Tile.SetTile(cellPosition + new Vector3Int(0, 1, 0), highlightTile);
           previousCellPos = cellPosition;
           previousCellPos2 = cellPosition + new Vector3Int(0, 1, 0);
       }
       else if (Size == SizeFurniture.Grid1X2 && Flip == true)
       {
           highlight_Tile.SetTile(previousCellPos, null);
           highlight_Tile.SetTile(previousCellPos2, null);
           highlight_Tile.SetTile(cellPosition, highlightTile);
           highlight_Tile  .SetTile(cellPosition + new Vector3Int(1, 0, 0), highlightTile);
           previousCellPos = cellPosition;
           previousCellPos2 = cellPosition + new Vector3Int(1, 0, 0);
       }
   }

   void combineGridUpdateHighlight(int rows, int columns, Vector3Int cellPosition)
   {
       //UpdateHighlight(cellPosition , FloorSelect);
       clearHightlight(rows, columns, previousCellPos);
       for (int i = 0; i < columns; i++)
       {
           UpdateHighlight(cellPosition - new Vector3Int(0, i, 0), FloorSelect);
       }
       for (int r = 0; r < rows; r++)
       {
           UpdateHighlight(cellPosition - new Vector3Int(-1, r, 0), FloorSelect);
       }


   }
   void clearHightlight(int rows, int columns, Vector3Int cellPosition)
   {
      *//* for (int i = 0; i < columns; i++)
       {
           FloorSelect.SetTile(cellPosition - new Vector3Int(0, i, 0), null);
       }
       for (int r = 0; r < rows; r++)
       {
           FloorSelect.SetTile(cellPosition - new Vector3Int(-1, r, 0), null);
       }*//*
       Debug.Log("clearHighlight");

       for (int x = 0; x < columns; x++)
       {
           for (int y = 0; y < rows; y++)
           {
               // คำนวณตำแหน่งที่จะลบโดยอิงจากจุดเริ่ม (cellPosition)
               // หมายเหตุ: ใน Isometric บางทีอาจต้องลบ (x, y) หรือ (-x, -y) 
               // ขึ้นอยู่กับว่าคุณใช้วิธีไหนในการวาด Highlight ขึ้นมา
               Vector3Int targetPos = new Vector3Int(cellPosition.x - x, cellPosition.y - y, cellPosition.z);

               FloorSelect.SetTile(targetPos, null);
           }
       }
   }*/

}
