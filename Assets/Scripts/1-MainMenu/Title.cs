using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Title : MonoBehaviour
{

	public void OnPointerEnter ()
	{
		GetComponent<Text> ().color = Color.yellow;
	}

	public void OnPointExit ()
	{
		GetComponent<Text> ().color = Color.red;
	}
}
