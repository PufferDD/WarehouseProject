using UnityEngine;

public class RespawnOnGameOver : MonoBehaviour
{
    public Transform respawnPoint;

    void Update()
    {
        if (GameManager.I == null) return;
        if (!GameManager.I.gameOver) return;

        var player = GameObject.FindWithTag("Player");

        if (player != null && respawnPoint != null)
        {
            player.transform.position = respawnPoint.position;
        }

        GameManager.I.gameOver = false;
    }
}
