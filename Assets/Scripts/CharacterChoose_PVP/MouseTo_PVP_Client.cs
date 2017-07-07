using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MouseTo_PVP_Client : NetworkBehaviour
{

    [SyncVar(hook = "ChangeImage2")]
    public int imagePath = 0;

    [SyncVar(hook = "sureP2")]
    public bool determineP2 = false;

    private GameObject P2CharacterImage;

    public GameObject content;
    public GameObject content2;

    private Vector3 mouseP;

    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;

        GameObject NetworkMM = GameObject.Find("NetworkManager");

        int who = NetworkMM.GetComponent<NetworkManager_My>().Player;

        if (who == 1)
        {
            GameObject.FindGameObjectWithTag("player1Cursor").GetComponent<MouseTo_PVP_Host>().SetVisable(true);
        }
        else if (who == 2)
        {
            GameObject.FindGameObjectWithTag("player2Cursor").GetComponent<MouseTo_PVP_Client>().SetVisable(true);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            mouseP = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100f));

            /*if (Input.GetMouseButtonDown (0)) {
                Debug.Log (Input.mousePosition);
                Debug.Log (mouseP);
            }

            if(Input.GetMouseButtonDown (1)) {
                Cursor.visible = true;
            }*/

            this.transform.position = new Vector3(mouseP.x, mouseP.y);
        }
    }

    public void Onclick(int Num)
    {
        if (isLocalPlayer) CmdTellServerP2SelectCharacter(Num);
    }

    public void sureClick()
    {
        if (isLocalPlayer) CmdTellServerP2Determine();
    }

    void ChangeImage2(int m_path)
    {
        P2CharacterImage = GameObject.Find("P2CharacterImage");

        if (m_path == 1)
        {
            P2CharacterImage.GetComponent<RawImage>().texture = Resources.Load("CharacterChoose_Train/BoxManR") as Texture;
        }
        else if (m_path == 2)
        {
            P2CharacterImage.GetComponent<RawImage>().texture = Resources.Load("CharacterChoose_Train/UnknowManR") as Texture;
        }

        PlayerPrefs.SetInt("P2Num", m_path);
    }

    void sureP2(bool m_P2)
    {
        determineP2 = m_P2;
        Debug.Log("p2p2p2");
        if (determineP2 && GameObject.FindGameObjectWithTag("player1Cursor").GetComponent<MouseTo_PVP_Host>().determineP1)
        {
            CmdTellServerOK();
        }
    }

    public void SetVisable(bool V)
    {
        content.GetComponent<SpriteRenderer>().enabled = V;
        content2.GetComponent<SpriteRenderer>().enabled = V;
    }

    public void TellServerP2WantToPlayAgain()
    {
        CmdTellServerP2WantToPlayAgain();
    }

    [Command]
    void CmdTellServerP2SelectCharacter(int Num)
    {
        GameObject.FindGameObjectWithTag("player2Cursor").GetComponent<MouseTo_PVP_Client>().imagePath = Num;
    }

    [Command]
    void CmdTellServerP2Determine()
    {
        GameObject.FindGameObjectWithTag("player2Cursor").GetComponent<MouseTo_PVP_Client>().determineP2 = true;
    }

    [Command]
    void CmdTellServerOK()
    {
        GameObject.Find("NetworkManager").GetComponent<NetworkManager_My>().ChangeScene("ArenaChoose_PVP");
    }

    [Command]
    void CmdTellServerP2WantToPlayAgain()
    {
        GameObject.Find("PVPEndFF").GetComponent<PVPEnd>().PlayAgain = 2;
    }

    public void TellServerChangeScene(string sceneName)
    {
        if (isLocalPlayer) CmdTellServerChangeScene(sceneName);
    }

    [Command]
    public void CmdTellServerChangeScene(string sceneName)
    {
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager_My>().ChangeScene(sceneName);
    }
}