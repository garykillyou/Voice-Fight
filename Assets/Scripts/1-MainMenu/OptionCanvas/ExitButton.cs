using UnityEngine;
using System.Collections;

public class ExitButton : MonoBehaviour {


	public void onClick() {
		GetComponentInParent<Canvas>().enabled = false ;
	}
}
