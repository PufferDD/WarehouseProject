using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public PauseMenuController pauseMenuController;

    // This static variable stays in memory even when switching scenes
    private static string previousSceneName;

    public void StartGame()
    {
        Debug.Log("Starting Game...");
        SceneManager.LoadScene("OfficeScene"); 
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
            Time.timeScale = 1f;
        }
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to Main Menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }

    // --- UPDATED SETTINGS MENU LOGIC ---
    public void SettingMenu()
    {
        Debug.Log("Opening Settings Menu...");
        
        // 1. Save the current scene name before we leave
        previousSceneName = SceneManager.GetActiveScene().name;
        
        // 2. Load the Settings Scene
        SceneManager.LoadScene("SettingsMenu");
    }

    // --- NEW GO BACK LOGIC ---
    public void GoBack()
    {
        Debug.Log("Going back...");

        if (!string.IsNullOrEmpty(previousSceneName))
        {
            // Load the scene we came from
            SceneManager.LoadScene(previousSceneName);
        }
        else
        {
            // Fallback: If for some reason history is empty, go to Main Menu
            SceneManager.LoadScene("TitleScene");
        }
    }
}