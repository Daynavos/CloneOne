using UnityEngine;

public class Shooter : MonoBehaviour
{
	public GameObject bulletPrefab;
	public Transform firePoint;
	public Transform target;
	
	void Shoot()
	{
		GameObject proj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
		Bullet projectileScript = proj.GetComponent < Bullet>();
		if (projectileScript != null)
		{
			//projectileScript.SetTarget(target.position);
		}
	}
}
