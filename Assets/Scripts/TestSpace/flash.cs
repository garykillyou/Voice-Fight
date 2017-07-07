using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class flash : MonoBehaviour
{

    Text gg;

    // Use this for initialization
    void Start()
    {
        gg = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        gg.color = Color.Lerp(Color.black, Color.clear, Mathf.PingPong(Time.time, 1f));
    }
}