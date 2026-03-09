using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;


public class OpeningCutscene : MonoBehaviour
{
    public GameObject[] tutorialPages;

   
    public float fadeDuration = 0.8f;


    private int currentPageIndex = 0;

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
            else
            {
                Debug.LogError("Tutorial Page missing CanvasGroup: " + page.name);
            }
        }


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

                cg.DOFade(2f, fadeDuration)
                  .SetEase(Ease.OutQuad);
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
        if (sceneTransitionOverlay != null)
        {
            // 2. ทำให้ FadePanel บังการกดปุ่มอื่น ๆ
            sceneTransitionOverlay.blocksRaycasts = true;

            // 3. สั่ง Fade เป็นสีดำ (Alpha 0 -> 1)
            sceneTransitionOverlay.DOFade(2f, fadeDuration)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                  
                    SceneManager.LoadScene(game_scene);
                });
        }

    }
    public void Skip()
    {
        SceneManager.LoadScene(game_scene);
    }

}
