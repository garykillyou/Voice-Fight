using UnityEngine;
using System.Collections;

public class OptionCanvas2_PVP : MonoBehaviour
{

	private bool pauseEnabled;
	private bool move;
	// Use this for initialization
	void Start ()
	{
		GetComponent<Canvas> ().enabled = false;
		pauseEnabled = false;
		move = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//check if pause button (escape key) is pressed
		if (Input.GetKeyDown ("escape")) {

			//check if game is already paused
			if (pauseEnabled == true) {
				//unpause the game
				GetComponent<Canvas> ().enabled = false;
				pauseEnabled = false;
				Time.timeScale = 1;

			}

			//else if game isn't paused, then pause it
			else if (pauseEnabled == false) {
				if (!move) {
					GetComponent<Canvas> ().enabled = true;
					transform.localScale = new Vector3 (1, 1, 1);
					transform.localPosition = new Vector3 (0, 0);
				}
				pauseEnabled = true;
				Time.timeScale = 0;
			}
		}
	}
}
