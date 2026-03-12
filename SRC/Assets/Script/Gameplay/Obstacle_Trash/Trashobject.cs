using UnityEngine;
using TMPro;

public class Trashobject : MonoBehaviour
{
    public enum TrashType
    {
        water,
        glass,
        bags
    }
    public static int totalTrash = 0;

    [Header("Game Settings")]
  
   

    public GameObject trashParticle;
    [SerializeField] private TrashType type;

    [Header("UI Management")]
    public TextMeshProUGUI scoreText;

    

    
    private Vector3 offset;
    private bool isOverBin = false; 
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        UpdateScoreUI();
    }

   
    void OnMouseDown()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - mousePos;

        if (BinManager.Instance != null) BinManager.Instance.SetHighlight(true);
    }

    void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x + offset.x, mousePos.y + offset.y, transform.position.z);
    }

    
    void OnMouseUp()
    {

        if (BinManager.Instance != null) BinManager.Instance.SetHighlight(false);

        if (isOverBin)
        {
           
            ThrowInBin();
        }
        else
        {
           
            LeanTween.move(gameObject, startPosition, 0.3f).setEase(LeanTweenType.easeOutBack);
        }
    }

   
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TrashBin"))
        {
            isOverBin = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("TrashBin"))
        {
            isOverBin = false;
        }
    }

 
    void ThrowInBin()
    {
        totalTrash++;
        UpdateScoreUI();

        if (TaskManager.Instance != null && totalTrash >= TaskManager.Instance.totalTrashNeeded)
        {
            TaskManager.Instance.CompleteTrashTask();
        }

        GetComponent<Collider2D>().enabled = false;

        LeanTween.scale(gameObject, Vector3.zero, 0.3f)
            .setEase(LeanTweenType.easeInBack)
            .setOnComplete(() =>
            {
                if (trashParticle != null)
                {
                    Instantiate(trashParticle, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            });
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Total Trash : " + totalTrash + " / " + TaskManager.Instance.totalTrashNeeded;
        }
    }
    
    
    /*if (Type == TrashType.water)
    {
        SoundManager.instance.playSFX(SoundManager.instance.Dirtywater);
    }*/

}

