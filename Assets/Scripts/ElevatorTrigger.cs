using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    public ElevatorDoors doors;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doors.OpenDoors();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doors.CloseDoors();
        }
    }
}

