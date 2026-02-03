using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SettingsMenuController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Slider musicSlider, sfxSlider, voiceSlider;

    Resolution[] resolutions;

    void Start()
    {
        SetupResolutionDropdown();
        LoadAndApplySettings();
    }

    void SetupResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("ResIndex", currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();
    }

    void LoadAndApplySettings()
    {
        // Load Volume (Default to 0.75f if not found)
        float music = PlayerPrefs.GetFloat("MusicVol", 0);
        float sfx = PlayerPrefs.GetFloat("SFXVol", 0);
        float voice = PlayerPrefs.GetFloat("VoiceVol", 0);

        // Update UI Elements
        if(musicSlider) musicSlider.value = music;
        if(sfxSlider) sfxSlider.value = sfx;
        if(voiceSlider) voiceSlider.value = voice;

        // Apply to Mixer
        SetMusicVolume(music);
        SetSFXVolume(sfx);
        SetVoiceVolume(voice);

        // Load Quality
        int quality = PlayerPrefs.GetInt("QualityIndex", 2);
        qualityDropdown.value = quality;
        SetQuality(quality);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
        PlayerPrefs.SetFloat("MusicVol", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume);
        PlayerPrefs.SetFloat("SFXVol", volume);
    }

    public void SetVoiceVolume(float volume)
    {
        audioMixer.SetFloat("voiceVolume", volume);
        PlayerPrefs.SetFloat("VoiceVol", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityIndex", qualityIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResIndex", resolutionIndex);
    }
}