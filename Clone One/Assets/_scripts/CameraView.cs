using UnityEngine;

public class CameraView : MonoBehaviour
{
    public Transform playerBody;
    public float mouseSensitivity = 100f;
    public float clampAngle = 90f;

    private float xRotation = 0f;

    [HideInInspector]
    public float yawInput = 0f; // <-- ADD THIS

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yawInput = mouseX; // <-- STORE THIS FOR OTHER SCRIPTS

        // Rotate the camera up/down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -clampAngle, clampAngle);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the plane body left/right
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
