using UnityEngine;
using UnityEngine.Networking;

public class ToStart_PVP_Arena : NetworkBehaviour
{

    public GameObject Arena_PVP;

    // Use this for initialization
    void Start()
    {
        if (isServer)
        {
            /*GameObject AP = Instantiate(Arena_PVP, Vector3.zero, Quaternion.identity) as GameObject;
            NetworkServer.Spawn(AP);*/
            GameObject.FindGameObjectWithTag("player1Cursor").GetComponent<MouseTo_PVP_Host>().SetVisable(false);
        }
        else
        {
            GameObject.FindGameObjectWithTag("player2Cursor").GetComponent<MouseTo_PVP_Client>().SetVisable(false);
        }
    }
}