using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float speed = 20f;
	private Vector3 targetDirection;

	public void SetTarget(Vector3 target)
	{
		targetDirection = (target - transform.position).normalized;
	}

	void Update()
	{
		transform.position += targetDirection * speed * Time.deltaTime;
	}
}
