
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float gravity = -9.81f;

    public Transform playerCamera;

    private CharacterController controller;
    private float verticalVelocity;
    private float cameraRotationX;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MovePlayer();
        LookAround();

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void MovePlayer()
    {
        float x = 0f;
        float z = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) z += 1f;
            if (Keyboard.current.sKey.isPressed) z -= 1f;
            if (Keyboard.current.aKey.isPressed) x -= 1f;
            if (Keyboard.current.dKey.isPressed) x += 1f;
        }

        Vector3 movement = transform.right * x + transform.forward * z;

        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
        movement.y = verticalVelocity;

        controller.Move(movement * moveSpeed * Time.deltaTime);
    }

    void LookAround()
    {
        if (Mouse.current == null || Cursor.lockState != CursorLockMode.Locked)
        {
            return;
        }

        float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity * 0.1f;
        float mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivity * 0.1f;

        transform.Rotate(Vector3.up * mouseX);

        cameraRotationX -= mouseY;

        if (cameraRotationX > 80f) cameraRotationX = 80f;
        if (cameraRotationX < -80f) cameraRotationX = -80f;

        playerCamera.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
    }
}

