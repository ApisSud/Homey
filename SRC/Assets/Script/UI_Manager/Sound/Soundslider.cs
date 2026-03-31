using UnityEngine;
using UnityEngine.UI;

public class Soundslider : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {

       
        float savedBGM = PlayerPrefs.GetFloat("SavedBGMVolume", 1f);
        float savedSFX = PlayerPrefs.GetFloat("SavedSFXVolume", 1f);

        bgmSlider.value = savedBGM;
        sfxSlider.value = savedSFX;


        bgmSlider.onValueChanged.AddListener(UpdateBGM);
        sfxSlider.onValueChanged.AddListener(UpdateSFX);
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
