using UnityEngine;

public class Bullet : MonoBehaviour
{

	public float lifeTime = 2f;
	private float timer = 0f;

	void Update()
	{
		timer += Time.deltaTime;
		if (timer >= lifeTime)
		{
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		Destroy(gameObject);
	}
}
