using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeCounter_PVP : MonoBehaviour
{

    public int time;
    public Text TimeText;

    // Use this for initialization
    void Start()
    {
        TimeText.text = time.ToString();
        //InvokeRepeating ("DownCounter", 0.0f, 1.0f); 
    }

    void DownCounter()
    {
        if (time > 0)
        {
            time--;
            TimeText.text = time.ToString();
        }
        else
        {
            if (GameObject.Find("P1CharacterImage").GetComponent<RawImage>().texture != null &&
                GameObject.Find("P2CharacterImage").GetComponent<RawImage>().texture != null)
                SceneManager.LoadScene(2);
            else
            {
                SceneManager.LoadScene(0);
            }

        }
    }
}