using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlaneScript: MonoBehaviour
{
    public CameraView cameraView;
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
    private bool isDestroyed = false;

    [Header("Post-Crash Behavior")]
    public GameObject activateAfterCrash;
    public GameObject activateAfterCrashii;
    public GameObject deactivateAfterCrash; 
    public GameObject deactivateAfterCrashii;

    private Rigidbody rb;
    private float currentSpeed = 0f;

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

    void FixedUpdate()
    {
        if (isDestroyed) return;

        HandleThrust();
        HandleRotation();
    }

    void HandleThrust()
    {
        float throttleInput = Input.GetAxis("Vertical"); 
        currentSpeed += throttleInput * acceleration * Time.fixedDeltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);

        rb.linearVelocity = transform.forward * currentSpeed;
        currentSpeed *= drag;
    }

    void HandleRotation()
    {
        float pitch = -Input.GetAxis("Mouse Y") * pitchSpeed * Time.fixedDeltaTime;
        float roll = -Input.GetAxis("Horizontal") * rollSpeed * Time.fixedDeltaTime;
        float yaw = cameraView.yawInput * yawSpeed * Time.fixedDeltaTime;

        Vector3 rotation = new Vector3(pitch, yaw, roll);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
    }

    void CrashPlane()
    {
        isDestroyed = true;

        Debug.Log("Plane is destroyed! Falling from the sky.");

        rb.useGravity = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        currentSpeed = 0f;


        StartCoroutine(PostCrashSequence());
    }

    IEnumerator PostCrashSequence()
    {
        yield return new WaitForSeconds(7f);

        if (activateAfterCrash != null)
            activateAfterCrash.SetActive(true);

        if (activateAfterCrashii != null)
            activateAfterCrashii.SetActive(true);

        if (deactivateAfterCrash != null)
            deactivateAfterCrash.SetActive(false);

        if (deactivateAfterCrashii != null)
            deactivateAfterCrashii.SetActive(false);
    }
}
