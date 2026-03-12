using UnityEngine;

public class FlowerObject : MonoBehaviour
{
    [Header("flower sprites")]
    public Sprite bloomingSprite;

    private SpriteRenderer spriteRenderer;
    public GameObject flowerParticle;

    void Start()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("WateringCan"))
        {
            Instantiate(flowerParticle, transform.position, Quaternion.identity);
            spriteRenderer.sprite = bloomingSprite;
        }
    }
}
