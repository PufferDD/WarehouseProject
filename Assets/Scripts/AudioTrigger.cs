using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioTrigger : MonoBehaviour
{
    [Header("Audio Clip Indices (Reference AudioManager)")]
    [SerializeField] private int _officeVoiceClipIndex = -1; // Assign index for Office voice clip in AudioManager
    [SerializeField] private int _gameVoiceClipIndex = -1;   // Assign index for Game voice clip in AudioManager

    [Header("Voicelines (Text to display for subtitles)")]
    [SerializeField, TextArea(3, 10)] 
    private string _officeVoicelineText = "Hello there, new hire. Welcome to the main office. "
                                        + "Please proceed to your assigned workstation and begin your orientation.";

    [SerializeField, TextArea(3, 10)] 
    private string _gameVoicelineText = "Welcome Employee number 32040. "
                                      + "Your task for today is: Warehouse duties. "
                                      + "Please sort warehouse number 240 into an optimal condition. "
                                      + "Have a great work day and remember to stay positive.";

    [Header("Settings")]
    [SerializeField] private float _speakDistance = 5f;

    private Transform _playerTransform;
    private bool _hasSpoken = false;

    private void Start()
    {
        // Find the player GameObject. You might have a more robust way to do this.
        GameObject playerGO = GameObject.FindWithTag("Player"); 
        if (playerGO != null)
        {
            _playerTransform = playerGO.transform;
        }
        else
        {
            Debug.LogError("Player GameObject not found! Make sure your player has the 'Player' tag.");
            enabled = false; // Disable this script if no player is found
        }
    }

    private void Update()
    {
        if (_playerTransform == null || _hasSpoken) return;

        // Check the distance between the robot and the player
        if (Vector3.Distance(transform.position, _playerTransform.position) <= _speakDistance)
        {
            Speak();
            _hasSpoken = true; // Ensure it only speaks once
        }
    }

    private void Speak()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager instance is not available. Cannot play audio.");
            return;
        }

        string currentSceneName = SceneManager.GetActiveScene().name;
        int clipIndexToPlay = -1;
        string textToDisplay = "";
        AudioClip actualClip = null; // To hold the retrieved clip

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
            Debug.LogWarning("RobotSpeaker is in an unrecognized scene: " + currentSceneName);
            return; // Don't try to play or show subtitles if scene is unrecognized
        }

        // Attempt to get the actual clip using the new public method
        actualClip = AudioManager.Instance.GetVoiceClip(clipIndexToPlay);

        // Play the audio clip using the AudioManager if a valid index was provided and clip exists
        if (actualClip != null) // Check if the clip was successfully retrieved
        {
            AudioManager.Instance.PlayVoiceClip(clipIndexToPlay); // Play using the index
            
            // Show subtitles using the SubtitleManager
            if (SubtitleManager.Instance != null)
            {
                SubtitleManager.Instance.ShowSubtitle(textToDisplay, actualClip.length);
            }
            Debug.Log($"Robot speaking in {currentSceneName}: {textToDisplay}");
        }
        else
        {
            Debug.LogWarning($"No valid Voice Clip found for index {clipIndexToPlay} in {currentSceneName}. Still showing text: {textToDisplay}");
            // Even if no audio clip, show the text for a default duration if subtitle manager exists
            if (SubtitleManager.Instance != null)
            {
                // If no clip, maybe show for a fixed duration like 5 seconds, or based on text length
                SubtitleManager.Instance.ShowSubtitle(textToDisplay, 5.0f); 
            }
        }
    }

    // Optional: Draw a sphere in the editor to visualize the speak distance
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _speakDistance);
    }
}