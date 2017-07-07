using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class CounterText : NetworkBehaviour
{

    public int time;
    public Vector3 speed;
    public Vector3 UNspeed;
    public GameObject counterTime;
    private Rigidbody m_rigidbody;
    private int counter = 3;

    [SyncVar(hook = "OnStartCounter")]
    public bool startCC = false;

    [SyncVar(hook = "OnShowK_O")]
    public bool endCC = false;

    // Use this for initialization
    void Start()
    {

        m_rigidbody = GetComponent<Rigidbody>();
        GetComponent<TextMesh>().text = counter.ToString();
        counterTime.GetComponent<Text>().text = time.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < -80f)
        {
            m_rigidbody.velocity = Vector3.zero;
            transform.position = new Vector3(GameObject.Find("Main Camera").gameObject.transform.position.x, -3f, -80f);
        }
    }

    void DownCounter()
    {

        if (time > -1)
        {
            counterTime.GetComponent<Text>().text = time.ToString();
            time--;
        }
        else
        {
            CancelInvoke();

            if (isServer)
            {
                if (GameFunction_PVP.Instance.Scores1 > GameFunction_PVP.Instance.Scores2)
                {
                    PlayerPrefs.SetInt("WhoWin", 1);
                }
                else PlayerPrefs.SetInt("WhoWin", 2);

                CmdTellServerToCounter(false);

            }
        }
    }

    public void CounterReady()
    {
        if (counter > 0)
        {
            m_rigidbody.velocity = speed;
            transform.position = new Vector3(0f, 0f, -70f);
            GetComponent<TextMesh>().text = counter.ToString();
            counter--;
        }
        else if (counter == 0)
        {
            transform.position = new Vector3(0f, 0f, -70f);
            GetComponent<TextMesh>().text = "GO!";
            counter--;
            if (isServer) CmdTellServerToStart();
        }
        else
        {
            CancelInvoke();

            GameObject.Find("MovieWall").GetComponent<MovieWall>().Play();
            m_rigidbody.velocity = Vector3.zero;
            transform.position = new Vector3(0f, -3f, 1f);
            InvokeRepeating("DownCounter", 0.0f, 1.0f);

        }
    }

    void OnStartCounter(bool cc)
    {
        startCC = cc;
        if (startCC) InvokeRepeating("CounterReady", 0.5f, 1.0f);
        else OnShowT_O();
    }

    public void StartTimeCounter()
    {
        InvokeRepeating("DownCounter", 0.0f, 1.0f);
    }

    public void OnShowK_O(bool cc)
    {
        Debug.Log("KOKO");

        GetComponent<TextMesh>().text = "K.O";
        GetComponent<MeshRenderer>().sortingOrder = 2;
        transform.position = new Vector3(GameObject.Find("Main Camera").gameObject.transform.position.x, -3f, 1f);
        m_rigidbody.velocity = UNspeed;

        Invoke("GoPVPEnd", 2f);
    }

    public void OnShowT_O()
    {
        Debug.Log("Time Out");

        GetComponent<TextMesh>().text = "Time Out";
        GetComponent<TextMesh>().fontSize = 85;
        GetComponent<MeshRenderer>().sortingOrder = 2;
        transform.position = new Vector3(GameObject.Find("Main Camera").gameObject.transform.position.x, -3f, 1f);
        m_rigidbody.velocity = UNspeed;

        Invoke("GoPVPEnd", 2f);
    }

    void GoPVPEnd()
    {
        GameObject.Find("GameFuntionObject").GetComponent<GameFunction_PVP>().GameOver();
        GameObject.Find("NetworkManager").GetComponent<NetworkManager_My>().ChangeScene("PVPEnd");
    }

    [Command]
    void CmdTellServerToCounter(bool S)
    {
        GameFunction_PVP.Instance.IsPlaying = false;
        print("no play");
        GameObject.Find("CounterText").GetComponent<CounterText>().startCC = S;
    }

    [Command]
    void CmdTellServerToStart()
    {
        GameFunction_PVP.Instance.IsPlaying = true;
        print("play");
    }
}