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
            // Check for current resolution
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        // Load index or default to current
        int savedRes = PlayerPrefs.GetInt("ResIndex", currentResolutionIndex);
        resolutionDropdown.value = savedRes;
        resolutionDropdown.RefreshShownValue();
    }

    void LoadAndApplySettings()
    {
        // Load Volume (Default to 0.75f linear which is 75% slider)
        float music = PlayerPrefs.GetFloat("MusicVol", 0.75f);
        float sfx = PlayerPrefs.GetFloat("SFXVol", 0.75f);
        float voice = PlayerPrefs.GetFloat("VoiceVol", 0.75f);

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

    // This converts the 0-1 slider value to a -80 to 0 dB value logarithmicly
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVol", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVol", volume);
    }

    public void SetVoiceVolume(float volume)
    {
        audioMixer.SetFloat("voiceVolume", Mathf.Log10(volume) * 20);
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