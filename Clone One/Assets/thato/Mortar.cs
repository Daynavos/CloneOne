using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Mortar : MonoBehaviour
{
    [Header("References")]
    public Transform turretTop;              // The entire turret that rotates (cylinder)
    public Transform firePoint;              // Where the projectile spawns
    public Transform rotationPivot;          // The pivot point at the base (assign in inspector)

    public GameObject projectilePrefab;

    [Header("Targeting")]
    public float rotationSpeed = 3f;

    [Header("Firing Settings")]
    public float bulletSpeed = 50f;
    public int shotsPerBurst = 10;
    public float shotInterval = 0.3f;
    public float cooldownDuration = 10f;

    private Transform player;
    private bool isCoolingDown = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        StartCoroutine(FireCycle());
    }

    void Update()
    {
        RotateTurretTowardsPlayer();
    }

    void RotateTurretTowardsPlayer()
    {
        if (player == null || turretTop == null || rotationPivot == null) return;

        Vector3 pivotPos = rotationPivot.position;
        Vector3 direction = (player.position - pivotPos).normalized;

        if (direction.sqrMagnitude < 0.001f) return;

        // Calculate target rotation so the turret forward faces the player
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly interpolate rotation from current to target
        Quaternion newRotation = Quaternion.Slerp(turretTop.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // Calculate offset from pivot to turretTop position BEFORE rotation
        Vector3 pivotToTurret = turretTop.position - pivotPos;

        // Rotate that offset by the change in rotation
        Quaternion rotationChange = newRotation * Quaternion.Inverse(turretTop.rotation);
        Vector3 rotatedOffset = rotationChange * pivotToTurret;

        // Set the new position so base stays at pivot
        turretTop.position = pivotPos + rotatedOffset;

        // Apply the new rotation
        turretTop.rotation = newRotation;
    }


    IEnumerator FireCycle()
    {
        while (true)
        {
            if (!isCoolingDown)
            {
                for (int i = 0; i < shotsPerBurst; i++)
                {
                    Fire();
                    yield return new WaitForSeconds(shotInterval);
                }

                isCoolingDown = true;
                yield return new WaitForSeconds(cooldownDuration);
                isCoolingDown = false;
            }

            yield return null;
        }
    }

    void Fire()
    {
        if (firePoint == null || projectilePrefab == null || player == null) return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 direction = (player.position - firePoint.position).normalized;
            rb.linearVelocity = direction * bulletSpeed;
        }

        Destroy(projectile, 5f);
    }
}
