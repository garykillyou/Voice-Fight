using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SmartLocalization;
using SmartLocalization.Editor;

public class NetworkManager_room : NetworkBehaviour
{

    public string nextScene;

    public Text TextState;

    public int counter;

    [SyncVar(hook = "OnReadyPlay")]
    public bool ReadyPlay = false;

    void Start()
    {
        TextState = GameObject.Find("TextState").GetComponent<Text>();
    }

    public void OnReadyPlay(bool rp)
    {
        print("OnReadyPlay");

        InvokeRepeating("CountToStart", 0.5f, 1f);
        
        print("OnReadyPlay END");
    }

    void CountToStart()
    {
        if (counter > 0)
        {
            TextState.text = counter.ToString() + " " + LanguageManager.Instance.GetTextValue("Counter");
            print("倒數 " + counter);
            counter--;
        }
        else if (counter == 0)
        {
            CancelInvoke();

            if (isServer)
            {
                GameObject.Find("NetworkManager").GetComponent<NetworkManager_My>().ChangeScene(nextScene);
            }
        }
    }

    [Command]
    public void CmdTellServerReadyToPlay()
    {
        NetworkServer.SetClientReady(NetworkServer.connections[0]);
        NetworkServer.SetClientReady(NetworkServer.connections[1]);
        GameObject.Find("Counter_MatchRoom").GetComponent<NetworkManager_room>().ReadyPlay = true;
    }
}