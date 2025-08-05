using UnityEngine;

public class CameraView : MonoBehaviour
{
    public Transform playerBody;
    public float mouseSensitivity = 100f;
    public float clampAngle = 90f;

    private float xRotation = 0f;

    [HideInInspector]
    public float yawInput = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yawInput = mouseX; 

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -clampAngle, clampAngle);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
