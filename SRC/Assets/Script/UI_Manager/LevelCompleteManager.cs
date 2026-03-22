using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelCompleteManager : MonoBehaviour
{
    [Header("UI Animation Reference")]
   
    public LevelCompleteMinimalTween uiAnimator;

    [Header("Scene Navigation Settings")]
    [Tooltip("พิมพ์ชื่อ Scene ของหน้าหลัก (เช่น MainMenu)")]
    public string homeSceneName = "MainMenu";

    [Tooltip("พิมพ์ชื่อ Scene ของด่านถัดไป (เช่น Level_02)")]
    public string nextLevelName;

    public void ShowLevelCompletePanel()
    {
        if (uiAnimator != null)
        {
            uiAnimator.ShowUI();
        }
    }

    public void ReplayLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoHome()
    {
        Time.timeScale = 1f;
       
        SceneManager.LoadScene(homeSceneName);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        
        SceneManager.LoadScene(nextLevelName);
    }
}
