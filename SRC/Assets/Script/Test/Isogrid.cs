using UnityEngine;
using UnityEngine.Rendering;


public class Isogrid : MonoBehaviour
{
    [Header("Placement Settings")]
    [SerializeField] private Transform anchorPoint;

    [Header("Isometric Settings")]
    public float stepX = 0.5f; // ระยะห่างเฉียงไปทางขวาลง
    public float stepY = 0.25f; // ระยะห่างเฉียงไปทางขวาขึ้น (สำหรับ Isometric 2:1)
    public int layer = 4;

    public int columns = 5;
    public int rows = 1;
    public float spacing = 0.5f;
    public bool canplace;

    [SerializeField] private int x, y, row, column;

    void Start()
    {
        canplace = false;
        if(gameObject.layer == LayerMask.NameToLayer("FurnitureStorage"))
        {
            Vector3Int cellPosition = new Vector3Int(x, y, 0);
            combinePlaceGrid(row,column, cellPosition);
        }
    }

    public Vector3 GetIsoSlotPosition(int col, int row)
    {

        Vector3 isoOffset = new Vector3(
            (col - row) * stepX * spacing ,
            (col + row) * stepY * spacing,
            0
        );

        return anchorPoint.position + isoOffset;
    }
    public Vector3 GetClosestSnapPoint(Vector3 mouseWorldPos, float snapThreshold = 0.6f)
    {
        //Debug.Log($"mousePos2 : {mouseWorldPos}");
        Vector3 bestPoint = mouseWorldPos;
        float closestDistance = float.MaxValue;
        canplace = false;
        for (int i = 0; i < columns; i++)
        {
            for (int r = 0; r < rows; r++)
            {
                Vector3 slotPos = GetIsoSlotPosition(i,r);
                float distance = Vector3.Distance(mouseWorldPos, slotPos);
                Debug.Log($"Slotsnap : {slotPos}+{i}");
                if (distance < snapThreshold && distance < closestDistance)
                {
                    canplace = true;
                    closestDistance = distance;
                    bestPoint = slotPos;
                  
                }
               
            
            }
        }
        Debug.Log($"snap : {bestPoint}");
        return bestPoint;
    }
  
    void OnDrawGizmos()
    {
        if (anchorPoint == null) return;

        for (int c = 0; c < columns; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(GetIsoSlotPosition(c, r), 0.07f);
                
            }
        }
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

                CheckGrid.instance.PlaceObject(targetPos, $"Storage");
            }
        }
    }



} 
