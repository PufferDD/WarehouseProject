using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // Make sure to assign this in the Inspector in your GameScene!
    public PauseMenuController pauseMenuController;

    public void StartGame()
    {
        Debug.Log("Starting Game...");
        SceneManager.LoadScene("OfficeScene"); //remember to add officescene to the scenelist
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ResumeGame()
    {
        Debug.Log("Resuming Game... (from ButtonManager)");
        if (pauseMenuController != null)
        {
            pauseMenuController.ResumeGame();
        }
        else
        {
            Debug.LogError("Error happened.");
            Time.timeScale = 1f; // Just in case, try to resume time
        }
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to Main Menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }
}