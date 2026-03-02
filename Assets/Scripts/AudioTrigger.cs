using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Required for Coroutines
using System.Collections.Generic;

public class AudioTrigger : MonoBehaviour
{
    [System.Serializable]
    public struct SubtitlePart
    {
        [TextArea(3, 5)] public string text;
        public float duration; // How long this part stays on screen
    }

    [Header("Audio Clip Indices (Reference AudioManager)")]
    [SerializeField] private int _officeVoiceClipIndex = -1;
    [SerializeField] private int _gameVoiceClipIndex = -1;

    [Header("Voicelines (Split into parts)")]
    [SerializeField] private List<SubtitlePart> _officeVoicelines;
    [SerializeField] private List<SubtitlePart> _gameVoicelines;

    [Header("Settings")]
    [SerializeField] private float _speakDistance = 5f;

    private Transform _playerTransform;
    private bool _hasSpoken = false;

    private void Start()
    {
        TryFindPlayer(); // ÄLÄ disabletä scriptiä jos player ei löydy heti
    }

    private void Update()
    {
        if (_hasSpoken) return;

        // Player voi spawnata vasta hetken päästä
        if (_playerTransform == null)
        {
            TryFindPlayer();
            return;
        }

        if (Vector3.Distance(transform.position, _playerTransform.position) <= _speakDistance)
        {
            Speak();
            _hasSpoken = true;
        }
    }

    private void TryFindPlayer()
    {
        var playerGO = GameObject.FindWithTag("Player");
        if (playerGO != null)
        {
            _playerTransform = playerGO.transform;
        }
        // EI elseä, EI disabled
    }

    private void Speak()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager instance is not available. Cannot play audio.");
            return;
        }

        string currentSceneName = SceneManager.GetActiveScene().name;
        int clipIndexToPlay;
        List<SubtitlePart> linesToDisplay;

        if (currentSceneName == "OfficeScene")
        {
            clipIndexToPlay = _officeVoiceClipIndex;
            linesToDisplay = _officeVoicelines;
        }
        else if (currentSceneName == "GameScene")
        {
            clipIndexToPlay = _gameVoiceClipIndex;
            linesToDisplay = _gameVoicelines;
        }
        else
        {
            Debug.LogWarning("AudioTrigger is in an unrecognized scene: " + currentSceneName);
            return;
        }

        // Play the audio
        AudioManager.Instance.PlayVoiceClip(clipIndexToPlay);

        // Start showing the split subtitles
        if (SubtitleManager.Instance != null && linesToDisplay != null && linesToDisplay.Count > 0)
        {
            StartCoroutine(ShowSubtitlesSequentially(linesToDisplay));
        }
    }

    private IEnumerator ShowSubtitlesSequentially(List<SubtitlePart> parts)
    {
        foreach (var part in parts)
        {
            // Show the current part
            SubtitleManager.Instance.ShowSubtitle(part.text, part.duration);
            
            // Wait for the duration of this part before showing the next one
            yield return new WaitForSeconds(part.duration);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _speakDistance);
    }
}