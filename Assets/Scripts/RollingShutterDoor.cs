using System.Collections;
using UnityEngine;

public class RollingShutterDoor : MonoBehaviour
{
    [Header("Door (the moving shutter)")]
    [SerializeField] private Transform door;

    [Header("Positions (LOCAL Y)")]
    [SerializeField] private float closedY = 2.142461f;
    [SerializeField] private float openY = 4.642461f;

    [SerializeField] private float moveSpeed = 2f;

    [Header("Environment Visibility")]
    [SerializeField] private GameObject[] indoorObjects;   // seinät jne
    [SerializeField] private GameObject[] outdoorObjects;  // pihan jutut

    private Coroutine _moveRoutine;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        MoveDoor(true);
        SetIndoors(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        MoveDoor(false);
        SetIndoors(false);
    }

    private void MoveDoor(bool open)
    {
        if (door == null) return;

        if (_moveRoutine != null)
            StopCoroutine(_moveRoutine);

        _moveRoutine = StartCoroutine(MoveTo(open ? openY : closedY));
    }

    private IEnumerator MoveTo(float targetLocalY)
    {
        Vector3 target = new Vector3(
            door.localPosition.x,
            targetLocalY,
            door.localPosition.z
        );

        while ((door.localPosition - target).sqrMagnitude > 0.001f)
        {
            door.localPosition = Vector3.MoveTowards(
                door.localPosition,
                target,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        door.localPosition = target;
    }

    private void SetIndoors(bool indoors)
    {
        foreach (var obj in indoorObjects)
            if (obj) obj.SetActive(indoors);

        foreach (var obj in outdoorObjects)
            if (obj) obj.SetActive(!indoors);
    }
}


