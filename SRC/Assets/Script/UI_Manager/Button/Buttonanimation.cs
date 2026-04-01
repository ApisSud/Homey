using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Buttonanimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("Settings")]
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float clickScale = 0.9f;
    [SerializeField] private float duration = 0.2f;

    private Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(originalScale * hoverScale, duration).SetEase(Ease.OutBack);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originalScale, duration).SetEase(Ease.OutBack);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
      
        Sequence clickSequence = DOTween.Sequence();
        clickSequence.Append(transform.DOScale(originalScale * clickScale, duration * 0.5f))
                     .Append(transform.DOScale(originalScale, duration * 0.5f))
                     .SetEase(Ease.OutQuad);
    }

    void OnDisable()
    {
        transform.DOKill();
        transform.localScale = originalScale;
    }
}
