using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;


public class PauseMen : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    [SerializeField] RectTransform PausepanelRect , PausebuttonRect;
    [SerializeField] float topPosY, middlePosY;
    [SerializeField] float TweenDuration;
    [SerializeField] CanvasGroup screenFaderCanvasGroup;

    [SerializeField] CanvasGroup CanvasGroup;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resumemenu();
            }
            else
            {
                Pausemenu();  
            }
        }
    }
    public void Pausemenu()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        pausepanelIntro();
    }

    public async void Home()
    {
        await FadeOutScene();
        SceneManager.LoadScene("01Main_Menu");
        Time.timeScale = 1f;
    }

    public async void Resumemenu()
    {
        isPaused = false;
        await pausepaneloutro();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

    }

    public async void Restart()
    {
        await FadeOutScene();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public async void QuitGame()
    {
        await FadeOutScene(); 
        Debug.Log("Quit Game Executed!"); 
        Application.Quit(); 
    }

    void pausepanelIntro()
    {
        CanvasGroup.DOFade(1 , TweenDuration).SetUpdate(true);
        PausepanelRect.DOAnchorPosY(middlePosY, TweenDuration).SetUpdate(true);
        PausebuttonRect.DOAnchorPosX(200 , TweenDuration).SetUpdate(true);
    }
    async Task pausepaneloutro()
    {
        CanvasGroup.DOFade(0, TweenDuration).SetUpdate(true);
        await PausepanelRect.DOAnchorPosY(topPosY, TweenDuration).SetUpdate(true).AsyncWaitForCompletion();
        PausebuttonRect.DOAnchorPosX(-120, TweenDuration).SetUpdate(true);
    }

    private async Task FadeOutScene()
    {
     
        if (screenFaderCanvasGroup == null)
        {
            Debug.LogError("Screen Fader Canvas Group is not assigned!");
            return;
        }

        screenFaderCanvasGroup.alpha = 0f;
        screenFaderCanvasGroup.gameObject.SetActive(true); 


        await screenFaderCanvasGroup.DOFade(2f, TweenDuration)
            .SetUpdate(true) 
            .AsyncWaitForCompletion();
    }
}
