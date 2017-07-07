using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LeftButton_PVP : MonoBehaviour
{

	public Scrollbar bar;
	private float maxMove;
	private float final;

	public GameObject movie1;
	public GameObject movie2;
	public GameObject movie3;

	// Use this for initialization
	void Start ()
	{
		
		maxMove = 0.005f;
	}

	public void OnClick ()
	{
		if (bar.value >= 0.49f && bar.value <= 0.51f) {
			GameObject.Find ("Arena_PVP").GetComponent<Arena_PVP> ().CmdLBClick (0f);
		} else if (bar.value >= 0.99f && bar.value <= 1.01f) {
			GameObject.Find ("Arena_PVP").GetComponent<Arena_PVP> ().CmdLBClick (0.5f);
		}

		//if (GetComponentInParent<Canvas_PVP> ().isLocalPlayer) IsClick();

	}

	public void IsClick (float V)
	{
			final = V;
			InvokeRepeating ("smoothMove", 0.0f, 0.01f);
	}

	private void smoothMove ()
	{
		if (bar.value > final) {
			/*Debug.Log (bar.value);
			Debug.Log (final);*/
			bar.value -= maxMove;
		} else {
			
			Debug.Log (final);
			bar.value = final;

			if (bar.value == 0f) {
				movie2.GetComponent<movie_PVP> ().movieSource.Stop ();
				movie2.GetComponent<movie_PVP> ().GetComponent<AudioSource> ().Stop ();

				movie1.GetComponent<movie_PVP> ().movieSource.Play ();
				movie1.GetComponent<movie_PVP> ().GetComponent<AudioSource> ().Play ();
			} else if (bar.value == 0.5f) {
				movie3.GetComponent<movie_PVP> ().movieSource.Stop ();
				movie3.GetComponent<movie_PVP> ().GetComponent<AudioSource> ().Stop ();

				movie2.GetComponent<movie_PVP> ().movieSource.Play ();
				movie2.GetComponent<movie_PVP> ().GetComponent<AudioSource> ().Play ();
			} 

			CancelInvoke ();
		}
	}
}