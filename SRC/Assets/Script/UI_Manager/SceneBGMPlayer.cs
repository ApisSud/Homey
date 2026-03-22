using UnityEngine;

public class SceneBGMPlayer : MonoBehaviour
{
    [Header("BGM Scene")]
    public AudioClip sceneBGM;

    private void Start()
    {
        // ทันทีที่ Scene นี้โหลดเสร็จ ให้ส่งเพลงไปบอก SoundManager
        if (SoundManage.Instance != null && sceneBGM != null)
        {
            SoundManage.Instance.PlayBGM(sceneBGM);
        }
    }
}
