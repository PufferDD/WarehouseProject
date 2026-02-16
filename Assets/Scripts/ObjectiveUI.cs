using UnityEngine;
using TMPro;

public class ObjectiveUI : MonoBehaviour
{
    public TMP_Text text;

    void Start()
    {
        if (text == null) text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (GameManager.I == null) return;
        text.text = GameManager.I.currentObjective;
    }
}

