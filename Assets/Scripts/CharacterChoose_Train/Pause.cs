using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

	private bool pauseEnabled = false;
	public GUIStyle style1;
	public GUIStyle style2;

	void  Start ()
	{
		pauseEnabled = false;
		Time.timeScale = 1;
		AudioListener.volume = 1;
	}

	void  Update ()
	{
 
		//check if pause button (escape key) is pressed
		if (Input.GetKeyDown ("escape")) {
 
			//check if game is already paused
			if (pauseEnabled == true) {
				//unpause the game
				pauseEnabled = false;
				Time.timeScale = 1;
				AudioListener.volume = 1;
				GameObject.Find ("Cover").GetComponent < RawImage > ().enabled = false;
			}
 
            //else if game isn't paused, then pause it
            else if (pauseEnabled == false) {
				pauseEnabled = true;
				AudioListener.volume = 0;
				Time.timeScale = 0;
				GameObject.Find ("Cover").GetComponent < RawImage > ().enabled = true;
			}
		}
	}


 
	void  OnGUI ()
	{
 
		if (pauseEnabled == true) {

			//Make a background box
			GUI.Box (new Rect (Screen.width / 2 - 125, Screen.height / 2 - 100, 250, 100), "Pause Menu", style1);
 
			//Make Main Menu button
			if (GUI.Button (new Rect (Screen.width / 2 - 125, Screen.height / 2, 250, 100), "Main Menu", style2)) {
				SceneManager.LoadScene (0);
			}
 
			//Make quit game button
			if (GUI.Button (new Rect (Screen.width / 2 - 125, Screen.height / 2 + 100, 250, 100), "Quit Game", style2)) {
				Application.Quit ();
			}
		}
	}
}