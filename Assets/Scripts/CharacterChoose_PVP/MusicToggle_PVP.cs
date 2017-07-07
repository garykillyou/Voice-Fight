using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MusicToggle_PVP : MonoBehaviour {

	private AudioSource music;
	// Use this for initialization
	void Start () {
		music = GameObject.FindGameObjectWithTag ("BackgroundSong").GetComponent<AudioSource>();
		GetComponent<Toggle> ().onValueChanged.AddListener( musicChanged ) ;
	}
	
	// Update is called once per frame
	void Update () {

	}
		
	public void musicChanged( bool change) {
		music.enabled = change;
	}
}
