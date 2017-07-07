using SmartLocalization;
using SmartLocalization.Editor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManager_My : NetworkManager
{

    public GameObject TextState;

    public int Player;

    void Start()
    {
        NetworkManager.singleton.clientLoadedScene = false;
        //TextState = GameObject.Find("TextState");
    }

    public void StartupHost()
    {
        Player = 1;
        SetPort();

        NetworkManager.singleton.offlineScene = "MatchRoom";
        NetworkManager.singleton.StartHost();
        
        GameObject.Find("TextState").GetComponent<LocalizedText>().localizedKey = "Game is created";
        LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture.languageCode);

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
        GameObject.Find("TextState").GetComponent<LocalizedText>().localizedKey = "Join game ing";
        LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture.languageCode);

        GameObject.Find("ButtonStartHost").GetComponent<Button>().interactable = false;
        GameObject.Find("ButtonJoinGame").GetComponent<Button>().interactable = false;
        GameObject.Find("ButtonExit").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonExit").GetComponent<Button>().onClick.AddListener(DisconnectClientExit);
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

    public void ExitPage()
    {
        SceneManager.LoadScene("1-MainMenu");
    }

    public void DisconnectHostExit()
    {
        NetworkManager.singleton.StopHost();
        SceneManager.LoadScene("1-MainMenu");
    }

    public void DisconnectClientExit()
    {
        NetworkManager.singleton.StopClient();
        SceneManager.LoadScene("1-MainMenu");
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        print("OnServerConnect");

        if (NetworkServer.connections.Count == 2)
        {
            print("Is2OnServerConnect");

            GameObject.Find("Counter_MatchRoom").GetComponent<NetworkManager_room>().CmdTellServerReadyToPlay();
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        print("OnServerDisconnect");
        if (networkSceneName == "PVPEnd")
        {
            GameObject.Find("PVPEndFF").GetComponent<PVPEnd>().OtherDisconnect();
        }
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        print("OnClientDisconnect");
        if (networkSceneName == "PVPEnd")
        {
            GameObject.Find("PVPEndFF").GetComponent<PVPEnd>().OtherDisconnect();
        }
    }

    /*public override void OnClientSceneChanged(NetworkConnection conn)
    {
        print("OnClientSceneChanged");
        print(networkSceneName);

        if (networkSceneName == "CharacterChoose_PVP")
        {
            GameObject.FindGameObjectWithTag("player1Cursor").GetComponent<MouseTo_PVP_Host>().imagePath = 0;
            GameObject.FindGameObjectWithTag("player1Cursor").GetComponent<MouseTo_PVP_Host>().determineP1 = false;
            GameObject.FindGameObjectWithTag("player2Cursor").GetComponent<MouseTo_PVP_Client>().imagePath = 0;
            GameObject.FindGameObjectWithTag("player2Cursor").GetComponent<MouseTo_PVP_Client>().determineP2 = false;
        }
    }*/

    /*public override void OnClientSceneChanged(NetworkConnection conn)
    {
        print("OnClientSceneChanged");
        print(networkSceneName);

        if (networkSceneName == "PVPMode")
        {
            ClientScene.Ready()
            if (Player == 1)
                GameObject.FindGameObjectWithTag("player1Cursor").GetComponent<MouseTo_PVP_Host>().SetVisable(false);
            else if (Player == 2)
                GameObject.FindGameObjectWithTag("player2Cursor").GetComponent<MouseTo_PVP_Client>().SetVisable(false);
        }
        else if (networkSceneName == "PVPEnd")
        {
            if (Player == 1)
                GameObject.FindGameObjectWithTag("player1Cursor").GetComponent<MouseTo_PVP_Host>().SetVisable(true);
            else if (Player == 2)
                GameObject.FindGameObjectWithTag("player2Cursor").GetComponent<MouseTo_PVP_Client>().SetVisable(true);
        }
    }*/

    public void ChangeScene(string sceneName)
    {
        print("Time to change scene!!!");
        ServerChangeScene(sceneName);
    }
}