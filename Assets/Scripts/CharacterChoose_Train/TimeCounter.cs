using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TimeCounter : MonoBehaviour
{


    public int time;

    // Use this for initialization
    void Start()
    {
        
        //InvokeRepeating ("DownCounter", 1.0f, 1.0f); 
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DownCounter()
    {
        if (time > 0)
        {
            time--;
            GetComponent<Text>().text = time + "";
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
