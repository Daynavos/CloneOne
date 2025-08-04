using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
	public float gravity = -9.81f;

	public void Attract(Rigidbody body)
	{
		Vector3 direction = (transform.position - body.position).normalized;
		Vector3 gravityForce = direction * gravity;

		body.AddForce(gravityForce, ForceMode.Acceleration);
	}
}
