using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadMusicVolume();
        }
        else
        {
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadSFXVolume();
        }
        else
        {
            SetSFXVolume();
        }
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        // Debug.Log(SFXVolume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        // Debug.Log(SFXVolume);
    }

    private void LoadMusicVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SetMusicVolume();
    }

    private void LoadSFXVolume()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        SetSFXVolume();
    }
}
