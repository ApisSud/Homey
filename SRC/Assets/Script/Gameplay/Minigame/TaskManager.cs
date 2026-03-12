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

    [Header("Buttons Setup")] 
    public GameObject BroomButton; 
    public GameObject TrashbinButton; 
    public GameObject FurnitureButton;

    [Header("Task Status")]
    public bool isTrashCleared = false;
    public bool isDirtCleared = false;
    public bool  isMouseClear = false;
    private bool allTasksCompleted = false;


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
        if (BroomButton != null) BroomButton.SetActive(true);   // 嗷源回枇 1
        if (TrashbinButton != null) TrashbinButton.SetActive(true);   // 嗷源回枇 2
        if (FurnitureButton != null) FurnitureButton.SetActive(false);
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
            
            successImage.transform.DOScaleX(1f, 0.5f).SetEase(Ease.OutBack);
        }
    }

   
    public void CompleteDirtTask()
    {
        isDirtCleared = true;
        CheckAllTasks();

        {
            successImage2.SetActive(true);
       
            successImage2.transform.DOScaleX(1f, 0.5f).SetEase(Ease.OutBack);
        }
    }

  
    private void CheckAllTasks()
    {
        if (isTrashCleared && isDirtCleared && isMouseClear)
        {
            Debug.Log("All Task done");
        

            if ( BroomButton != null) BroomButton.SetActive(false); 
            if (TrashbinButton != null) TrashbinButton.SetActive(false); 

            if (FurnitureButton != null)
            {
               FurnitureButton.SetActive(true); 
 
            }
        }
    }
}
