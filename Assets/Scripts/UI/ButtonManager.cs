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

    public void SettingMenu()
    {
        previousSceneName = SceneManager.GetActiveScene().name;
        
        if (previousSceneName == "TitleScene")  
        {
            // From Title: Just switch scenes normally
            SceneManager.LoadScene("SettingsMenu");
        }
        else 
         {
            // If we are IN GAME:
            // Pause the game world (Stop time)
            Time.timeScale = 0f;

            // Unlock the cursor so the player can click buttons
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Load settings on top of the game
            SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Additive);
        }
    }

    public void GoBack()
    {
        // Check if there is more than one scene loaded
        // If sceneCount > 1, it means Settings is an overlay (Additive)
        if (SceneManager.sceneCount > 1)
        {
            Debug.Log("Unloading Settings overlay...");
            SceneManager.UnloadSceneAsync("SettingsMenu");

            // Unpause the game (Resume time)
            Time.timeScale = 1f;
            
            // Note: If you locked the cursor in your game, 
            // you might want to re-lock it here.
        }
        else
        {
            // If sceneCount is 1, Settings is the ONLY scene open.
            // We must LOAD the TitleScene instead of unloading.
            Debug.Log("Returning to Title Scene...");
            SceneManager.LoadScene("TitleScene");
        }
    }}
