using UnityEngine;

public class WorkTask : MonoBehaviour
{
    public KeyCode key = KeyCode.E;
    public int alertIncreasePercent = 25;
    public string taskName = "Work Task";

    bool inRange;
    bool done;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        inRange = true;

        if (!done && InteractUI.I != null)
            InteractUI.I.Show($"Press {key} to do: {taskName}");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        inRange = false;

        if (InteractUI.I != null)
            InteractUI.I.Hide();
    }

    private void Update()
    {
        if (!inRange || done) return;

        if (Input.GetKeyDown(key))
        {
            done = true;

            if (InteractUI.I != null)
                InteractUI.I.Hide();

            if (GameManager.I != null)
                GameManager.I.CompleteTask(alertIncreasePercent);

            // Destroy(gameObject); // jos haluat että katoaa
        }
    }
}



