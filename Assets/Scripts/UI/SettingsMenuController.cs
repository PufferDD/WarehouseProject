using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenuController : MonoBehaviour
{
    public AudioMixer audioMixer;

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
}
