using UnityEngine;

public class FlowerObject : MonoBehaviour
{
    [Header("flower sprites")]
    public Sprite bloomingSprite;

    private SpriteRenderer spriteRenderer;
    public GameObject flowerParticle;

    private bool isWatered = false;

    void Start()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("WateringCan") && !isWatered)
        {
            
            spriteRenderer.sprite = bloomingSprite;
            Instantiate(flowerParticle, transform.position, Quaternion.identity);

            isWatered = true;

            Debug.Log("Water done");
        }
    }
}
