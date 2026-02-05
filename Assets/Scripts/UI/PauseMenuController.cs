using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign your pause menu panel here in the Inspector
    public KeyCode pauseKey = KeyCode.Escape;

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false); // Ensure pause menu is hidden at start
        }
        Time.timeScale = 1f; // Ensure time is flowing at the start of the game scene

        // Added code for cursor visibility and lock state in Start
        Cursor.visible = true; // Make cursor visible
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            Debug.Log("Pause key pressed.");
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Stop time
        isPaused = true;
        AudioListener.pause = true; // Pause all audio
        Cursor.visible = true; // Make cursor visible
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume time
        isPaused = false;
        AudioListener.pause = false; // Resume all audio
        Cursor.visible = false; // Hide cursor when resuming game
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center
    }
}