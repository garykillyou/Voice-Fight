using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TrainButton : MonoBehaviour {

	public void ChangeScene ( string SceneLevel ) {
		SceneManager.LoadScene( SceneLevel );
	}
}
