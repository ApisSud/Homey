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
    public GameObject Trashbin;
    public GameObject Checkbox1;
    public GameObject Checkbox2;
    public GameObject Checkbox3;

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
    private int currentMouseCount = 0;
    public GameObject trashBin;


    [Header("Tutorial System")]
    public FurnitureTutorial furnitureTutorialObj;

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

        if (Checkbox1 != null) { Checkbox1.SetActive(false); }
        if (Checkbox2 != null) { Checkbox2.SetActive(false);}
        if (Checkbox3 != null) { Checkbox3.SetActive(false);  }

        if (BroomButton != null) BroomButton.SetActive(true);   
        if (TrashbinButton != null) TrashbinButton.SetActive(true);   
        if (FurnitureButton != null) FurnitureButton.SetActive(false);
    }


    public void AddMouseCount()
    {
        if (isMouseClear) return;
        currentMouseCount++;


       
        if (currentMouseCount >= totalMouseNeeded)
        {
            isMouseClear = true; 
            CheckAllTasks();     
            if (successImage3 != null)
            {
                successImage3.SetActive(true);
                
                successImage3.transform.DOScaleX(1f, 0.7f).SetEase(Ease.OutBack);
            }
            if (Checkbox3 != null)
            {
                Checkbox3.SetActive(true);

            }
        }

       


    }
    public void CompleteTrashTask()
    {
        if (isTrashCleared) return;

        isTrashCleared = true;

        if (trashBin != null && trashBin.activeSelf)
        {
            
            if (trashBin.GetComponent<TrashBin>() != null)
            {
                LeanTween.scale(trashBin, Vector3.zero, 0.3f)
                    .setEase(LeanTweenType.easeInBack)
                    .setOnComplete(() => trashBin.SetActive(false));
            }
           
            else if (trashBin.GetComponent<CauldronManager>() != null)
            {
                
                Debug.Log("โยนลงหม้อครบแล้ว! (หม้อไม่หายไป)");
            }
        }

        CheckAllTasks();

        if (successImage != null)
        {
            successImage.SetActive(true);
            
            successImage.transform.DOScaleX(1f, 0.5f).SetEase(Ease.OutBack);
        }

        if (Checkbox1 != null)
        {
            Checkbox1.SetActive(true);
           
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
        if (Checkbox2 != null)
        {
            Checkbox2.SetActive(true);
          
        }
    }


    private void CheckAllTasks()
    {
        if (isTrashCleared && isDirtCleared && isMouseClear && !allTasksCompleted)
        {
            allTasksCompleted = true; 
            Debug.Log("All Task done");

            if (BroomButton != null)
            {
                BroomButton.transform.DOScale(0f, 0.8f).SetEase(Ease.InBack).OnComplete(() => BroomButton.SetActive(false));
            }
            if (TrashbinButton != null)
            {
                TrashbinButton.transform.DOScale(0f, 0.8f).SetEase(Ease.InBack).OnComplete(() => TrashbinButton.SetActive(false));
            }

            DOVirtual.DelayedCall(2f, () =>
            {
                if (FurnitureButton != null)
                {
                    FurnitureButton.SetActive(true);
                    FurnitureButton.transform.localScale = Vector3.zero;

                   
                    FurnitureButton.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                    {
                       
                        DOVirtual.DelayedCall(0.3f, () =>
                        {
                            if (furnitureTutorialObj != null)
                            {
                                furnitureTutorialObj.ShowTutorial();
                            }
                        });
                    });
                }
            });
        }
    }
}
