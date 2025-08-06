using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float damage = 10f;
    public Vector3 shooterPosition;

    void OnCollisionEnter(Collision other)
    {
        IDamageable target = other.gameObject.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage); // Adjusted to match the method signature
        }

        Destroy(gameObject);
    }
}
