using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;



public class OpeningCutscene : MonoBehaviour
{
    public GameObject[] tutorialPages;
    public float fadeDuration = 0.8f;
    public float startDelay = 1.5f;

    private int currentPageIndex = 0;

    [Header("UI References")]
    public GameObject nextButton; 
    public GameObject skipButton;
    public string game_scene = "SampleScene";
    public CanvasGroup sceneTransitionOverlay;

    void Start()
    {
       
        foreach (GameObject page in tutorialPages)
        {
            if (page.TryGetComponent<CanvasGroup>(out CanvasGroup cg))
            {
                cg.alpha = 0f;
                page.SetActive(false);
            }
        }

        
        StartCoroutine(StartWithDelay());
    }

    IEnumerator StartWithDelay()
    {
       
        yield return new WaitForSeconds(startDelay);

        
        ShowPage(currentPageIndex);
    }

    private void ShowPage(int index)
    {
        if (index >= 0 && index < tutorialPages.Length)
        {
            if (currentPageIndex != index && currentPageIndex < tutorialPages.Length)
            {
                tutorialPages[currentPageIndex].SetActive(false);
            }

            GameObject newPage = tutorialPages[index];
            newPage.SetActive(true);

            if (newPage.TryGetComponent<CanvasGroup>(out CanvasGroup cg))
            {
                cg.alpha = 0f;
               
                cg.DOFade(1f, fadeDuration).SetEase(Ease.OutQuad);
            }

            currentPageIndex = index;
        }
    }

    public void GoToNextPage()
    {
        GameObject currentPage = tutorialPages[currentPageIndex];
        if (currentPage.TryGetComponent<CanvasGroup>(out CanvasGroup cg))
        {
            cg.DOFade(0f, fadeDuration)
              .SetEase(Ease.InQuad)
              .OnComplete(() =>
              {
                  currentPage.SetActive(false);
                  if (currentPageIndex < tutorialPages.Length - 1)
                  {
                      ShowPage(currentPageIndex + 1);
                  }
                  else
                  {
                      EndTutorial();
                  }
              });
        }
    }

    public void EndTutorial()
    {

        HideButtons();

        if (sceneTransitionOverlay != null)
        {
            sceneTransitionOverlay.blocksRaycasts = true;
           
            sceneTransitionOverlay.DOFade(1f, fadeDuration)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    SceneManager.LoadScene(game_scene);
                });
        }
    }

    public void Skip()
    {
        HideButtons();
        SceneManager.LoadScene(game_scene);
    }

    private void HideButtons()
    {
        if (nextButton != null) nextButton.SetActive(false);
        if (skipButton != null) skipButton.SetActive(false);
    }
}
