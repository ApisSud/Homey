using UnityEngine;
using UnityEngine.Rendering;

public class Isogrid : MonoBehaviour
{
    [Header("Placement Settings")]
    [SerializeField] private Transform anchorPoint;

    [Header("Isometric Settings")]
    public float stepX = 0.5f; // ระยะห่างเฉียงไปทางขวาลง
    public float stepY = 0.25f; // ระยะห่างเฉียงไปทางขวาขึ้น (สำหรับ Isometric 2:1)

    public int columns = 5;
    public int rows = 1;
    public float spacing = 0.5f;

    void Start()
    {
        
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
