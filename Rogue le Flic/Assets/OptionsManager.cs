using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class OptionsManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public TextMeshProUGUI textMaster;
    public TextMeshProUGUI textMusic;
    public TextMeshProUGUI textSFX;

    public UnityEngine.UI.Slider sliderMaster;
    public UnityEngine.UI.Slider sliderMusic;
    public UnityEngine.UI.Slider sliderSFX;


    private void Start()
    {
        textMaster.text = Mathf.RoundToInt(sliderMaster.value + 80).ToString();
        textMusic.text = Mathf.RoundToInt(sliderMusic.value + 80).ToString();
        textSFX.text = Mathf.RoundToInt(sliderSFX.value + 80).ToString();
    }


    public void SetMasterVolume()
    {
        audioMixer.SetFloat("MasterVolume", sliderMaster.value);
        textMaster.text = Mathf.RoundToInt(sliderMaster.value + 80).ToString();
    }

    public void SetMusicVolume()
    {
        audioMixer.SetFloat("MusicVolume", sliderMusic.value);
        textMusic.text = Mathf.RoundToInt(sliderMusic.value + 80).ToString();
    }

    public void SetSFXVolume()
    {
        audioMixer.SetFloat("SFXVolume", sliderSFX.value);
        textSFX.text = Mathf.RoundToInt(sliderSFX.value + 80).ToString();
    }
    
    public void SetFullScreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }
}
