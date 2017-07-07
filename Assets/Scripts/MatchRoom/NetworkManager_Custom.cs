using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*public class MyMsgTypes : MessageBase
{
    public string method;
};*/

public class NetworkManager_Custom : NetworkManager
{

    public GameObject TextState;

    public int Player = 1;

    public bool isPVPEnd = false;
    
    public bool ReadyGo = false;

    //private const short m_MyMsgTypes = 64;
    
    public void StartupHost()
    {
        Player = 1;
        SetPort();
        NetworkManager.singleton.StartHost();
        TextState.GetComponent<Text>().text = "就是愛開房間!!!";

        GameObject.Find("ButtonStartHost").GetComponent<Button>().interactable = false;
        GameObject.Find("ButtonJoinGame").GetComponent<Button>().interactable = false;
        GameObject.Find("ButtonExit").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonExit").GetComponent<Button>().onClick.AddListener(DisconnectHostExit);
    }

    public void JoinGame()
    {
        Player = 2;
        SetIPAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
        TextState.GetComponent<Text>().text = "我要當房客~~~";

        GameObject.Find("ButtonStartHost").GetComponent<Button>().interactable = false;
        GameObject.Find("ButtonJoinGame").GetComponent<Button>().interactable = false;
        GameObject.Find("ButtonExit").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonExit").GetComponent<Button>().onClick.AddListener(DisconnectClientExit);
    }

    public void ExitPage()
    {
        SceneManager.LoadScene("1-MainMenu");
    }

    public void DisconnectHostExit()
    {
        NetworkManager.singleton.StopHost();
        SceneManager.LoadScene("1-MainMenu");
    }

    public void DisconnectHostExit_PVPEnd()
    {
        /*MyMsgTypes msg = new MyMsgTypes();
        msg.method = "Player 1 Disconnect";
        NetworkServer.SendToAll(m_MyMsgTypes, msg);*/
    }

    public void DisconnectClientExit()
    {
        NetworkManager.singleton.StopClient();
        SceneManager.LoadScene("1-MainMenu");
    }

    public void DisconnectClientExit_PVPEnd()
    {
        NetworkManager.singleton.StopClient();
    }

    void SetIPAddress()
    {
        string ipAddress = GameObject.Find("InputFieldIPAddress").transform.FindChild("Text").GetComponent<Text>().text;
        NetworkManager.singleton.networkAddress = ipAddress;
    }

    void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        print("OnServerConnect");
        //print(NetworkManager.singleton.spawnPrefabs.Count);

        if (NetworkServer.connections.Count == 2)
        {
            print("Is2OnServerConnect");

            /*MyMsgTypes msg = new MyMsgTypes();
            msg.method = "ReadyEnterGame";
            NetworkServer.SendToAll(m_MyMsgTypes, msg);
            */
            //ChangeScene("PVPMode");
            GameObject.Find("Counter_MatchRoom").GetComponent<NetworkManager_room>().CmdTellServerReadyToPlay();
        }
    }

    /*public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        print("OnServerAddPlayer");
        //print(NetworkManager.singleton.spawnPrefabs.Count);

        NetworkServer.AddPlayerForConnection(conn, NetworkManager.singleton.spawnPrefabs[3], playerControllerId);
    }*/

    public void OneMoreCounter()
    {
        print("OneMoreCounter");
        GameObject.Find("Counter_MatchRoom").GetComponent<NetworkManager_room>().CmdTellServerReadyToPlay();
    }

    /*public override void OnServerDisconnect(NetworkConnection conn)
    {
        print("OnServerDisconnect");
        if (isPVPEnd)
        {
            MyMsgTypes msg = new MyMsgTypes();
            msg.method = "Player 2 Disconnect";
            NetworkServer.SendToAll(m_MyMsgTypes, msg);
        }
    }*/

    public override void OnServerSceneChanged(string sceneName)
    {
        print("OnServerSceneChanged");
        print("networkSceneNameServer is " + sceneName);

        NetworkServer.SetClientReady(NetworkServer.connections[0]);
        NetworkServer.SetClientReady(NetworkServer.connections[1]);
        if (sceneName == "PVPMode")
        {
            /* GameObject P1 = GameObject.Instantiate(NetworkManager.singleton.spawnPrefabs[3], new Vector3(-36f, -8f, 0f), Quaternion.identity) as GameObject;
             GameObject P2 = GameObject.Instantiate(NetworkManager.singleton.spawnPrefabs[4], new Vector3(36f, -8f, 0f), Quaternion.identity) as GameObject;
             */

             /*NetworkServer.AddPlayerForConnection(NetworkServer.connections[1], GameObject.FindGameObjectWithTag("player2"), 0);
             NetworkServer.AddPlayerForConnection(NetworkServer.connections[0], GameObject.FindGameObjectWithTag("player1"), 0);*/
            
        }
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        print("OnClientSceneChanged");
        print("networkSceneName is " + networkSceneName);

        /*if (networkSceneName == "PVPMode")
        {
            
            ClientScene.AddPlayer(NetworkManager.singleton.client.connection, 0);
        }*/
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        print("OnClientConnect");
        
    }

    /*void ClientGetMethod(NetworkMessage netMsg)
    {

        MyMsgTypes Msg = netMsg.ReadMessage<MyMsgTypes>();
        print("Server say : "+ Msg.method);
        if (Msg.method == "ReadyEnterGame")
        {
            GameObject.Find("TextState").GetComponent<NetworkManager_room>().ReadyEnterGame();
        }
        else if (Msg.method == "Player 1 Disconnect")
        {
            GameObject.Find("PVPEndFF").GetComponent<PVPEnd>().P1Disconnect();
        }
        else if (Msg.method == "Player 2 Disconnect")
        {
            GameObject.Find("PVPEndFF").GetComponent<PVPEnd>().P2Disconnect();
        }
    }*/

    public void ChangeScene(string sceneName)
    {
        print("Time to change scene!!!");
        ServerChangeScene(sceneName);
    }
}