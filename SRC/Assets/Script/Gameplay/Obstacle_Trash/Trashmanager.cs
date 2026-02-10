using UnityEngine;
using TMPro;

public class Trashmanager : MonoBehaviour
{
    public static Trashmanager instance; 
    public TextMeshProUGUI scoreText; 
    public int totalTrash = 0;
    void Awake()
    {
        instance = this; 
    }

    void Start()
    {
        UpdateScoreUI();
    }

    public void CollectTrash()
    {
        totalTrash++; 
        UpdateScoreUI(); 
     
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Total Trash :  " + totalTrash + " / 4";
    }
}
