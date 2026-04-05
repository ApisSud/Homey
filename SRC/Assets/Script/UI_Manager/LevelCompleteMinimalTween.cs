using UnityEngine;

public class LevelCompleteMinimalTween : MonoBehaviour
{
    [Header("UI Elements")]
    public CanvasGroup darkOverlay;
    public RectTransform mainPanel;      
    public RectTransform levelSuccess;
    public RectTransform extraImage;
    public RectTransform[] buttons;      

    [Header("Animation Settings")]
    public float animDuration = 0.3f;    
    public float buttonDelay = 0.1f;
    public float successTargetScale = 1.2f;

    [Header("Dark Overlay Settings")]
    [Range(0f, 1f)]
    public float maxDarkAlpha = 0.7f;

    public void ShowUI()
    {
        
        mainPanel.gameObject.SetActive(true);

        if (darkOverlay != null)
        {
            darkOverlay.gameObject.SetActive(true);
            darkOverlay.alpha = 0f; // เริ่มต้นที่โปร่งใส 100%

            // ค่อยๆ เฟดความทึบขึ้นมาจนถึงค่า maxDarkAlpha
            LeanTween.alphaCanvas(darkOverlay, maxDarkAlpha, animDuration)
                .setIgnoreTimeScale(true);
        }

        mainPanel.localScale = Vector3.zero;
        levelSuccess.localScale = Vector3.zero;

        if (extraImage != null)
        {
            extraImage.localScale = Vector3.zero;
        }

        foreach (var btn in buttons)
        {
            btn.localScale = Vector3.zero;
        }

       
        LeanTween.scale(mainPanel, Vector3.one, animDuration)
            .setEase(LeanTweenType.easeOutBack)
            .setIgnoreTimeScale(true);


        LeanTween.scale(levelSuccess, Vector3.one * successTargetScale, animDuration)
              .setEase(LeanTweenType.easeOutBack)
            .setDelay(animDuration - 0.1f)
            .setIgnoreTimeScale(true);

        if (extraImage != null)
        {
            LeanTween.scale(extraImage, Vector3.one, animDuration)
                .setEase(LeanTweenType.easeOutBack)
                .setDelay(animDuration)
                .setIgnoreTimeScale(true);
        }

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
