using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(AudioSource))]

public class movie_PVP : MonoBehaviour {

	public MovieTexture movieSource;
	public AudioSource audioSource;

	void Awake() {
		GetComponent<RawImage> ().texture = movieSource as MovieTexture;
		movieSource.loop = true;
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = movieSource.audioClip;
	}
}