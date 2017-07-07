using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Canvas_PVP : NetworkBehaviour
{
	public GameObject LButton;
	public GameObject RButton;
	public GameObject SButton;
	public GameObject SView;

	[SyncVar (hook = "OnLBClick")] public float LB = 0;
	[SyncVar (hook = "OnRBClick")] public float RB = 0;
	[SyncVar (hook = "OnSBClick")] public float SB = 0;
	[SyncVar (hook = "OnSVDrag")] public float SV = 0;
	[SyncVar (hook = "OnSVEndDrag")] public float SVEnd = 0;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnLBClick (float oLB)
	{
		LB = oLB;
		LButton.GetComponent<LeftButton_PVP> ().IsClick (oLB);
	}

	void OnRBClick (float oRB)
	{
		RB = oRB;
		RButton.GetComponent<RightButton_PVP> ().IsClick (oRB);
	}

	void OnSBClick (float oSB)
	{
		SB = oSB;
		SButton.GetComponent<SureButton_PVP> ().IsClick ();
	}

	void OnSVDrag (float oSV)
	{
		SV = oSV;
		SView.GetComponent<ScrollView_PVP> ().IsDrag (oSV);
	}

	void OnSVEndDrag (float oSVEnd)
	{
		SVEnd = oSVEnd;
		SView.GetComponent<ScrollView_PVP> ().IsEndDrag (oSVEnd);
	}

	[Command]
	public void CmdLBClick ()
	{
		GameObject.Find ("Canvas").GetComponent<Canvas_PVP> ().LB++;
	}

	[Command]
	public void CmdRBClick ()
	{
		GameObject.Find ("Canvas").GetComponent<Canvas_PVP> ().RB++;
		//GameObject.Find ("RightButton").GetComponent<RightButton_PVP> ().IsClick ();
	}

	[Command]
	public void CmdSBClick ()
	{
		GameObject.Find ("Canvas").GetComponent<Canvas_PVP> ().SB++;
	}

	[Command]
	public void CmdSBClickEnd ()
	{
		GameObject.Find ("NetworkManager").GetComponent<NetworkManager_Custom> ().ChangeScene ("TrainMode");
	}

	[Command]
	public void CmdSVDrag (float barValue)
	{
		GameObject.Find ("Canvas").GetComponent<Canvas_PVP> ().SV = barValue;
	}

	[Command]
	public void CmdSVEndDrag ()
	{
		GameObject.Find ("Canvas").GetComponent<Canvas_PVP> ().SVEnd++;
	}
}
