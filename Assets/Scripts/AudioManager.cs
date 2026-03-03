using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
               Debug.LogError("AudioManager instance is null");
            }
            return _instance;
        }
    }

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] _musicClip;
    [SerializeField] private AudioClip[] _voiceClip;
    [SerializeField] private AudioClip[] _sfxClip;


    [Header("Audio Sources")]
    [SerializeField] private AudioSource _sfxAudioSource;
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _voiceAudioSource;

    private void Awake()
    {
        Debug.Log($"AudioManager Awake — _instance is {(_instance == null ? "null" : "SET")}, this={GetInstanceID()}");

        if (_instance != null && _instance != this)
        {
            Debug.Log($"Duplicate detected, destroying {GetInstanceID()}");
            _instance.OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"OnSceneLoaded fired — scene={scene.name}, _instance={(_instance == null ? "null" : _instance.GetInstanceID())}");

        if (scene.name == "OfficeScene")
            PlayMusicClip(0, true);
        else if (scene.name == "GameScene")
            PlayMusicClip(1, true);
        else if (scene.name == "TitleScene" || scene.name == "SettingsScene")
        {
            if (_musicAudioSource.clip != _musicClip[2] || !_musicAudioSource.isPlaying)
                PlayMusicClip(2, true);
        }
    }
    private void OnDestroy()
    {
        Debug.Log($"OnDestroy called on {GetInstanceID()}, _instance is {(_instance == null ? "null" : _instance.GetInstanceID().ToString())}");
        if (_instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public AudioClip GetVoiceClip(int index)
    {
        if (index >= 0 && index < _voiceClip.Length)
        {
            return _voiceClip[index];
        }
        Debug.LogWarning($"Attempted to get Voice clip at invalid index: {index}. Array size: {_voiceClip.Length}");
        return null;
    }


    public void PlayMusicClip(int value, bool loop = false)
    {
        Debug.Log($"PlayMusicClip({value}) — clip={(_musicClip[value] == null ? "NULL" : _musicClip[value].name)}, source={(_musicAudioSource == null ? "NULL" : "OK")}, listenerPaused={AudioListener.pause}");

        if (value < 0 || value >= _musicClip.Length)
        {
            _musicAudioSource.Pause();
            return;
        }
        _musicAudioSource.clip = _musicClip[value];
        _musicAudioSource.loop = loop;
        _musicAudioSource.Play();
    }

    public void PlayMusicClip(AudioClip _clip, bool loop = false)
    {
        if (_clip != null)
        {
            _musicAudioSource.clip = _clip;
            _musicAudioSource.loop = loop;
            _musicAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Attempted to play a null Music clip.");
        }
    }

    public void PlayVoiceClip(int value, bool loop = false)
    {
        if (value < 0 || value >= _voiceClip.Length)
        {
            Debug.LogWarning("Invalid Voice clip index: " + value);
            return;
        }
        _voiceAudioSource.clip = _voiceClip[value];
        _voiceAudioSource.loop = loop;
        _voiceAudioSource.Play();
    }

    public void PlayVoiceClip(AudioClip _clip, bool loop = false)
    {
        if (_clip != null)
        {
            _voiceAudioSource.clip = _clip;
            _voiceAudioSource.loop = loop;
            _voiceAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Attempted to play a null Voice clip.");
        }
    }

    public void PlaySFXClip(int value, bool loop = false)
    {
        if (value < 0 || value >= _sfxClip.Length)
        {
            Debug.LogWarning("Invalid SFX clip index: " + value);
            return;
        }
        _sfxAudioSource.clip = _sfxClip[value];
        _sfxAudioSource.loop = loop;
        _sfxAudioSource.Play();
    }

    public void PlaySFXClip(AudioClip _clip, bool loop = false)
    {
        if (_clip != null)
        {
            _sfxAudioSource.clip = _clip;
            _sfxAudioSource.loop = loop;
            _sfxAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Attempted to play a null SFX clip.");
        }
    }
}