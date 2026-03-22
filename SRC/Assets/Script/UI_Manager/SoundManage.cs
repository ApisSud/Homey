using UnityEngine;
using UnityEngine.Audio;

public class SoundManage : MonoBehaviour
{
    public static SoundManage Instance;

    [Header("Audio Mixer Reference")]
    public AudioMixer audioMixer;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Furniture Sound Categories")]
    public AudioClip LightSound;  
    public AudioClip HeavySound; 
    public AudioClip GlassSound; 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadVolumeSettings();
    }


    public void PlayBGM(AudioClip bgmClip)
    {
        if (bgmClip == null)
        {
            bgmSource.Stop();
            bgmSource.clip = null;
            return;
        }

       
        if (bgmSource.clip == bgmClip)
        {
            return;
        }

      
        bgmSource.clip = bgmClip;
        bgmSource.Play();
    }

    public void PlayFurnitureSFX(FurnitureType type)
    {
        AudioClip clipToPlay = null;

        // เช็คหมวดหมู่ที่ส่งมา แล้วเลือกแผ่นเสียงให้ถูก
        switch (type)
        {
            case FurnitureType.light: 
                clipToPlay = LightSound;
                break;
            case FurnitureType.Heavy:
                clipToPlay = HeavySound;
                break;
            case FurnitureType.Glass:
                clipToPlay = GlassSound;
                break;
        }

        // เอาแผ่นเสียงที่เลือก ไปเปิดออกลำโพง
        if (clipToPlay != null)
        {
            sfxSource.PlayOneShot(clipToPlay);
        }
    }



    public void SetBGMVolume(float sliderValue)
    {
        float volume = Mathf.Max(sliderValue, 0.0001f);
        float dbValue = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("BGMVolume", dbValue);
        PlayerPrefs.SetFloat("SavedBGMVolume", sliderValue);
    }

    public void SetSFXVolume(float sliderValue)
    {
        float volume = Mathf.Max(sliderValue, 0.0001f);
        float dbValue = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("SFXVolume", dbValue);
        PlayerPrefs.SetFloat("SavedSFXVolume", sliderValue);
    }

    public void LoadVolumeSettings()
    {
        float savedBGM = PlayerPrefs.GetFloat("SavedBGMVolume", 1f);
        float savedSFX = PlayerPrefs.GetFloat("SavedSFXVolume", 1f);

        SetBGMVolume(savedBGM);
        SetSFXVolume(savedSFX);
    }
}
