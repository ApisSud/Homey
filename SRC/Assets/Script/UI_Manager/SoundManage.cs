using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

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
    public AudioClip LightSound2;
  /*  public AudioClip LightSound3;*/

    public AudioClip HeavySound; 
    public AudioClip HeavySound2;
   /* public AudioClip HeavySound3;*/

    public AudioClip GlassSound;
    public AudioClip GlassSound2;
   /* public AudioClip GlassSound3;*/

    public AudioClip ClothSound;
    public AudioClip ClothSound2;
   /* public AudioClip ClothSound3;*/

    public AudioClip WoodSound;
    public AudioClip WoodSound2;
   /* public AudioClip WoodSound3;*/

    public AudioClip SackSound;
    public AudioClip SackSound2;
   /* public AudioClip SackSound3;*/

    [Header("Fade Settings")]
    public float fadeDuration = 1.5f;

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
        if (bgmSource.clip == bgmClip) return;

       
        bgmSource.DOKill();

       
        if (bgmClip == null)
        {
            bgmSource.DOFade(0f, fadeDuration).OnComplete(() =>
            {
                bgmSource.Stop();
                bgmSource.clip = null;
                bgmSource.volume = 1f; 
            });
            return;
        }

       
        if (bgmSource.isPlaying)
        {
            bgmSource.DOFade(0f, fadeDuration).OnComplete(() =>
            {
                bgmSource.clip = bgmClip;
                bgmSource.Play();
                bgmSource.DOFade(1f, fadeDuration); 
            });
        }
     
        else
        {
            bgmSource.volume = 0f; 
            bgmSource.clip = bgmClip;
            bgmSource.Play();
            bgmSource.DOFade(1f, fadeDuration); 
        }
    }

    public void PlayFurnitureSFX(FurnitureType type)
    {
        AudioClip clipToPlay = null;

       
        switch (type)
        {
            case FurnitureType.light: 
                clipToPlay = LightSound;
                break;

            case FurnitureType.light2:
                clipToPlay = LightSound2;
                break;

              

            case FurnitureType.Heavy:
                clipToPlay = HeavySound;
                break;

            case FurnitureType.Heavy2:
                clipToPlay = HeavySound2;
                break;

            


            case FurnitureType.Glass:
                clipToPlay = GlassSound;
                break;

                case FurnitureType.Glass2:
                    clipToPlay = GlassSound2;
                break;

                

            case FurnitureType.cloth:
                clipToPlay = ClothSound;
                break;

             case FurnitureType.cloth2:
                    clipToPlay = ClothSound2;
                break;

               

            case FurnitureType.sack:
                clipToPlay = SackSound;
                break;

                case FurnitureType.sack2:
                    clipToPlay = SackSound2;
                break;
               
            case FurnitureType.wood:
                clipToPlay = WoodSound;
                break;
                case FurnitureType.wood2:
                clipToPlay = WoodSound2;
                break;
              
              
        }

        
        if (clipToPlay != null)
        {
            sfxSource.PlayOneShot(clipToPlay);
        }
    }


    public void PlaySFX(AudioClip sfxClip)
    {
      
        if (sfxClip != null)
        {
         
            sfxSource.PlayOneShot(sfxClip);
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
