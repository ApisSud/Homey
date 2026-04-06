using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.UI;


public class PauseMen : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    [SerializeField] RectTransform PausepanelRect , PausebuttonRect;
    [SerializeField] float topPosY, middlePosY;
    [SerializeField] float TweenDuration;
    [SerializeField] CanvasGroup screenFaderCanvasGroup;
    public AudioClip ButtonClick;

    [SerializeField] CanvasGroup CanvasGroup;

    private bool isPaused = false;

    void Update()
    {
        if (ButtonClick != null && SoundManage.Instance != null)
        {
            SoundManage.Instance.PlaySFX(ButtonClick);
        }

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
        if (ButtonClick != null && SoundManage.Instance != null)
        {
            SoundManage.Instance.PlaySFX(ButtonClick);
        }
        await FadeOutScene();
        SceneManager.LoadScene("01Main_Menu");
        Time.timeScale = 1f;
    }

    public async void Resumemenu()
    {
        if (ButtonClick != null && SoundManage.Instance != null)
        {
            SoundManage.Instance.PlaySFX(ButtonClick);
        }
        isPaused = false;
        await pausepaneloutro();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

    }

    public async void Restart()
    {
        if (ButtonClick != null && SoundManage.Instance != null)
        {
            SoundManage.Instance.PlaySFX(ButtonClick);
        }

        MouseCOntroller.totalMouse = 0;
        Trashobject.totalTrash = 0;

        await FadeOutScene();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public async void QuitGame()
    {
        if (ButtonClick != null && SoundManage.Instance != null)
        {
            SoundManage.Instance.PlaySFX(ButtonClick);
        }
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
        PausebuttonRect.DOAnchorPosX(-70, TweenDuration).SetUpdate(true);
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
