using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    // Call this function to go to a NEW scene (e.g., Opening Settings)
    public void LoadSceneWithHistory(string sceneName)
    {
        // Remember the current scene name before we leave
        SceneHistory.PreviousScene = SceneManager.GetActiveScene().name;
        
        // Load the new scene
        SceneManager.LoadScene(sceneName);
    }

    // Call this function on your "Back" button
    public void GoBack()
    {
        if (!string.IsNullOrEmpty(SceneHistory.PreviousScene))
        {
            SceneManager.LoadScene(SceneHistory.PreviousScene);
        }
        else
        {
            // Optional: If there's no history, go to a default scene like MainMenu
            SceneManager.LoadScene("MainMenu");
        }
    }
}