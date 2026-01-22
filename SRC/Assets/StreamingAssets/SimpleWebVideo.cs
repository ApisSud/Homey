using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // ถ้าใช้ New Input System

public class SimpleWebVideo : MonoBehaviour
{
    [Header("Settings")]
    public VideoPlayer videoPlayer;
    public string [] videoFileName;

    [Header("Scene Transition")]
    public bool loadNextSceneOnEnd = true;
    public string nextSceneName;

    [Header("Skip Option")]
    public bool canSkip = true; // ติ๊กออกถ้าไม่อยากให้ข้าม

    void Start()
    {
        // ... (โค้ดโหลดไฟล์ส่วนเดิม) ...
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
        Debug.Log("Loading: " + videoPath);

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoPath;

        // แก้เรื่องเสียงไม่ออกในบาง Browser
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, GetComponent<AudioSource>());

        videoPlayer.Prepare();

        videoPlayer.prepareCompleted += (source) => source.Play();

        // เมื่อเล่นจบให้เปลี่ยนฉาก
        videoPlayer.loopPointReached += (source) => LoadNextLevel();
    }

    void Update()
    {
        // ถ้าอนุญาตให้ข้าม + มีการกดปุ่ม (Spacebar หรือ คลิกเมาส์ หรือปุ่มใดๆ)
        if (canSkip && (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame))
        {
            Debug.Log("Skipped Cutscene");
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        // ป้องกันการโหลดซ้อน (เช่น VDO จบพอดีกับจังหวะกดข้าม)
        if (loadNextSceneOnEnd)
        {
            loadNextSceneOnEnd = false; // ปิดสวิตช์กันโหลดซ้ำ
            SceneManager.LoadScene(nextSceneName);
        }
    }
}