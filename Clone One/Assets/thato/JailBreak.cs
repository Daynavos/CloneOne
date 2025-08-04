using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;

public class JailBreak : MonoBehaviour, IDamageable
{
    public float minFollowDistance = 5f;
    public float maxFollowDistance = 25f;
    public float shootInterval = 2f;
    public float bulletSpeed = 50f;

    [Header("Gun Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    private GameObject player;
    private NavMeshAgent agent;
    private Coroutine followRoutine;
    private Coroutine shootRoutine;
    private int currentHits = 0;
    public int maxHits = 3;
    public GameObject[] heartIcons;

    private readonly string[] targetTags = { "Player" };

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        followRoutine = StartCoroutine(FollowPlayer());
        shootRoutine = StartCoroutine(ShootTargets());
    }

    IEnumerator FollowPlayer()
    {
        while (player != null)
        {
            float randomDistance = Random.Range(minFollowDistance, maxFollowDistance);

            Vector3 directionToPlayer = (transform.position - player.transform.position).normalized;
            Vector3 targetPosition = player.transform.position + directionToPlayer * randomDistance;
            agent.SetDestination(targetPosition);

            yield return new WaitForSeconds(1.5f);
        }
    }

    IEnumerator ShootTargets()
    {
        while (true)
        {
            GameObject target = FindClosestValidTarget();

            if (target != null)
            {
                Vector3 targetPosition = target.transform.position;
                Vector3 bodyPosition = transform.position;
                Vector3 lookDirection = (targetPosition - bodyPosition).normalized;

                Vector3 flatLookDir = new Vector3(lookDirection.x, 0f, lookDirection.z);
                if (flatLookDir.sqrMagnitude > 0.01f)
                {
                    Quaternion npcYawRotation = Quaternion.LookRotation(flatLookDir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, npcYawRotation, Time.deltaTime * 5f);
                }

                if (firePoint != null)
                {
                    firePoint.LookAt(targetPosition);
                }


                if (firePoint != null && bulletPrefab != null)
                {
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                    Rigidbody rb = bullet.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.linearVelocity = firePoint.forward * bulletSpeed;
                    }
                    Destroy(bullet, 5f);
                }
            }

            yield return new WaitForSeconds(shootInterval);
        }
    }

    GameObject FindClosestValidTarget()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in allObjects)
        {
            if (obj == null || obj == gameObject) continue;

            foreach (string tag in targetTags)
            {
                if (obj.CompareTag(tag))
                {
                    float distance = Vector3.Distance(transform.position, obj.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closest = obj;
                    }
                }
            }
        }

        return closest;
    }

    public void TakeDamage(float amount)
    {
        currentHits++;
        Debug.Log($"{gameObject.name} was hit ({currentHits}/{maxHits})");

        if (currentHits <= heartIcons.Length)
        {
            heartIcons[currentHits - 1].SetActive(false);
        }

        if (currentHits >= maxHits)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        if (followRoutine != null) StopCoroutine(followRoutine);
        if (shootRoutine != null) StopCoroutine(shootRoutine);
        Destroy(gameObject);
    }
}

