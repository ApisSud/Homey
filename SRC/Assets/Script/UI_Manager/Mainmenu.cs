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

    private void Start()
    {
        
        if (transitionManager == null)
        {
            transitionManager = FindObjectOfType<TransitionManager>();
        }
    }

    public void PlayGame()
    {

        if (transitionManager != null)
        {
            transitionManager.ChangeScene(game_scene);
        }
        else
        {
            Debug.LogWarning("หา TransitionManager ไม่เจอครับ! อย่าลืมสร้าง Game Object และใส่สคริปต์ TransitionManager ไว้ในซีนนะ");
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
        /*if (buttonSound != null) buttonSound.Play();*/
        Debug.Log("Exit");
        Application.Quit();
    }
}