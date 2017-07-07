using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class NetworkMove : NetworkBehaviour
{

	public GameObject enemy;
	public string axis = "Horizontal";
	public float speed = 40.0f;
	private bool toRight;
	private bool toLeft;
	// Use this for initialization
	void Start ()
	{
		this.transform.SetParent (GameObject.Find ("BackCanvas").transform, false);

		if (isServer) {
			GetComponentInChildren<Text> ().text += PlayerPrefs.GetString ("P1Num");
			this.transform.localPosition = new Vector3 (-400.0f, -145.0f, -1.0f);

		} else if (isClient) {
			GetComponentInChildren<Text> ().text += PlayerPrefs.GetString ("P2Num");
			this.transform.localPosition = new Vector3 (400.0f, -145.0f, -1.0f);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey ("right") && isLocalPlayer) {
			//Debug.Log ("down");
			if (transform.localPosition.x < 810.0f) {
				if (Mathf.Abs (enemy.transform.localPosition.x - transform.localPosition.x) < 1000.0f || toRight) {
					transform.localPosition += Input.GetAxis (axis) * Vector3.right * speed * Time.deltaTime;
					//transform.localPosition = Vector3.Lerp (transform.localPosition, transform.localPosition + new Vector3 (100, 0), Time.deltaTime);
				} else {
					toLeft = true;
					toRight = false;
				}
			}
		}

		if (Input.GetKey ("left") && isLocalPlayer) {
			//Debug.Log ("down");
			if (transform.localPosition.x > -810.0f) {
				if (Mathf.Abs (enemy.transform.localPosition.x - transform.localPosition.x) < 1000.0f || toLeft) {
					transform.localPosition += Input.GetAxis (axis) * Vector3.left * speed * Time.deltaTime;
					//transform.localPosition = Vector3.Lerp (transform.localPosition, transform.localPosition + new Vector3 (-100, 0), Time.deltaTime);
				} else {
					toRight = true;
					toLeft = false;
				}
			}
		}
	}
}