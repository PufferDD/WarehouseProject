using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorLoadScene : MonoBehaviour
{
    public string sceneName = "GameScene";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}

