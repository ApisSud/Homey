using UnityEngine;
using DG.Tweening;

public class FlowerObject : MonoBehaviour
{
    [Header("flower sprites")]
    public Sprite bloomingSprite;
   

    private SpriteRenderer spriteRenderer;
   /* public GameObject flowerParticle;*/

    private bool isWatered = false;

    private Tween highlightTween;

    void Start()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartHighlight()
    {
        
        if (!isWatered)
        {
          
            highlightTween = transform.DOScale(1.1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
    }

    
    public void StopHighlight()
    {
        if (!isWatered)
        {
            highlightTween.Kill(); 
            transform.localScale = Vector3.one; 
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("WateringCan") && !isWatered)
        {

            highlightTween.Kill(); 

           
            spriteRenderer.sprite = bloomingSprite;

           
            transform.localScale = Vector3.zero;
            transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
           /* Instantiate(flowerParticle, transform.position, Quaternion.identity);*/

            isWatered = true;

            Debug.Log("Water done");
        }
    }
}
