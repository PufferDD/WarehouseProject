using UnityEngine;
using UnityEngine.SceneManagement; // Required for managing scenes

public class SceneMusicController : MonoBehaviour
{
    [SerializeField] private int officeMusicIndex = 0;
    [SerializeField] private int gameMusicIndex = 1;

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // This function is called when the component is disabled
    private void OnDisable()
    {
        // Unsubscribe from the event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This method is called every time a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check the name of the loaded scene
        if (scene.name == "OfficeScene")
        {
            // Tell the AudioManager to play the office music and loop it
            AudioManager.Instance.PlayMusicClip(officeMusicIndex, true);
        }
        else if (scene.name == "GameScene")
        {
            // Tell the AudioManager to play the game music and loop it
            AudioManager.Instance.PlayMusicClip(gameMusicIndex, true);
        }
    }
}