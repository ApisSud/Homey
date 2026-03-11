using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance; 

    [Header("UI Elements")]
    public GameObject successImage;
    public GameObject successImage2;

    [Header("Task Status")]
    public bool isTrashCleared = false;
    public bool isDirtCleared = false;

    [Header("Level Settings")]
    public int totalTrashNeeded = 4;

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
            successImage.transform.localScale = new Vector3(0, 1, 1);
        }
        if (successImage2 != null)
        {
            successImage2.SetActive(false);
            successImage2.transform.localScale = new Vector3(0, 1, 1);
        }
    }

    
    public void CompleteTrashTask()
    {
        if (isTrashCleared) return;

        isTrashCleared = true;
        CheckAllTasks();

        if (successImage != null)
        {
            successImage.SetActive(true);
            // ค่อยๆ ยืดแกน X ให้กลับมาเป็น 1 (ขนาดปกติ) ภายในเวลา 0.5 วินาที
            successImage.transform.DOScaleX(1f, 0.5f).SetEase(Ease.OutBack);
        }
    }

   
    public void CompleteDirtTask()
    {
        isDirtCleared = true;
        CheckAllTasks();

        {
            successImage2.SetActive(true);
            // ค่อยๆ ยืดแกน X 
            successImage2.transform.DOScaleX(1f, 0.5f).SetEase(Ease.OutBack);
        }
    }

  
    private void CheckAllTasks()
    {
        if (isTrashCleared && isDirtCleared)
        {
            Debug.Log("All Task done");
        
        }
    }
}
