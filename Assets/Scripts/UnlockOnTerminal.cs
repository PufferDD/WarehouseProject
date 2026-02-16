using UnityEngine;

public class UnlockOnTerminal : MonoBehaviour
{
    public GameObject lockedObject;   // esim oven collider/mesh
    public GameObject unlockedObject; // optional

    void Update()
    {
        if (GameManager.I == null) return;

        bool unlocked = GameManager.I.terminalUnlocked;

        if (lockedObject != null) lockedObject.SetActive(!unlocked);
        if (unlockedObject != null) unlockedObject.SetActive(unlocked);
    }
}

