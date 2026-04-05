using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


[System.Serializable]
public class TutorialPage
{
    [TextArea(3, 5)]
    public string descriptionText;
    public Sprite tutorialImage;
}
public class TutorialManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject tutorialPanel;
    public Image displayImage;
    public GameObject darkPanel;
    public TextMeshProUGUI displayText;

    public Button nextButton;
    public Button prevButton;
    public Button confirmButton;

    [Header("Settings")]
    public float startDelay = 1.5f;

    [Header("Tutorial Content")]
    public TutorialPage[] pages;

    private int currentPageIndex = 0;

    [Header("Animation Settings")]
    public float popDuration = 0.5f;
    public Ease openEase = Ease.OutBack;
    public Ease closeEase = Ease.InBack;

    private RectTransform panelRectTransform;
    void Awake()
    {
      
        if (tutorialPanel != null)
            panelRectTransform = tutorialPanel.GetComponent<RectTransform>();
    }

    void Start()
    {
        
        tutorialPanel.SetActive(false);
        darkPanel.SetActive(false);

        
        StartCoroutine(ShowTutorialWithDelay());
    }

    IEnumerator ShowTutorialWithDelay()
    {
        yield return new WaitForSeconds(startDelay);

        Time.timeScale = 0f;
        
        panelRectTransform.localScale = Vector3.zero;

     
        tutorialPanel.SetActive(true);
        darkPanel.SetActive(true);

        panelRectTransform.DOScale(Vector3.one, popDuration)
            .SetEase(openEase)
            .SetUpdate(true); 
        currentPageIndex = 0;
        UpdatePage();
    }

    public void NextPage()
    {
        if (currentPageIndex < pages.Length - 1)
        {
            currentPageIndex++;
            UpdatePage();
        }
    }

    public void PrevPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdatePage();
        }
    }


    public void ConfirmTutorial()
    {
        panelRectTransform.DOScale(Vector3.zero, popDuration)
            .SetEase(closeEase)
            .SetUpdate(true) 
            .OnComplete(() =>  
            {
               
                tutorialPanel.SetActive(false);
                darkPanel.SetActive(false);

              
                Time.timeScale = 1f;
            });
    }

    void UpdatePage()
    {
        displayText.text = pages[currentPageIndex].descriptionText;
        displayImage.sprite = pages[currentPageIndex].tutorialImage;

        prevButton.gameObject.SetActive(currentPageIndex > 0);

        if (currentPageIndex == pages.Length - 1)
        {
            nextButton.gameObject.SetActive(false);
            confirmButton.gameObject.SetActive(true);
        }
        else
        {
            nextButton.gameObject.SetActive(true);
            confirmButton.gameObject.SetActive(false);
        }
    }
}
