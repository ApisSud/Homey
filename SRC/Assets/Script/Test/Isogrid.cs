using UnityEngine;
using UnityEngine.Rendering;

public class Isogrid : MonoBehaviour
{
    [Header("Placement Settings")]
    [SerializeField] private Transform anchorPoint;

    [Header("Isometric Settings")]
    public float stepX = 0.5f; // ระยะห่างเฉียงไปทางขวาลง
    public float stepY = 0.25f; // ระยะห่างเฉียงไปทางขวาขึ้น (สำหรับ Isometric 2:1)
    public Vector2 directionVector = new Vector2(-0.5f, 0.25f);
    public static Isogrid Instance;

    public int columns = 5;
    public int rows = 1;
    public float spacing = 0.5f;

    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
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
    public Vector3 GetClosestSnapPoint(Vector3 mouseWorldPos, float snapThreshold = 2f)
    {
        Vector3 bestPoint = mouseWorldPos;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < columns; i++)
        {
            Vector3 slotPos = GetSlotPosition(i);
            Debug.Log(slotPos);
            float distance = Vector3.Distance(mouseWorldPos, slotPos);
            Debug.Log($"snap : {slotPos}+{i}");
            if (distance < snapThreshold && distance < closestDistance)
            {
                closestDistance = distance;
                bestPoint = slotPos;
            }
        }
        Debug.Log($"snap : {bestPoint}");
        return bestPoint;
    }
    public Vector3 GetSlotPosition(int i)
    {

        Vector3 offset = new Vector3(
            i * directionVector.x * spacing,
            i * directionVector.y * spacing,
            0
        );

        return anchorPoint.position + offset;
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

   


} 
