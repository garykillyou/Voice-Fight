using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionButton : MonoBehaviour
{

	public void onClick ()
	{
		GameObject.Find ("OptionCanvas").GetComponent<Canvas> ().enabled = true;
		GameObject.Find ("OptionCanvas").transform.localScale = new Vector3 (1, 1, 1);
		GameObject.Find ("OptionCanvas").transform.localPosition = new Vector3 (0, 0);
	}
}
