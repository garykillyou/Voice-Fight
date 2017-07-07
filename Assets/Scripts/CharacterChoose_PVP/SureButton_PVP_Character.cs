using UnityEngine;
using UnityEngine.UI;

public class SureButton_PVP_Character : MonoBehaviour
{

	public void OnClick ()
	{
        GetComponent<Button>().interactable = false;
        GameObject.FindGameObjectWithTag("player1Cursor").GetComponent<MouseTo_PVP_Host>().sureClick();
        GameObject.FindGameObjectWithTag("player2Cursor").GetComponent<MouseTo_PVP_Client>().sureClick();
	}
}