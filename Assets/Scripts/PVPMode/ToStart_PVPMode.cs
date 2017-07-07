using UnityEngine;
using UnityEngine.Networking;

public class ToStart_PVPMode : NetworkBehaviour
{

    public GameObject BoxMan1;
    public GameObject BoxMan2;
    public GameObject UnknowMan1;
    public GameObject UnknowMan2;

    [SyncVar(hook = "OnStart")]
    public bool start = false;

    // Use this for initialization
    void Start()
    {
        GameObject.FindGameObjectWithTag("player1Cursor").GetComponent<MouseTo_PVP_Host>().SetVisable(false);
        GameObject.FindGameObjectWithTag("player2Cursor").GetComponent<MouseTo_PVP_Client>().SetVisable(false);

        if (isServer)
        {
            print("is Server PVPMode~");

            ClientScene.localPlayers.Add(null);
            GameObject P1;
            GameObject P2;

            if (PlayerPrefs.GetInt("P1Num") == 1)
            {
                P1 = GameObject.Instantiate(BoxMan1, new Vector3(-36f, -8f, 0f), Quaternion.identity) as GameObject;
            }
            else if (PlayerPrefs.GetInt("P1Num") == 2)
            {
                P1 = GameObject.Instantiate(UnknowMan1, new Vector3(-36f, -8f, 0f), Quaternion.identity) as GameObject;
            }
            else
            {
                P1 = GameObject.Instantiate(BoxMan1, new Vector3(-36f, -8f, 0f), Quaternion.identity) as GameObject;
            }

            if (PlayerPrefs.GetInt("P2Num") == 1)
            {
                P2 = GameObject.Instantiate(BoxMan2, new Vector3(36f, -8f, 0f), Quaternion.identity) as GameObject;
            }
            else if (PlayerPrefs.GetInt("P2Num") == 2)
            {
                P2 = GameObject.Instantiate(UnknowMan2, new Vector3(36f, -8f, 0f), Quaternion.identity) as GameObject;
            }
            else
            {
                P2 = GameObject.Instantiate(UnknowMan2, new Vector3(36f, -8f, 0f), Quaternion.identity) as GameObject;
            }

            NetworkServer.AddPlayerForConnection(NetworkServer.connections[0], P1, 1);
            NetworkServer.AddPlayerForConnection(NetworkServer.connections[1], P2, 1);

            CmdTellServerStart();
        }
    }

    void OnStart(bool S)
    {
        GameObject.Find("Main Camera").GetComponent<MainCamera_PVP>().ConnectGameObjectAndStart();
    }

    [Command]
    void CmdTellServerStart()
    {

        GameObject.Find("ToStart").GetComponent<ToStart_PVPMode>().start = true;
        GameObject.Find("CounterText").GetComponent<CounterText>().startCC = true;
    }
}