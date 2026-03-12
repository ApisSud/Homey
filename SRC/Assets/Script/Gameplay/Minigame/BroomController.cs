using UnityEngine;
using UnityEngine.EventSystems;

public class BroomController : MonoBehaviour
{
    [Header("Settings")]
    public bool isEquipped = false; 
    private SpriteRenderer spriteRenderer;
  
    /*public GameObject Outparticle;*/

   public GameObject broomImage;
    private Vector3 originalScale;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        originalScale = transform.localScale;
    }

    void Update()
    {
        
        if (isEquipped)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
           
            Cursor.visible = true;
        }
       

}

    public void ToggleBroom()
    {
        Cursor.visible = true;
        if (isEquipped)
        {
            UnequipBroom(); 
        }
        else
        {
            EquipBroom();  
        }
    }
    public void EquipBroom()
    {
        LeanTween.cancel(gameObject);

        isEquipped = true;
        spriteRenderer.enabled = true; 
        Cursor.visible = false;

       
        transform.localScale = Vector3.zero;

        LeanTween.scale(gameObject, originalScale, 0.5f).setEase(LeanTweenType.easeOutBack);
    }

    public void UnequipBroom()
    {
       
        LeanTween.cancel(gameObject);

        isEquipped = false;
        Cursor.visible = true;

       
        LeanTween.scale(gameObject, Vector3.zero, 0.5f)
            .setEase(LeanTweenType.easeInBack)
            .setOnComplete(() =>
            {
             
                spriteRenderer.enabled = false;
              /*  Instantiate(Outparticle, transform.position, Quaternion.identity);*/
            });
    }

  
    
}
