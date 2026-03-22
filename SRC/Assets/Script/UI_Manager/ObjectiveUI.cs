using DG.Tweening;
using UnityEngine;

public class ObjectiveUI : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform objectivePanel;

    [Header("Animation Settings")]
    public float hidePositionX = -1500f;
    public float moveDuration = 0.5f;

   
    private bool isUIOpen = true;

   
    public void ToggleObjectiveUI()
    {
        if (isUIOpen) 
        {
            
            objectivePanel.DOAnchorPosX(hidePositionX, moveDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() => {
                });

            isUIOpen = false; 
        }
        else 
        {
         

           
            objectivePanel.DOAnchorPosX(26f, moveDuration).SetEase(Ease.OutBack);

            isUIOpen = true; 
        }
    }
}
