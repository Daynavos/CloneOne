using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PushingPlanetScript : MonoBehaviour
{
    [Header("Gravity Settings")]
    public float gravityRadius = 50f; 
    public float gravityStrength = -9.81f;

    void Start()
    {
        SphereCollider col = GetComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = gravityRadius;
    }

    void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            Vector3 directionToCenter = (transform.position - rb.position).normalized;
            float distance = Vector3.Distance(transform.position, rb.position);

            if (distance <= gravityRadius)
            {
                rb.AddForce(directionToCenter * gravityStrength, ForceMode.Acceleration);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.darkRed;
        Gizmos.DrawWireSphere(transform.position, gravityRadius);

        Vector3 direction = Vector3.right;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + direction * gravityRadius);
        Gizmos.DrawLine(transform.position, transform.position - direction * gravityRadius);
    }
}
