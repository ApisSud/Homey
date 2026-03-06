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

    [Header("Trash Properties")]
    public GameObject trashParticle;
    [SerializeField] private TrashType type;

    [Header("UI Management")]
    public TextMeshProUGUI scoreText; 

    
    public static int totalTrash = 0;

    void Start()
    {
        
        UpdateScoreUI();
    }

    void OnMouseDown()
    {
      
        totalTrash++;
        UpdateScoreUI();

       
        LeanTween.scale(gameObject, Vector3.zero, 0.5f)
            .setEase(LeanTweenType.easeInBack)
            .setOnComplete(() =>
            {
                Destroy(gameObject);
                if (trashParticle != null)
                {
                    Instantiate(trashParticle, transform.position, Quaternion.identity);
                }
            });
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Total Trash : " + totalTrash + " / 4";
        }
    }
    /*if (Type == TrashType.water)
    {
        SoundManager.instance.playSFX(SoundManager.instance.Dirtywater);
    }*/





}

