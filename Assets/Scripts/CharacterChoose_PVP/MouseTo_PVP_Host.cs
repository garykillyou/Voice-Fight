using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MouseTo_PVP_Host : NetworkBehaviour
{

    [SyncVar(hook = "ChangeImage1")]
    public int imagePath = 0;

    [SyncVar(hook = "sureP1")]
    public bool determineP1 = false;

    private GameObject P1CharacterImage;

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
        if (isLocalPlayer) CmdTellServerP1SelectCharacter(Num);
    }

    public void sureClick()
    {
        if (isLocalPlayer) CmdTellServerP1Determine();
    }

    void ChangeImage1(int m_path)
    {

        P1CharacterImage = GameObject.Find("P1CharacterImage");

        if (m_path == 1)
        {
            P1CharacterImage.GetComponent<RawImage>().texture = Resources.Load("CharacterChoose_Train/BoxManL") as Texture;
        }
        else if (m_path == 2)
        {
            P1CharacterImage.GetComponent<RawImage>().texture = Resources.Load("CharacterChoose_Train/UnknowManL") as Texture;
        }

        PlayerPrefs.SetInt("P1Num", m_path);
    }

    void sureP1(bool m_P1)
    {
        determineP1 = m_P1;
        Debug.Log("p1p1p1");
        if (determineP1 && GameObject.FindGameObjectWithTag("player2Cursor").GetComponent<MouseTo_PVP_Client>().determineP2)
        {
            CmdTellServerOK();
        }
    }

    public void SetVisable(bool V)
    {
        content.GetComponent<SpriteRenderer>().enabled = V;
        content2.GetComponent<SpriteRenderer>().enabled = V;
    }

    public void TellServerP1WantToPlayAgain()
    {
        CmdTellServerP1WantToPlayAgain();
    }

    [Command]
    void CmdTellServerP1SelectCharacter(int Num)
    {
        GameObject.FindGameObjectWithTag("player1Cursor").GetComponent<MouseTo_PVP_Host>().imagePath = Num;
    }

    [Command]
    void CmdTellServerP1Determine()
    {
        GameObject.FindGameObjectWithTag("player1Cursor").GetComponent<MouseTo_PVP_Host>().determineP1 = true;
    }

    [Command]
    void CmdTellServerOK()
    {
        GameObject.Find("NetworkManager").GetComponent<NetworkManager_My>().ChangeScene("ArenaChoose_PVP");
    }

    [Command]
    void CmdTellServerP1WantToPlayAgain()
    {
        GameObject.Find("PVPEndFF").GetComponent<PVPEnd>().PlayAgain = 1;
    }

    public void TellServerChangeScene(string sceneName)
    {
        if(isLocalPlayer) CmdTellServerChangeScene(sceneName);
    }

    [Command]
    public void CmdTellServerChangeScene(string sceneName)
    {
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager_My>().ChangeScene(sceneName);
    }
}