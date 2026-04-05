using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public string game_scene = "intro";

    [Header("References")]
   
    [SerializeField] private TransitionManager transitionManager;

    public AudioClip ButtonClick;
    private void Start()
    {
        
        if (transitionManager == null)
        {
            transitionManager = FindObjectOfType<TransitionManager>();
        }
    }

    public void PlayGame()
    {
        if (ButtonClick != null && SoundManage.Instance != null)
        {
            SoundManage.Instance.PlaySFX(ButtonClick);
        }

        if (transitionManager != null)
        {
            transitionManager.ChangeScene(game_scene);
        }
      
    }

    public void SelectLevel(string scenename)
    {

        if (transitionManager != null)
        {
            transitionManager.ChangeScene(scenename);
        }
    }

   
    public void QuitGame()
    {
        if (ButtonClick != null && SoundManage.Instance != null)
        {
            SoundManage.Instance.PlaySFX(ButtonClick);
        }
        /*if (buttonSound != null) buttonSound.Play();*/
        Debug.Log("Exit");
        Application.Quit();
    }
}