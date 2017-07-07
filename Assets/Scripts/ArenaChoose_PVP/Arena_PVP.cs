using UnityEngine;
using UnityEngine.Networking;

public class Arena_PVP : NetworkBehaviour
{

    private GameObject LButton;
    private GameObject RButton;
    private GameObject SButton;
    private GameObject SView;

    [SyncVar(hook = "OnLBClick")]
    public float LB;
    [SyncVar(hook = "OnRBClick")]
    public float RB;
    [SyncVar(hook = "OnSBClick")]
    public float SB;
    [SyncVar(hook = "OnSVDrag")]
    public float SV;
    [SyncVar(hook = "OnSVEndDrag")]
    public float SVEnd;

    // Use this for initialization
    void Start()
    {
        LButton = GameObject.Find("LeftButton");
        RButton = GameObject.Find("RightButton");
        SButton = GameObject.Find("SureButton");
        SView = GameObject.Find("Scroll View");

        if (isServer)
        {
            GameObject.Find("Cover").gameObject.SetActive(false);
        }
        else
        {
            LButton.gameObject.SetActive(false);
            RButton.gameObject.SetActive(false);
            SButton.gameObject.SetActive(false);
        }
    }

    void OnLBClick(float oLB)
    {
        LB = 2f;
        LButton.GetComponent<LeftButton_PVP>().IsClick(oLB);
    }

    void OnRBClick(float oRB)
    {
        RB = 2f;
        RButton.GetComponent<RightButton_PVP>().IsClick(oRB);
    }

    void OnSBClick(float oSB)
    {
        SB = oSB;
        SButton.GetComponent<SureButton_PVP>().IsClick();
    }

    void OnSVDrag(float oSV)
    {
        SV = oSV;
        SView.GetComponent<ScrollView_PVP>().IsDrag(oSV);
    }

    void OnSVEndDrag(float oSVEnd)
    {
        SVEnd = 2f;
        SView.GetComponent<ScrollView_PVP>().IsEndDrag(oSVEnd);
    }

    [Command]
    public void CmdLBClick(float V)
    {
        GameObject.Find("Arena_PVP").GetComponent<Arena_PVP>().LB = V;
    }

    [Command]
    public void CmdRBClick(float V)
    {
        GameObject.Find("Arena_PVP").GetComponent<Arena_PVP>().RB = V;
    }

    [Command]
    public void CmdSBClick()
    {
        GameObject.Find("Arena_PVP").GetComponent<Arena_PVP>().SB = 1f;
    }

    [Command]
    public void CmdSBClickEnd(string ArenaName)
    {
        GameObject.Find("NetworkManager").GetComponent<NetworkManager_My>().ChangeScene(ArenaName);
    }

    [Command]
    public void CmdSVDrag(float barValue)
    {
        GameObject.Find("Arena_PVP").GetComponent<Arena_PVP>().SV = barValue;
    }

    [Command]
    public void CmdSVEndDrag(float V)
    {
        GameObject.Find("Arena_PVP").GetComponent<Arena_PVP>().SVEnd = V;
    }
}