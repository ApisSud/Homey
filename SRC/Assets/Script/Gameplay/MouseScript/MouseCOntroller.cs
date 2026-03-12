using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MouseCOntroller : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform[] waypoints; 
    public float speed = 3f;
    public static int totalMouse = 0;
    public GameObject MouseParticle;

    private int currentWaypointIndex = 0;

    [Header("UI Management")]
    public TextMeshProUGUI scoreText;


    void Start()
    {
        UpdateScoreUI();    
    }

    void Update()
    {
        
        if (waypoints.Length == 0) return;

        
        transform.position = Vector2.MoveTowards(
            transform.position,
            waypoints[currentWaypointIndex].position,
            speed * Time.deltaTime
        );

      
        if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
         
            currentWaypointIndex++;

          
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
    }

   
    void OnMouseDown()
    {
       if (TaskManager.Instance != null)
        {
            TaskManager.Instance.AddMouseCount();
        }
        LeanTween.scale(gameObject, Vector3.zero, 0.3f)
             .setEase(LeanTweenType.easeInBack)
             .setOnComplete(() =>
             {
                 if (MouseParticle != null)
                 {
                     Instantiate(MouseParticle, transform.position, Quaternion.identity);
                 }
                 Destroy(gameObject);
             });
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Total Mouse : " + totalMouse + " / " + TaskManager.Instance.totalMouseNeeded;
        }
    }
}
