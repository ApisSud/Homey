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

    public Sprite idleSprite;
    public Sprite draggingSprite;
    private SpriteRenderer spriteRenderer;

    private Vector3 offset;
    private bool isOverBin = false; 
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        UpdateScoreUI();

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

        // เช็คว่าปล่อยลงถังขยะหรือไม่
        if (isOverBin)
        {
            // ---- ถ้าลงถัง: ไม่ต้องเปลี่ยนรูปกลับ ปล่อยให้เป็นรูปกำลังลาก (ขยำ) ลงถังไปเลย ----
            ThrowInBin();
        }
        else
        {
            // ---- ถ้าพลาดเป้า (ไม่ลงถัง): ค่อยเปลี่ยนรูปลับมาเป็นรูปปกติ (idleSprite) ----
            if (spriteRenderer != null && idleSprite != null)
            {
                spriteRenderer.sprite = idleSprite;
            }

            // แล้วเด้งกลับที่เดิม
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

        if (GetComponent<Collider2D>() != null) GetComponent<Collider2D>().enabled = false;

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

