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
/*
    [Header("Game Settings")]
  
    public GameObject trashParticle;*/

    [SerializeField] private TrashType type;

    [Header("UI Management")]
    public TextMeshProUGUI scoreText;

    public Sprite idleSprite;
    public Sprite draggingSprite;
    private SpriteRenderer spriteRenderer;

    [Header("Pop-up Animation Settings")]
    public float popScaleFactor = 1.2f;
    public float popDuration = 0.15f;
    private Vector3 originalScale;

    private Vector3 offset;
    private bool isOverBin = false; 
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        UpdateScoreUI();

        originalScale = transform.localScale;

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (idleSprite == null && spriteRenderer != null)
        {
            idleSprite = spriteRenderer.sprite;
        }
    }

   
    void OnMouseDown()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - mousePos;

        if (BinManager.Instance != null) BinManager.Instance.SetHighlight(true);
        
        if (spriteRenderer != null && draggingSprite != null)
        {
            spriteRenderer.sprite = draggingSprite;

            LeanTween.cancel(gameObject);
            LeanTween.scale(gameObject, originalScale * popScaleFactor, popDuration)
                .setEase(LeanTweenType.easeOutBack);
        }
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
           
            if (spriteRenderer != null && idleSprite != null)
            {
                spriteRenderer.sprite = idleSprite;
            }


            LeanTween.cancel(gameObject);
            LeanTween.move(gameObject, startPosition, 0.3f).setEase(LeanTweenType.easeOutBack);
            LeanTween.scale(gameObject, originalScale, 0.2f).setEase(LeanTweenType.easeOutQuad); 
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

        if (GetComponent<Collider2D>() != null) GetComponent<Collider2D>().enabled = false;

        LeanTween.scale(gameObject, Vector3.zero, 0.3f)
             .setEase(LeanTweenType.easeInBack)
             .setOnComplete(() =>
            {
               /* if (trashParticle != null)
                {
                    Instantiate(trashParticle, transform.position, Quaternion.identity);
                }*/
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

