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
    public Toggle fullscreenToggle;

    Resolution[] resolutions;

    void Start()
    {
        SetupResolutionDropdown();
        LoadAndApplySettings();
    }

    void SetupResolutionDropdown()
    {
        Resolution[] allResolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        
        List<Resolution> filteredResolutions = new List<Resolution>();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        
        // Define your minimum requirements here
        int minWidth = 1280;
        int minHeight = 720;

        for (int i = 0; i < allResolutions.Length; i++)
        {
            // 1. Filter out resolutions smaller than your UI can handle
            if (allResolutions[i].width < minWidth || allResolutions[i].height < minHeight)
            {
                continue;
            }

            string option = allResolutions[i].width + " x " + allResolutions[i].height;

            // 2. Check if the previous option was the same width/height to skip refresh rate duplicates
            if (options.Count > 0 && options[options.Count - 1] == option)
            {
                // We update the resolution stored to the one with the higher refresh rate
                // (Unity usually sorts resolutions by refresh rate ascending)
                filteredResolutions[filteredResolutions.Count - 1] = allResolutions[i];
                continue; 
            }

            // 3. Add valid resolution to our lists
            filteredResolutions.Add(allResolutions[i]);
            options.Add(option);

            // 4. Track current resolution index
            if (allResolutions[i].width == Screen.currentResolution.width &&
                allResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = filteredResolutions.Count - 1;
            }
        }

        // Apply the filtered list back to the class array so SetResolution(index) works correctly
        resolutions = filteredResolutions.ToArray();

        resolutionDropdown.AddOptions(options);

        // Load saved index or default to current
        int savedRes = PlayerPrefs.GetInt("ResIndex", currentResolutionIndex);
        
        // Safety check: if monitor changed, ensure saved index isn't out of bounds
        if (savedRes >= resolutions.Length) savedRes = currentResolutionIndex;

        resolutionDropdown.value = savedRes;
        resolutionDropdown.RefreshShownValue();
    }

    public void ResetToDefaults()
    {
        // 1. Define Default Values
        float defaultVol = 0.75f;
        int defaultQuality = 0; // High
        bool defaultFullscreen = true;
        
        // Pick the highest resolution as default (last in the filtered list)
        int defaultResIndex = resolutions.Length - 1;

        // 2. Apply to UI
        if (musicSlider) musicSlider.value = defaultVol;
        if (sfxSlider) sfxSlider.value = defaultVol;
        if (voiceSlider) voiceSlider.value = defaultVol;
        if (qualityDropdown) qualityDropdown.value = defaultQuality;
        if (fullscreenToggle) fullscreenToggle.isOn = defaultFullscreen;
        if (resolutionDropdown) resolutionDropdown.value = defaultResIndex;
        if (fullscreenToggle) fullscreenToggle.isOn = defaultFullscreen;

        // 3. Apply to System & Save
        SetMusicVolume(defaultVol);
        SetSFXVolume(defaultVol);
        SetVoiceVolume(defaultVol);
        SetQuality(defaultQuality);
        SetFullscreen(defaultFullscreen);
        SetResolution(defaultResIndex);
        SetFullscreen(defaultFullscreen);
        
        PlayerPrefs.Save();
        
        // Force dropdown UI to update text
        resolutionDropdown.RefreshShownValue();
        qualityDropdown.RefreshShownValue();
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

        // Load Fullscreen
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        Screen.fullScreen = isFullscreen;
        if (fullscreenToggle) fullscreenToggle.isOn = isFullscreen;
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

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }
}