using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlaneSimulator : MonoBehaviour
{
    public Camera planeCamera;

    [Header("Flight Settings")]
    public float maxSpeed = 100f;
    public float acceleration = 30f;
    public float pitchSpeed = 50f;
    public float rollSpeed = 50f;
    public float yawSpeed = 30f;
    public float drag = 0.98f;

    [Header("Damage System")]
    public float maxDamage = 100f;
    public float currentDamage = 0f;
    public float damageMultiplier = 0.1f;

    private Rigidbody rb;
    private float currentSpeed = 0f;
    private bool isDestroyed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = 0f;
        rb.angularDamping = 0.5f;
    }

    void Update()
    {
        currentDamage = Mathf.Clamp(currentDamage, 0f, maxDamage);

        if (currentDamage >= maxDamage && !isDestroyed)
        {
            CrashPlane();
        }
    }

    void FixedUpdate()
    {
        if (isDestroyed) return;

        HandleThrust();
        AlignWithCamera();
        ApplyRollInput();
    }

    void HandleThrust()
    {
        float throttleInput = Input.GetAxis("Vertical"); // W = 1, S = -1
        currentSpeed += throttleInput * acceleration * Time.fixedDeltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);

        // Move forward in camera direction (not flattened)
        Vector3 forward = planeCamera.transform.forward;
        rb.linearVelocity = forward * currentSpeed;

        currentSpeed *= drag; // simulate basic drag
    }

    void AlignWithCamera()
    {
        if (planeCamera == null) return;

        // Align rotation smoothly with camera's full rotation (pitch, yaw, roll)
        Quaternion targetRotation = Quaternion.LookRotation(planeCamera.transform.forward, planeCamera.transform.up);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 2f));
    }

    void ApplyRollInput()
    {
        float roll = 0f;
        if (Input.GetKey(KeyCode.Q)) roll = rollSpeed * Time.fixedDeltaTime;
        if (Input.GetKey(KeyCode.E)) roll = -rollSpeed * Time.fixedDeltaTime;

        if (roll != 0f)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, 0f, roll));
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDestroyed) return;

        if (collision.rigidbody != null)
        {
            float otherMass = collision.rigidbody.mass;
            float relativeVelocity = collision.relativeVelocity.magnitude;
            float impactForce = otherMass * relativeVelocity;

            float damageTaken = impactForce * damageMultiplier;
            currentDamage += damageTaken;
            Debug.Log("Damage: " + damageTaken + " | Total: " + currentDamage);
        }
    }

    void CrashPlane()
    {
        isDestroyed = true;
        Debug.Log("Plane is destroyed! Falling from the sky.");

        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        currentSpeed = 0f;
    }
}
