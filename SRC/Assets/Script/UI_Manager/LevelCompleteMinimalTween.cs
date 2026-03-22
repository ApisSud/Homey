using UnityEngine;

public class LevelCompleteMinimalTween : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform mainPanel;      
    public RectTransform levelSuccess;  
    public RectTransform[] buttons;      

    [Header("Animation Settings")]
    public float animDuration = 0.3f;    
    public float buttonDelay = 0.1f;    

    public void ShowUI()
    {
        
        mainPanel.gameObject.SetActive(true);

       
        mainPanel.localScale = Vector3.zero;
        levelSuccess.localScale = Vector3.zero;
        foreach (var btn in buttons)
        {
            btn.localScale = Vector3.zero;
        }

       
        LeanTween.scale(mainPanel, Vector3.one, animDuration)
            .setEase(LeanTweenType.easeOutBack)
            .setIgnoreTimeScale(true);

       
        LeanTween.scale(levelSuccess, Vector3.one, animDuration)
            .setEase(LeanTweenType.easeOutBack)
            .setDelay(animDuration - 0.1f)
            .setIgnoreTimeScale(true);

      
        for (int i = 0; i < buttons.Length; i++)
        {
            LeanTween.scale(buttons[i], Vector3.one, animDuration)
                .setEase(LeanTweenType.easeOutBack)
                .setDelay(animDuration + (i * buttonDelay)) 
                .setIgnoreTimeScale(true);
        }
        Time.timeScale = 0f;
    }
}
