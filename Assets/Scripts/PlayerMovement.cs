using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;
    private Vector2 moveInput;

    void Start()
    {
        // Grab the Rigidbody component once at the start
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // FixedUpdate is used for physics calculations
    void FixedUpdate()
    {
        // Convert moveInput into a direction relative to where the player is facing
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

        // Apply to velocity
        rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);
    }
}