using UnityEngine;

public class BinManager : MonoBehaviour
{
    public static BinManager Instance;

    [Header("Trash Bin GameObject")]
    public GameObject trashBin;

    [Header("Spawn Point")]
    public Transform spawnPoint;

    [Header("Highlight Settings")]
    public GameObject binHighlight;
 



    private void Awake()
    {
        // ตั้งค่า Instance
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // ตอนเริ่มเกม ให้ซ่อนไฮไลต์เอาไว้ก่อน
        if (binHighlight != null)
        {
            binHighlight.SetActive(false);
        }

       

       
    }


    public void ToggleTrashBin()
    {
        if (trashBin.activeSelf)
        {
          
            LeanTween.scale(trashBin, Vector3.zero, 0.3f)
                .setEase(LeanTweenType.easeInBack)
                .setOnComplete(() => trashBin.SetActive(false));
        }
        else
        {
           
            if (spawnPoint != null)
            {
                trashBin.transform.position = spawnPoint.position;
            }

            
            trashBin.SetActive(true);
            trashBin.transform.localScale = Vector3.zero;
            LeanTween.scale(trashBin, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutBack);
        }
    }

    public void SetHighlight(bool isShow)
    {
       
        if (binHighlight != null && trashBin.activeSelf)
        {
            binHighlight.SetActive(isShow);
        }

        
    }
}
