using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {

	public float paddleSpeed = 0.5f;

	private Vector3 playerPos = new Vector3 (0, 0.5f, 0);

	// Update is called once per frame
	void Update () {
		float xPos = transform.position.x + (Input.GetAxis ("Horizontal") * paddleSpeed);
		playerPos = new Vector3 (Mathf.Clamp(xPos, -7.5f, 7.5f), 0.5f, 0f);
		transform.position = playerPos;	
	}
}