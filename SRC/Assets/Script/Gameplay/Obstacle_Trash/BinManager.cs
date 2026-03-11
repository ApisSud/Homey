using UnityEngine;

public class BinManager : MonoBehaviour
{
    [Header("Trash Bin GameObject")]
    public GameObject trashBin;

    [Header("Spawn Point")]
    public Transform spawnPoint; 

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
}
