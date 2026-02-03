using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;

public class SettingsMenuController : MonoBehaviour
{
    public AudioMixer audioMixer;

    public TMPro.TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetMusicVolume(float volume)
    {
        Debug.Log("Volume set to: " + volume);
        audioMixer.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        Debug.Log("Volume set to: " + volume);
        audioMixer.SetFloat("sfxVolume", volume);
    }

    public void SetVoiceVolume(float volume)
    {
        Debug.Log("Volume set to: " + volume);
        audioMixer.SetFloat("voiceVolume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
    // Check how many levels Unity actually sees
    int actualLevelCount = QualitySettings.names.Length;

    if (qualityIndex < actualLevelCount)
        {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log("Quality set to: " + QualitySettings.names[qualityIndex]);
        }
    else
        {
        Debug.LogError($"Dropdown tried to set Quality Index {qualityIndex}, but you only have {actualLevelCount} levels in Project Settings!");
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }   
}
