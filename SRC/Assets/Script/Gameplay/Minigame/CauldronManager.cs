using UnityEngine;

public class CauldronManager : MonoBehaviour
{
    public static CauldronManager Instance;

    [Header("Highlight Settings")]
   
    public GameObject cauldronHighlight;

    /* [SerializeField]
     private Animator binAnimator;*/
    [SerializeField] private int x, y, row, column;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Vector3Int cellPosition = new Vector3Int(x, y, 0);
        combinePlaceGrid(row, column, cellPosition);
        if (cauldronHighlight != null)
        {
            cauldronHighlight.SetActive(false);
        }
    }
  


    public void SetHighlight(bool isShow)
    {
       
        if (cauldronHighlight != null)
        {
            cauldronHighlight.SetActive(isShow);
        }
    }

    private void combinePlaceGrid(int rows, int columns, Vector3Int cellPosition)
    {
        //Debug.Log($"input : {cellPosition}");
        for (int c = 0; c < columns; c++)
        {
            // Ç¹ÅÙ»á¹Çá¶Ç
            for (int r = 0; r < rows; r++)
            {
                Vector3Int targetPos = new Vector3Int(cellPosition.x - c, cellPosition.y - r, 0);

                CheckGrid.instance.PlaceObject(targetPos, $"Storage");
            }
        }
    }

    /*public void playAnimation()
    {
        binAnimator.SetTrigger("OnDrop");
        Debug.Log("Trigger ja");

    }*/


}
