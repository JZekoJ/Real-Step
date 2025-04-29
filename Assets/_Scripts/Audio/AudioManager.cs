using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public static float SlideMusicValue;
    public static float SlideVFXValue;

    [Header("Mixer Audio")]
    public AudioMixer audioMixer;

    [Header("Volume Keys")]
    private const string MASTER_KEY = "MasterVolume";
    private const string MUSIC_KEY = "MusicVolume";
    private const string SFX_KEY = "SFXVolume";

    private void Awake()
    {
        //SlideMusicValue = PlayerPrefs.GetFloat("Music");
        //SlideVFXValue = PlayerPrefs.GetFloat("SFXVolume");
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadVolumes();
    }

    public void SetMasterVolume(float value)
    {
        if (value == 0)
        {
            audioMixer.SetFloat("MasterVolume", -80f);
            PlayerPrefs.SetFloat(MASTER_KEY, value);
            
        }
        else
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
            PlayerPrefs.SetFloat(MASTER_KEY, value);
        }
    }

    public void SetMusicVolume(float value)
    {
        SlideMusicValue = value;
        Debug.Log("la musique dans la peau");
        if (SlideMusicValue == 0)
        {
            audioMixer.SetFloat("MusicVolume", -80f);
            PlayerPrefs.SetFloat(MASTER_KEY, SlideMusicValue);
        }
        else
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(SlideMusicValue) * 20);
            PlayerPrefs.SetFloat(MASTER_KEY, SlideMusicValue);
        }
    }

    public void SetSFXVolume(float value)
    {
        SlideVFXValue = value;
        if (SlideVFXValue == 0)
        {
            audioMixer.SetFloat("SFXVolume", -80f);
            PlayerPrefs.SetFloat(MASTER_KEY, SlideVFXValue);
        }
        else
        {
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(SlideVFXValue) * 20);
            PlayerPrefs.SetFloat(MASTER_KEY, SlideVFXValue);
        }
    }

    public void LoadVolumes()
    {
        float master = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float music = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfx = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        SetMasterVolume(master);
        SetMusicVolume(music);
        SetSFXVolume(sfx);
    }
}
