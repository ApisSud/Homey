using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject optionsMenuPanel;
    public RectTransform menuRect; // แนะนำให้ใช้ RectTransform สำหรับ UI แทน GameObject ตรงๆ ในการทำอนิเมชั่น

    [Header("Animation Settings")]
    public float popDuration = 0.3f;

    public AudioClip ButtonClick;
   

    private void Start()
    {
       
        optionsMenuPanel.SetActive(false);
    }

    
    public void OpenMenu()
    {
        if (ButtonClick != null && SoundManage.Instance != null)
        {
            SoundManage.Instance.PlaySFX(ButtonClick);
        }
        optionsMenuPanel.SetActive(true);

      
        // 2. เซ็ตขนาดเริ่มต้นเป็น 0 เพื่อเตรียมขยาย
        menuRect.localScale = Vector3.zero;

        // 3. สั่ง DOTween ให้ขยายไปที่ (1,1,1) แบบเด้งๆ
        // SetUpdate(true) ใช้เพื่อให้เมนูยังขยับได้แม้คุณจะ Pause เกม (Time.timeScale = 0)
        menuRect.DOScale(Vector3.one, popDuration).SetEase(Ease.OutBack).SetUpdate(true);
    }

    
    public void CloseMenu()
    {
        if (ButtonClick != null && SoundManage.Instance != null)
        {
            SoundManage.Instance.PlaySFX(ButtonClick);
        }
        menuRect.DOScale(Vector3.zero, popDuration).SetEase(Ease.InBack).SetUpdate(true).OnComplete(() =>
        {
            // 2. เมื่ออนิเมชั่นเล่นจบ ค่อยทำการปิด Panel
            optionsMenuPanel.SetActive(false);
        });
    }


   
}
