using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioTrigger : MonoBehaviour
{
    [Header("Audio Clip Indices (Reference AudioManager)")]
    [SerializeField] private int _officeVoiceClipIndex = -1;
    [SerializeField] private int _gameVoiceClipIndex = -1;

    [Header("Voicelines (Text to display for subtitles)")]
    [SerializeField, TextArea(3, 10)]
    private string _officeVoicelineText =
        "Hello there, new hire. Welcome to the main office. " +
        "Please proceed to your assigned workstation and begin your orientation.";

    [SerializeField, TextArea(3, 10)]
    private string _gameVoicelineText =
        "Welcome Employee number 32040. " +
        "Your task for today is: Warehouse duties. " +
        "Please sort warehouse number 240 into an optimal condition. " +
        "Have a great work day and remember to stay positive.";

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
        string textToDisplay;

        if (currentSceneName == "OfficeScene")
        {
            clipIndexToPlay = _officeVoiceClipIndex;
            textToDisplay = _officeVoicelineText;
        }
        else if (currentSceneName == "GameScene")
        {
            clipIndexToPlay = _gameVoiceClipIndex;
            textToDisplay = _gameVoicelineText;
        }
        else
        {
            Debug.LogWarning("AudioTrigger is in an unrecognized scene: " + currentSceneName);
            return;
        }

        // Hae clip (voi olla null jos index väärin)
        AudioClip actualClip = AudioManager.Instance.GetVoiceClip(clipIndexToPlay);

        if (actualClip != null)
        {
            AudioManager.Instance.PlayVoiceClip(clipIndexToPlay);

            if (SubtitleManager.Instance != null)
                SubtitleManager.Instance.ShowSubtitle(textToDisplay, actualClip.length);
        }
        else
        {
            Debug.LogWarning($"No valid Voice Clip found for index {clipIndexToPlay} in {currentSceneName}. Showing text anyway.");
            if (SubtitleManager.Instance != null)
                SubtitleManager.Instance.ShowSubtitle(textToDisplay, 5f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _speakDistance);
    }
}
