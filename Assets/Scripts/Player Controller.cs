using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Transform playerCamer = null; // Reference to the camera
    [SerializeField] float mouseSensitivity = 8f; // Mouse sensitivity

    [SerializeField] float walkSpeed = 5f; // Walking speed
    [SerializeField] float sprintSpeed = 10f; // Sprinting speed
    [SerializeField] [Range(0f, 0.5f)] float moveSmoothTime = 0.3f; // smooth time for movement
    [SerializeField] [Range(0f, 0.5f)] float mouseSmoothTime = 0.03f; // smooth time for mouse movement

    [SerializeField] float gravity = 13f; // Gravity value

    [SerializeField] bool lockCursor = true; // Option to lock the cursor

    private float cameraPitch = 0f; // Camera pitch (up and down rotation)
    private float velocityY = 0f; // Vertical velocity (for gravity)

    CharacterController controller = null; // Reference to the CharacterController component

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    Vector2 currentMouseDir = Vector2.zero;
    Vector2 currentMouseDirVelocity = Vector2.zero;

    // Start is called before the first frame update  
    void Start()
    {
        controller = GetComponent<CharacterController>(); // Get the CharacterController component attached to the player

        mouseSensitivity /= 100f; // Adjust mouse sensitivity for better control

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
            Cursor.visible = false; // Hide the cursor
        }
    }

    // Update is called once per frame  
    void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
    }

    void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDir = Vector2.SmoothDamp(currentMouseDir, targetMouseDelta, ref currentMouseDirVelocity, mouseSmoothTime); // Smoothly transition to the target mouse delta

        cameraPitch -= currentMouseDir.y * mouseSensitivity; // Adjust camera pitch based on mouse movement
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f); // Clamp the pitch to prevent flipping

        playerCamer.localEulerAngles = cameraPitch * Vector3.right; // Apply the pitch to the camera

        transform.Rotate(Vector3.up * currentMouseDir.x * mouseSensitivity); // Rotate the player around the Y axis
    }
    void UpdateMovement()
    {
        float speed = (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed); // Check if sprinting or walking

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); // Get input direction
        targetDir.Normalize(); // Normalize the input direction

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime); // Smoothly transition to the target direction

        if (controller.isGrounded) // Check if the player is grounded
        {
            velocityY = 0.0f; // Reset vertical velocity when grounded
        }
        velocityY -= gravity * Time.deltaTime; // Apply gravity to vertical velocity

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * speed + Vector3.up * velocityY; // Calculate velocity based on input direction and speed

        controller.Move(velocity * Time.deltaTime); // Move the player based on the calculated velocity
    }
}