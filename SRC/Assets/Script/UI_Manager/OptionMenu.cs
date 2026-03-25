using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject optionsMenuPanel; 
    public Slider bgmSlider;           
    public Slider sfxSlider;
    public AudioClip ButtonClick;


    private void Start()
    {
       
        optionsMenuPanel.SetActive(false);

       
        float savedBGM = PlayerPrefs.GetFloat("SavedBGMVolume", 1f);
        float savedSFX = PlayerPrefs.GetFloat("SavedSFXVolume", 1f);

        bgmSlider.value = savedBGM;
        sfxSlider.value = savedSFX;

        
        bgmSlider.onValueChanged.AddListener(UpdateBGM);
        sfxSlider.onValueChanged.AddListener(UpdateSFX);
    }

    
    public void OpenMenu()
    {
        if (ButtonClick != null && SoundManage.Instance != null)
        {
            SoundManage.Instance.PlaySFX(ButtonClick);
        }
        optionsMenuPanel.SetActive(true);
    }

    
    public void CloseMenu()
    {
        if (ButtonClick != null && SoundManage.Instance != null)
        {
            SoundManage.Instance.PlaySFX(ButtonClick);
        }
        optionsMenuPanel.SetActive(false);
    }


    private void UpdateBGM(float value)
    {
        
        if (SoundManage.Instance != null)
        {
            SoundManage.Instance.SetBGMVolume(value);
        }
    }

    private void UpdateSFX(float value)
    {
        if (SoundManage.Instance != null)
        {
            SoundManage.Instance.SetSFXVolume(value);
        }
    }
}
