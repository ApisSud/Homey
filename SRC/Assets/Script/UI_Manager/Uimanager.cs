using UnityEngine;
using DG.Tweening;

public class Uimanager : MonoBehaviour
{
    public float fadetime = 1f;
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;

    public void Panelfadein()
    {
        canvasGroup.alpha = 0f;
        rectTransform.transform.localPosition = new Vector3(0f , -1000f , 0f);
        rectTransform.DOAnchorPos(new Vector2(0f, 0f), fadetime, false).SetEase(Ease.OutElastic);
        canvasGroup.DOFade(1, fadetime);
    }
}
