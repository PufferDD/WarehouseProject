using UnityEngine;
using TMPro; // Make sure this is included for TextMeshProUGUI
using System.Collections; // For Coroutines

public class SubtitleManager : MonoBehaviour
{
    private static SubtitleManager _instance;
    public static SubtitleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("SubtitleManager instance is null. Make sure it's present in your scene.");
            }
            return _instance;
        }
    }

    [Header("UI Elements")]
    [SerializeField] private GameObject _subtitleCanvasGameObject;
    [SerializeField] private TextMeshProUGUI _subtitleText;

    private Coroutine _hideSubtitleCoroutine;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject); // Keep it across scenes if you want global subtitles
    }

    void Start()
    {
        // Ensure canvas and text are initially hidden/empty
        if (_subtitleCanvasGameObject != null)
        {
            _subtitleCanvasGameObject.SetActive(false);
        }
        if (_subtitleText != null)
        {
            _subtitleText.text = "";
        }
    }

    /// <summary>
    /// Displays a subtitle for a specified duration.
    /// </summary>
    /// <param name="text">The text to display.</param>
    /// <param name="duration">How long the subtitle should be visible.</param>
    public void ShowSubtitle(string text, float duration)
    {
        if (_subtitleCanvasGameObject == null || _subtitleText == null)
        {
            Debug.LogError("SubtitleManager: Subtitle Canvas or Text is not assigned in the Inspector!");
            return;
        }

        // Stop any currently running hide coroutine to prevent premature hiding
        if (_hideSubtitleCoroutine != null)
        {
            StopCoroutine(_hideSubtitleCoroutine);
        }

        _subtitleText.text = text;
        _subtitleCanvasGameObject.SetActive(true);

        // Start a new coroutine to hide the subtitle after the duration
        _hideSubtitleCoroutine = StartCoroutine(HideSubtitleAfterDelay(duration));
    }

    /// <summary>
    /// Immediately hides the subtitle.
    /// </summary>
    public void HideSubtitle()
    {
        if (_subtitleText != null)
        {
            _subtitleText.text = "";
        }
        if (_subtitleCanvasGameObject != null)
        {
            _subtitleCanvasGameObject.SetActive(false);
        }
        if (_hideSubtitleCoroutine != null)
        {
            StopCoroutine(_hideSubtitleCoroutine);
            _hideSubtitleCoroutine = null;
        }
    }

    private IEnumerator HideSubtitleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideSubtitle();
    }
}