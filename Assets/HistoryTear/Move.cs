using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Move : MonoBehaviour
{
	public int Num;
	public GameObject enemy;
	public string RightKey;
	public string LeftKey;

	private bool toRight;
	private bool toLeft;
	// Use this for initialization
	void Start ()
	{
		if (Num == 1)
			GetComponentInChildren<Text> ().text += PlayerPrefs.GetString ("P1Num");
		else if (Num == 2)
			GetComponentInChildren<Text> ().text += PlayerPrefs.GetString ("P2Num");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey (RightKey)) {
			//Debug.Log ("down");
			if (transform.localPosition.x < 810.0f) {
				if (Mathf.Abs (enemy.transform.localPosition.x - transform.localPosition.x) < 1000.0f || toRight) {
					transform.localPosition = new Vector3 (transform.localPosition.x + 10.0f, transform.localPosition.y, transform.localPosition.z);
					//transform.localPosition = Vector3.Lerp (transform.localPosition, transform.localPosition + new Vector3 (100, 0), Time.deltaTime);
				} else {
					toLeft = true;
					toRight = false;
				}
			}
		}
		if (Input.GetKey (LeftKey)) {
			//Debug.Log ("down");
			if (transform.localPosition.x > -810.0f) {
				if (Mathf.Abs (enemy.transform.localPosition.x - transform.localPosition.x) < 1000.0f || toLeft) {
					transform.localPosition = new Vector3 (transform.localPosition.x - 10.0f, transform.localPosition.y, transform.localPosition.z);
					//transform.localPosition = Vector3.Lerp (transform.localPosition, transform.localPosition + new Vector3 (-100, 0), Time.deltaTime);
				} else {
					toRight = true;
					toLeft = false;
				}
			}
		}
	}
}