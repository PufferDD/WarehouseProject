using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody; // Drag the Player object here in Inspector

    private float xRotation = 0f;

    void Start()
    {
        // Locks the cursor to the middle of the screen and hides it
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();

        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        // Vertical look logic (clamped so you don't flip upside down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal look logic (rotates the whole player body)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}