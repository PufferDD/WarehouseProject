using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [SerializeField] private Transform teleportDestination;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            other.transform.position = teleportDestination.position;
        }
    }
}
