using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance; 

    [Header("UI Elements")]
    public GameObject successImage; 

    [Header("Task Status")]
    public bool isTrashCleared = false;
    public bool isDirtCleared = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {

        if (successImage != null)
        {
            successImage.SetActive(false);
        }
    }

    
    public void CompleteTrashTask()
    {
        isTrashCleared = true;
        CheckAllTasks();

        if (successImage != null)
        {
            successImage.SetActive(true);
        }
    }

   
    public void CompleteDirtTask()
    {
        isDirtCleared = true;
        CheckAllTasks();

    }

  
    private void CheckAllTasks()
    {
        if (isTrashCleared && isDirtCleared)
        {
            Debug.Log("All Task done");
        
        }
    }
}
