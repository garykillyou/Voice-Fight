using UnityEngine;

public class Background_Song : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject[] musicInstant;
		musicInstant = GameObject.FindGameObjectsWithTag( "BackgroundSong" );
		if ( musicInstant.Length > 1 ) Destroy( this.gameObject );

		DontDestroyOnLoad( this.gameObject );
	}
}