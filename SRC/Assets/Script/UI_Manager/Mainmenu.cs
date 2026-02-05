using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{

    public string game_scene = "intro";

    public Image fadePanel; 
    [SerializeField] public float fadeDuration = 1.0f;
    public AudioSource buttonSound;

    public void PlayGame()
    {
        if (buttonSound != null)
        {
            buttonSound.Play();
        }
        StartCoroutine(FadeAndLoadScene());
    }

    IEnumerator FadeAndLoadScene()
    {
   
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);
            Color panelColor = fadePanel.color;
            panelColor.a = 0f; 
            fadePanel.color = panelColor;

            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

                panelColor.a = alpha;
                fadePanel.color = panelColor;

                yield return null; 
            }
        }

     
        SceneManager.LoadScene(game_scene);
    }
    public void QuitGame()
    {
        if (buttonSound != null) buttonSound.Play();
        Debug.Log("Exit");
        Application.Quit();
    }

 
}