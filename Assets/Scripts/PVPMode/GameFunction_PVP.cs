using UnityEngine;
using UnityEngine.Networking;

public class GameFunction_PVP : NetworkBehaviour
{

    [SyncVar(hook = "OnScore1Change")]
    public int Scores1 = 100; // 宣告一整數 Score
    [SyncVar(hook = "OnScore2Change")]
    public int Scores2 = 100; // 宣告一整數 Score

    [SyncVar]
    public bool IsPlaying = false; // 宣告IsPlaying 的布林資料，並設定初始值false

    [SyncVar]
    public bool p1hited = false;
    [SyncVar]
    public bool p1hit = false;
    [SyncVar]
    public bool p2hited = false;
    [SyncVar]
    public bool p2hit = false;
    [SyncVar]
    public bool p1hitended = false; // 被重擊打中
    [SyncVar]
    public bool p2hitended = false; // 被重擊打中
    [SyncVar]
    public bool p1up = false; //起身
    [SyncVar]
    public bool p2up = false; //起身

    [SyncVar]
    public int p1hittime = 0;
    [SyncVar]
    public int p2hittime = 0;

    [SyncVar]
    public Vector3 pos1;
    [SyncVar]
    public Vector3 pos2;

    public static GameFunction_PVP Instance; // 設定Instance，讓其他程式能讀取
                                             // Use this for initialization

    void Start()
    {

        Instance = this;

        GameObject t = GameObject.FindGameObjectWithTag("Cursor");
        if (t != null) t.GetComponent<MouseTo>().SetVisable(false);
    }

    public void Score1()
    {
        //p1hited = false;
        GameObject.FindGameObjectWithTag("player1").GetComponent<BoxMan1_PVP>().CmdTellServerP1hited(false);

        //Scores1 = Scores1 - 10; //分數-10
        GameObject.FindGameObjectWithTag("player1").GetComponent<BoxMan1_PVP>().CmdTellServerP1Score(10);


    }

    void OnScore1Change(int HP)
    {
        Scores1 = HP;
        GameObject.Find("P1Blood").GetComponent<BloodSlider>().HP = HP;
        if (HP <= 0)
        {
            PlayerPrefs.SetInt("WhoWin", 2);
            CmdTellServerGameOver();
        }
    }

    public void Score2()
    {
        //p2hited = false;
        GameObject.FindGameObjectWithTag("player2").GetComponent<BoxMan2_PVP>().CmdTellServerP2hited(false);

        //Scores2 = Scores2 - 10; //分數-10
        GameObject.FindGameObjectWithTag("player2").GetComponent<BoxMan2_PVP>().CmdTellServerP2Score(10);


    }

    void OnScore2Change(int HP)
    {
        Scores2 = HP;
        GameObject.Find("P2Blood").GetComponent<BloodSlider>().HP = HP;
        if (HP <= 0)
        {
            PlayerPrefs.SetInt("WhoWin", 1);
            CmdTellServerGameOver();
        }
    }

    public void GameOver() //GameOver的Function
    {
        PlayerPrefs.SetInt("P1AtkNum", p1hittime);
        PlayerPrefs.SetInt("P2AtkNum", p2hittime);
    }

    [Command]
    public void CmdTellServerGameOver()
    {
        GameFunction_PVP.Instance.IsPlaying = false;
        GameObject.Find("CounterText").GetComponent<CounterText>().endCC = true;
    }
}