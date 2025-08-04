using UnityEngine;
using UnityEngine.InputSystem;

public class FlightController : MonoBehaviour
{
    public GameObject ship;
    public GameObject planet;
    public float rotationSpeed = 20f;
    public float turnSpeed = 90f;
    public float zoomSpeed = 10f;
    public float minScale = 0.1f;
    public float maxScale = 30f;

    private InputSystem_Actions controls;
    private Vector2 moveInput;
    private float zoomInput;
    
    public Camera cam;
    private Vector3 clickHitPoint;
    private Quaternion targetRotation;
    private bool isRotating = false;
    public Color targetColor = Color.red;

    void Awake()
    {
        controls = new InputSystem_Actions();

        controls.Flying.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Flying.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Flying.Zoom.performed += ctx => zoomInput = ctx.ReadValue<float>();
        controls.Flying.Zoom.canceled += ctx => zoomInput = 0;
    }

    void OnEnable() => controls.Flying.Enable();
    void OnDisable() => controls.Flying.Disable();

    void Update()
    {
        // Turn ship (A/D)
        ship.transform.Rotate(Vector3.forward.normalized * -1 *moveInput.x * turnSpeed * Time.deltaTime, Space.Self);

        // Move forward/backward (W/S) → rotate planet
        planet.transform.Rotate(ship.transform.right.normalized, -moveInput.y * rotationSpeed * Time.deltaTime, Space.World);


        // Zoom (scroll)
        if (zoomInput != 0)
        {
            Vector3 newScale = planet.transform.localScale + Vector3.one * zoomInput * zoomSpeed * Time.deltaTime;
            newScale = Vector3.Max(Vector3.one * minScale, Vector3.Min(Vector3.one * maxScale, newScale));
            planet.transform.localScale = newScale;
        }
        
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == planet)
                {
                    RotatePlanetToClick(hit);
                }
            }
        }
        
        if (isRotating)
        {
            planet.transform.rotation = Quaternion.RotateTowards(
                planet.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            if (Quaternion.Angle(planet.transform.rotation, targetRotation) < 0.1f)
            {
                planet.transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }

    void RotatePlanetToClick(RaycastHit hit)
    {
        Vector3 hitPoint = hit.point;
        Vector3 planetCenter = planet.transform.position;

        Vector3 fromDirection = (hitPoint - planetCenter).normalized;
        Vector3 toDirection = (ship.transform.position - planetCenter).normalized;

        Quaternion rotationDelta = Quaternion.FromToRotation(fromDirection, toDirection);

        targetRotation = rotationDelta * planet.transform.rotation;
        isRotating = true;
    }


}
