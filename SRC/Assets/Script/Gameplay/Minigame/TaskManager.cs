using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance; 

    [Header("UI Elements")]
    public GameObject successImage;
    public GameObject successImage2;
    public GameObject successImage3;

    [Header("Task Status")]
    public bool isTrashCleared = false;
    public bool isDirtCleared = false;
    public bool  isMouseClear = false;
 

    [Header("Level Settings")]
    public int totalTrashNeeded = 4;
    public int totalMouseNeeded = 0;




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

        if (successImage3 != null)
        {
            successImage3.SetActive(false);
            successImage3.transform.localScale = new Vector3(0, 1, 1);
        }
    }


    public void AddMouseCount()
    {
        if (isMouseClear) return;
        isMouseClear = true;

        CheckAllTasks();

        if (successImage != null)
        {
            successImage3.SetActive(true);
            
            successImage3.transform.DOScaleX(1f, 0.5f).SetEase(Ease.OutBack);
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
        if (isTrashCleared && isDirtCleared && isMouseClear)
        {
            Debug.Log("All Task done");
        
        }
    }
}
