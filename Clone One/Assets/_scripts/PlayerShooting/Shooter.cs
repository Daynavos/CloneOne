using UnityEngine;

public class Shooter : MonoBehaviour
{
	public GameObject bulletPrefab;
	public Transform firePoint;
	public Transform target;

	public float moveSpeed = 5f;
	public float gravity = -9.81f;
	public float jumpHeight = 2f;

	private CharacterController controller;
	private Vector3 velocity;
	private bool isGrounded;

	public Transform groundCheck;
	public float groundDistance = 0.4f;
	public LayerMask groundMask;
	

	void Start()
	{
		controller = GetComponent<CharacterController>();
	}

	void Update()
	{

		if (Input.GetKeyDown(KeyCode.Space)) // shoot when pressing space
		{
			Shoot();
		}

		// Check if player is grounded
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

		if (isGrounded && velocity.y < 0)
		{
			velocity.y = -2f;
		}

		// Movement input
		float x = Input.GetAxis("Horizontal"); // A/D
		float z = Input.GetAxis("Vertical");   // W/S

		Vector3 move = transform.right * x + transform.forward * z;
		controller.Move(move * moveSpeed * Time.deltaTime);

		// Jump
		//if (Input.GetButtonDown("Jump") && isGrounded)
		//{
		//	velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
		//}

		// Gravity
		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}
	
	void Shoot()
	{
		GameObject proj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
		Bullet projectileScript = proj.GetComponent < Bullet>();
		if (projectileScript != null)
		{
			projectileScript.SetTarget(target.position);
		}
	}
}
