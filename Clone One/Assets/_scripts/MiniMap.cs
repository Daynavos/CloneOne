using UnityEngine;

public class MiniMap : MonoBehaviour
{
	// For the mini map to follow the player and maintain a certain height

	public Transform player;
	public Vector3 offset = new Vector3(0, 50, 0);

	void LateUpdate()
	{
		Vector3 newPos = player.position + offset;
		newPos.y = offset.y;
		transform.position = newPos;
	}
}
