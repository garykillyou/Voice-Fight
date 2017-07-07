using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PVPButton : MonoBehaviour {

	/*// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}*/

	public void ChangeScene ( string SceneLevel ) {
		SceneManager.LoadScene( SceneLevel );
	}
}
