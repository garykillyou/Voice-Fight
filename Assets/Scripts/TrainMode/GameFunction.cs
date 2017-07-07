using UnityEngine;

public class GameFunction : MonoBehaviour
{

    public int Scores1 = 100; // 宣告一整數 Score
    public int Scores2 = 100; // 宣告一整數 Score

    public bool IsPlaying = false; // 宣告IsPlaying 的布林資料，並設定初始值false

    public GameObject RealMan1;
    public GameObject RealMan2;

    public GameObject UnknowMan1;
    public GameObject UnknowMan2;

    public bool p1hited = false;
    public bool p1hit = false;
    public bool p2hited = false;
    public bool p2hit = false;
    public bool p1hitended = false; // 被重擊打中
    public bool p2hitended = false; // 被重擊打中
    public bool p1up = false; //起身
    public bool p2up = false; //起身
    public Vector3 pos1;
    public Vector3 pos2;
    public int p1hittime = 0;
    public int p2hittime = 0;

    public static GameFunction Instance; // 設定Instance，讓其他程式能讀取
                                         // Use this for initialization
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Cursor") != null) GameObject.FindGameObjectWithTag("Cursor").GetComponent<MouseTo>().SetVisable(false);

        if (PlayerPrefs.GetInt("P1Num") == 1)
        {
            Instantiate(RealMan1, new Vector3(-53f, -9f, 0), gameObject.transform.rotation); // 生成PLAYER 1
        }
        else if (PlayerPrefs.GetInt("P1Num") == 2)
        {
            Instantiate(UnknowMan1, new Vector3(-53f, -9f, 0), gameObject.transform.rotation); // 生成PLAYER 1
        }
        else
        {
            Instantiate(RealMan1, new Vector3(-53f, -9f, 0), gameObject.transform.rotation); // 生成PLAYER 1
        }

        if (PlayerPrefs.GetInt("P2Num") == 1)
        {
            Instantiate(RealMan2, new Vector3(53, -9f, 0), gameObject.transform.rotation); // 生成 PLAYER 2
        }
        else if (PlayerPrefs.GetInt("P2Num") == 2)
        {
            Instantiate(UnknowMan2, new Vector3(53, -9f, 0), gameObject.transform.rotation); // 生成 PLAYER 2

        }
        else
        {
            Instantiate(UnknowMan2, new Vector3(-53f, -9f, 0), gameObject.transform.rotation); // 生成PLAYER 2
        }

        GameObject.Find("Main Camera").GetComponent<MainCamera_TrainMode>().ConnectGameObjectAndStart();
        GameObject.Find("CounterText").GetComponent<CounterText_TrainMode>().OnStartCounter();
    }

    public void Score1(int D)
    {
        p1hited = false;
        Scores1 = Scores1 - D; //分數 - D
        if (Scores1 > 100) Scores1 = 100;
        if (Scores1 <= 0 && IsPlaying)
        {
            IsPlaying = false;

            //PlayerPrefs.SetInt("WhoWin", 2);
            GameOverK_O();
        }
        GameObject.Find("P1Blood").GetComponent<BloodSlider>().HP = Scores1;
    }

    public void Score2(int D)
    {
        p2hited = false;
        Scores2 = Scores2 - D; //分數 - D
        if (Scores2 > 100) Scores2 = 100;
        if (Scores2 <= 0 && IsPlaying)
        {
            IsPlaying = false;

            //PlayerPrefs.SetInt("WhoWin", 1);
            GameOverK_O();
        }
        GameObject.Find("P2Blood").GetComponent<BloodSlider>().HP = Scores2;
    }

    public void GameOverK_O()
    {
        PlayerPrefs.SetInt("P1AtkNum", p1hittime);
        PlayerPrefs.SetInt("P2AtkNum", p2hittime);
        GameObject.Find("CounterText").GetComponent<CounterText_TrainMode>().ShowK_O();
    }

    public void GameOverT_O()
    {
        IsPlaying = false;

        if (Scores1 > Scores2)
        {
            PlayerPrefs.SetInt("WhoWin", 1);
        }
        else
        {
            PlayerPrefs.SetInt("WhoWin", 2);
        }
        PlayerPrefs.SetInt("P1AtkNum", p1hittime);
        PlayerPrefs.SetInt("P2AtkNum", p2hittime);
        GameObject.Find("CounterText").GetComponent<CounterText_TrainMode>().ShowT_O();
    }
}