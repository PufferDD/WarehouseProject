using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public static SettingsManager instance;

    void Awake()
    {
        // Singleton pattern: This object will persist through all scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        ApplyAllSettings();
    }

    public void ApplyAllSettings()
    {
        // Apply Audio
        ApplyVolume("musicVolume", "MusicVol");
        ApplyVolume("sfxVolume", "SFXVol");
        ApplyVolume("voiceVolume", "VoiceVol");

        // Apply Quality
        int quality = PlayerPrefs.GetInt("QualityIndex", QualitySettings.GetQualityLevel());
        QualitySettings.SetQualityLevel(quality);
    }

    void ApplyVolume(string mixerParam, string prefKey)
    {
        float vol = PlayerPrefs.GetFloat(prefKey, 0.75f);
        audioMixer.SetFloat(mixerParam, Mathf.Log10(vol) * 20);
    }
}