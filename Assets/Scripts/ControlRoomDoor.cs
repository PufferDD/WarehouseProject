using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToSceneLocked : MonoBehaviour
{
    [SerializeField] private string sceneName = "ControlRoom";
    [SerializeField] private KeyCode key = KeyCode.E;

    [Header("Lock")]
    [SerializeField] private bool requireTasks = true; // käyttää GameManager.terminalUnlocked
    [SerializeField] private string lockedMessage = "Door locked: complete 3 tasks first.";

    private bool playerInRange;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
        Debug.Log("Press E to use door");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
    }

    private void Update()
    {
        if (!playerInRange) return;
        if (!Input.GetKeyDown(key)) return;

        if (requireTasks && (GameManager.I == null || !GameManager.I.exitUnlocked))
        {
            Debug.Log(lockedMessage);
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}

