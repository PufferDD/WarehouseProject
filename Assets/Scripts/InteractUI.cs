using TMPro;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    public static InteractUI I;
    [SerializeField] private TMP_Text interactText;

    private void Awake()
    {
        I = this;
        Hide();
    }

    public void Show(string msg)
    {
        if (interactText == null) return;
        interactText.text = msg;
        interactText.enabled = true;
    }

    public void Hide()
    {
        if (interactText == null) return;
        interactText.text = "";
        interactText.enabled = false;
    }
}
