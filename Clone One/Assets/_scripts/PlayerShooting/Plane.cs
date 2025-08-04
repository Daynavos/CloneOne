using UnityEngine;
using UnityEngine.Internal;

public class Plane : MonoBehaviour
{
	public PlanetGravity planet;

	private Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.useGravity = false; // Disable default gravity
		rb.constraints = RigidbodyConstraints.FreezeRotation; // Optional: prevent spinning
	}
	
	public float moveSpeed = 5f;

	void Update()
	{
		float horizontal = Input.GetAxisRaw("Horizontal"); // Left/Right arrows or A/D
		float vertical = Input.GetAxisRaw("Vertical");     // Up/Down arrows or W/S	

		Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

		transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
	}
	void FixedUpdate()
	{
		if (planet != null)
		{
			planet.Attract(rb);
		}
	}
}
