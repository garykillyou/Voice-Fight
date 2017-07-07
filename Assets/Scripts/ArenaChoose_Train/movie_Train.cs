using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(AudioSource))]

public class movie_Train : MonoBehaviour {

	public MovieTexture movieSource;
	public AudioSource audioSource;

	void Awake() {
		GetComponent<RawImage> ().texture = movieSource as MovieTexture;
		movieSource.loop = true;
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = movieSource.audioClip;
	}

	// Use this for initialization
	void Start () {/*
		GetComponent<RawImage> ().texture = movieSource as MovieTexture;
		movieSource.loop = true;
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = movieSource.audioClip;*/
		/*movieSource.Play();
		audio.Play();*/

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
