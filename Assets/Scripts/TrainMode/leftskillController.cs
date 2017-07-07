using UnityEngine;
using System.Collections;

public class leftskillController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        gameObject.transform.position += new Vector3(-2f, 0, 0);

    }
}
