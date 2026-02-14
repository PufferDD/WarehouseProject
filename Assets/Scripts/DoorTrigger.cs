using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Transform door;
    public float openDistance = 2f;
    public float speed = 3f;

    Vector3 closedPos;
    Vector3 openPos;
    bool isOpen;

    void Start()
    {
        closedPos = door.localPosition;
        openPos = closedPos + Vector3.up * openDistance; 
        // ↑ Vaihda Vector3.up -> Vector3.right jos haluat sivulle
    }

    void Update()
    {
        Vector3 target = isOpen ? openPos : closedPos;
        door.localPosition = Vector3.Lerp(door.localPosition, target, Time.deltaTime * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isOpen = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isOpen = false;
    }
}


