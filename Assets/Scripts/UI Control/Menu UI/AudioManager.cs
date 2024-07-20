using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] musics;
    public Sound[] sfxs;
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup sfxMixerGroup;

    void Awake()
    {
        foreach (Sound s in musics)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = musicMixerGroup;
        }

        foreach (Sound s in sfxs)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = sfxMixerGroup;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayMusic("MainTheme");
        }
        else
        {
            PlayMusic("GameplayMusic");
        }
    }

    public void PlayMusic(string name)
    {
        Sound sound = null;

        foreach (Sound s in musics)
        {
            if (s.name == name)
            {
                sound = s;
                break;
            }
        }

        if (sound == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }

        sound.source.Play();
    }

    public void PlaySFX(string name)
    {
        Sound sound = null;

        foreach (Sound s in sfxs)
        {
            if (s.name == name)
            {
                sound = s;
                break;
            }
        }

        if (sound == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }

        sound.source.Play();
    }
}
