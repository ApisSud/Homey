using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("AudioBehaviour sources")]
    [SerializeField] AudioSource musicSorce;
    [SerializeField] AudioSource SFXSorce;


    [Header("Music")]
    public AudioClip Intro;
    public AudioClip BackgroundMusic1;
    public AudioClip BackgroundMusic2;
    public AudioClip BackgroundMusic3;
    public AudioClip BadEnd;

    [Header("Cutscene Sound")]
    public AudioClip Logo;
    public AudioClip BeforeTheStories;
    public AudioClip Failcutscene;
    public AudioClip PassButFailcutscene;
    public AudioClip Passcutscene;
    public AudioClip Secretcutscene;
    public AudioClip Endcreditscene;

    [Header("SFX")]
    public AudioClip ButtonPressed;
    public AudioClip MiniSuccess;
    public AudioClip Success;
    public AudioClip SuperSuccess;
    public AudioClip Failed;
    public AudioClip MomWalking;
    public AudioClip MomBox;
    public AudioClip MomWindow;
    public AudioClip Snore;
    public AudioClip Switch;
    public AudioClip Treadmill;
    public AudioClip Hulahoop;
    public AudioClip Dumbbell;
    public AudioClip Sing;



    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        // เริ่มฟัง Event "เมื่อโหลดฉากเสร็จ"
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // เลิกฟัง Event (สำคัญมาก กัน Error ตอนปิดเกม)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ฟังก์ชันนี้จะทำงาน "อัตโนมัติ" ทุกครั้งที่เปลี่ยน Scene
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name; // ได้ชื่อ Scene ปัจจุบันมา
        Debug.Log("Loaded Scene: " + sceneName);

        switch (sceneName)
        {
            case "SplashArt":       // ตัวอย่าง 1
                ChangeBGM(Logo);
                break;

            case "Mainmenu":        // ตัวอย่าง 2
                ChangeBGM(Intro);
                break;

            case "intro":  // ตัวอย่าง 3
                ChangeBGM(BeforeTheStories);
                break;

            case "Tutorial": // ตัวอย่าง 4
                ChangeBGM(Intro);
                break;

            case "Endiing_GetCatch": // ตัวอย่าง 5
                ChangeBGM(Failcutscene);
                break;

            case "Ending_NotEnoughtStrong":    // ตัวอย่าง 6
                ChangeBGM(PassButFailcutscene);
                break;

            case "Ending_Secret":  
                ChangeBGM(Secretcutscene);
                break;

            case "Ending_Win":   
                ChangeBGM(Passcutscene);
                break;

            case "Endcredit":   
                ChangeBGM(Endcreditscene);
                break;

            default:
                break;
        }
    }
    public void playSFX(AudioClip clip)
    { 
        SFXSorce.PlayOneShot(clip);
    }

    public void ChangeBGM(AudioClip newMusic)
    {
        if (musicSorce.clip == newMusic) return;

        musicSorce.Stop();          
        musicSorce.clip = newMusic; 
        musicSorce.Play();         
    }
    
}
