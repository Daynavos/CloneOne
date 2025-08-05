using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class EnemyFlyingFree : MonoBehaviour
{
    [Header("Flight & Targeting")]
    public Transform planetCenter;
    public Transform gunPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 40f;
    public float shootInterval = 2f;
    public float maxRadius = 100f;
    public float chaseRadius = 30f;
    public float flightSpeed = 10f;
    public float rotationSpeed = 2f;
    public float desiredDistanceFromPlayer = 10f;
    public float bufferRange = 2f;

    private Transform player;
    private Rigidbody rb;
    private Vector3 currentDestination;
    private Coroutine shootRoutine;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        PickNewWanderDestination();
        StartCoroutine(WanderRoutine());
        shootRoutine = StartCoroutine(ShootLoop());
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRadius)
        {
            Vector3 direction;

            if (distanceToPlayer > desiredDistanceFromPlayer + bufferRange)
            {
                // Too far — move toward player
                direction = (player.position - transform.position).normalized;
            }
            else if (distanceToPlayer < desiredDistanceFromPlayer - bufferRange)
            {
                // Too close — move away from player
                direction = (transform.position - player.position).normalized;
            }
            else
            {
                // In range — orbit or hold distance
                direction = Vector3.Cross(player.forward, Vector3.up).normalized; // orbit sideways
            }

            FlyInDirection(direction);
        }
        else
        {
            FlyInDirection((currentDestination - transform.position).normalized);
        }

        RotateGunToTarget();
    }

    void FlyInDirection(Vector3 direction)
    {
        rb.linearVelocity = transform.forward * flightSpeed;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));
    }

    void RotateGunToTarget()
    {
        if (gunPoint == null || player == null) return;

        Vector3 dir = player.position - gunPoint.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir.normalized);
        gunPoint.rotation = Quaternion.Slerp(gunPoint.rotation, lookRotation, Time.deltaTime * 5f);
    }

    IEnumerator WanderRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 6f));

            if (Vector3.Distance(transform.position, player.position) > chaseRadius)
                PickNewWanderDestination();
        }
    }

    void PickNewWanderDestination()
    {
        Vector3 randomDirection = Random.onUnitSphere * Random.Range(20f, maxRadius);
        currentDestination = planetCenter.position + randomDirection;
    }

    IEnumerator ShootLoop()
    {
        while (true)
        {
            // Fire continuously for 10 seconds
            float fireTime = 10f;
            float timer = 0f;

            while (timer < fireTime)
            {
                if (player != null && gunPoint != null && bulletPrefab != null)
                {
                    float distance = Vector3.Distance(transform.position, player.position);
                    if (distance <= chaseRadius)
                    {
                        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
                        Rigidbody rb = bullet.GetComponent<Rigidbody>();
                        if (rb != null)
                            rb.linearVelocity = gunPoint.forward * bulletSpeed;

                        Destroy(bullet, 5f);
                    }
                }

                yield return new WaitForSeconds(shootInterval);
                timer += shootInterval;
            }

            // Cooldown for 10 seconds
            yield return new WaitForSeconds(10f);
        }
    }

}
