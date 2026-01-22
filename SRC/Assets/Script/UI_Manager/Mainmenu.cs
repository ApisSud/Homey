using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string game_scene = "intro";
    //public string option_scene = "none";
   
    public void PlayGame()
    {
        
        SceneManager.LoadScene(game_scene);
    }

    public void QuitGame()
    {
       
        Debug.Log("ออกจากเกมแล้ว");
        Application.Quit();
    }
}