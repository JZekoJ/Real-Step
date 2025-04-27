using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    //public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("ICI : " + AudioManager.Instance);
        foreach (Slider slide in Resources.FindObjectsOfTypeAll(typeof(Slider)) as Slider[])
        {
            Debug.Log(slide.gameObject);
            if (slide.gameObject.name == "Son")
            {
                sfxSlider = slide;
            }
            if (slide.gameObject.name == "Musique")
            {
                musicSlider = slide;
            }
        }
        float master = PlayerPrefs.GetFloat("MasterVolume",1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

        //masterSlider.value = master;
        musicSlider.value = music;
        sfxSlider.value = sfx;

        //masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
    }

    private void Start()
    {
        
        
        
    }

    private void OnDestroy()
    {
        if (AudioManager.Instance != null)
        {
            //masterSlider.onValueChanged.RemoveListener(AudioManager.Instance.SetMasterVolume);
            musicSlider.onValueChanged.RemoveListener(AudioManager.Instance.SetMusicVolume);
            sfxSlider.onValueChanged.RemoveListener(AudioManager.Instance.SetSFXVolume);
        }
    }
}
