using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class CounterText_TrainMode : MonoBehaviour
{

    public int time;
    public Vector3 speed;
    public Vector3 UNspeed;
    public GameObject counterTime;
    private Rigidbody m_rigidbody;
    public int counter = 3;

    KeywordRecognizer m_KeywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    // Use this for initialization
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        counterTime.GetComponent<Text>().text = time.ToString();

        if (PhraseRecognitionSystem.isSupported)
        {
            keywords.Add("random", () =>
            {
                HealCalled();
            });

            keywords.Add("yes", () =>
            {
                HealCalled();
            });

            keywords.Add("fuck", () =>
            {
                HealCalled();
            });

            m_KeywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
            m_KeywordRecognizer.OnPhraseRecognized += KeywordRecognizerOnPhraseRecognized;
            m_KeywordRecognizer.Start();
        }
    }

    void KeywordRecognizerOnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    void HealCalled()
    {
        print("You say heal !");
        int A = time % 2;
        if (A == 1)
        {
            GameFunction.Instance.Score1(-10);
        }
        else
        {
            GameFunction.Instance.Score2(-10);
        }
    }

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
            GameFunction.Instance.GameOverT_O();
            //ShowK_O();
        }
    }

    void CounterStart()
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
            GameFunction.Instance.IsPlaying = true;
            GameObject.Find("MovieWall").GetComponent<MovieWall>().Play();
        }
        else
        {
            CancelInvoke();

            m_rigidbody.velocity = Vector3.zero;
            transform.position = new Vector3(0f, -3f, 1f);
            InvokeRepeating("DownCounter", 0.0f, 1.0f);
        }
    }

    public void OnStartCounter()
    {
        InvokeRepeating("CounterStart", 0.5f, 1f);
    }

    public void ShowK_O()
    {
        GetComponent<TextMesh>().text = "K.O";
        GetComponent<MeshRenderer>().sortingOrder = 2;
        transform.position = new Vector3(GameObject.Find("Main Camera").gameObject.transform.position.x, -3f, 1f);
        m_rigidbody.velocity = UNspeed;

        if (PhraseRecognitionSystem.isSupported) m_KeywordRecognizer.Stop();

        Invoke("GameEnd", 3f);
    }

    public void ShowT_O()
    {
        GetComponent<TextMesh>().text = "Time Out";
        GetComponent<TextMesh>().fontSize = 85;
        GetComponent<MeshRenderer>().sortingOrder = 2;
        transform.position = new Vector3(GameObject.Find("Main Camera").gameObject.transform.position.x, -3f, 1f);
        m_rigidbody.velocity = UNspeed;

        if (PhraseRecognitionSystem.isSupported) m_KeywordRecognizer.Stop();

        Invoke("GameEnd", 3f);
    }

    void GameEnd()
    {
        SceneManager.LoadScene("TrainEnd");
    }

}