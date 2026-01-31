using UnityEngine;
using TMPro;

public class AudioTriggerScript : MonoBehaviour
{
    private AudioSource audioSource;
    private bool hasPlayed = false;

    public GameObject subtitleCanvasGameObject;
    public TextMeshProUGUI subtitleText;

    public string voiceline = "Welcome Employee number 32040. " 
    + "Your task for today is: Warehouse duties. " 
    + "Please sort warehouse number 240 into an optimal condition. " 
    + "Have a great work day and remember to stay positive.";

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (subtitleCanvasGameObject != null)
        {
            subtitleCanvasGameObject.SetActive(false);
        }
        if (subtitleText != null)
        {
            subtitleText.text = "";
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            audioSource.Play();

            if (subtitleCanvasGameObject != null)
            {
                subtitleCanvasGameObject.SetActive(true);
            }
            if (subtitleText != null)
            {
                subtitleText.text = voiceline;
            }

            Invoke("HideCanvasAndClearText", audioSource.clip.length);

            hasPlayed = true;
        }
    }

    void HideCanvasAndClearText()
    {
        if (subtitleText != null)
        {
            subtitleText.text = "";
        }
        if (subtitleCanvasGameObject != null)
        {
            subtitleCanvasGameObject.SetActive(false);
        }
    }
}