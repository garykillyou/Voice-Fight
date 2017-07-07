using UnityEngine;
using UnityEngine.Networking;

public class ToStart_PVP_Character : NetworkBehaviour
{

    public GameObject Cursor_Host;
    public GameObject Cursor_Client;

    // Use this for initialization
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Cursor") != null) Destroy(GameObject.FindGameObjectWithTag("Cursor"));

        if (isServer && GameObject.FindGameObjectWithTag("player1Cursor") == null)
        {
            ClientScene.localPlayers.Add(null);

            GameObject P1 = GameObject.Instantiate(Cursor_Host, Vector3.zero, Quaternion.identity) as GameObject;
            NetworkServer.AddPlayerForConnection(NetworkServer.connections[0], P1, 0);

            GameObject P2 = GameObject.Instantiate(Cursor_Client, Vector3.zero, Quaternion.identity) as GameObject;
            NetworkServer.AddPlayerForConnection(NetworkServer.connections[1], P2, 0);
        }

        
    }
}