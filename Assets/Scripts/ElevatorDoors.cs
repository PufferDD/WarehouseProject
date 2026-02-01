using UnityEngine;

public class ElevatorDoors : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;

    public Vector3 leftOpenOffset = new Vector3(-1f, 0, 0);
    public Vector3 rightOpenOffset = new Vector3(1f, 0, 0);

    public float openSpeed = 2f;

    Vector3 leftClosedPos;
    Vector3 rightClosedPos;
    Vector3 leftOpenPos;
    Vector3 rightOpenPos;

    bool open = false;

    void Start()
    {
        leftClosedPos = leftDoor.localPosition;
        rightClosedPos = rightDoor.localPosition;

        leftOpenPos = leftClosedPos + leftOpenOffset;
        rightOpenPos = rightClosedPos + rightOpenOffset;
    }

    void Update()
    {
        if (open)
        {
            leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, leftOpenPos, Time.deltaTime * openSpeed);
            rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, rightOpenPos, Time.deltaTime * openSpeed);
        }
        else
        {
            leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, leftClosedPos, Time.deltaTime * openSpeed);
            rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, rightClosedPos, Time.deltaTime * openSpeed);
        }
    }

    public void OpenDoors()
    {
        open = true;
    }

    public void CloseDoors()
    {
        open = false;
    }
}
