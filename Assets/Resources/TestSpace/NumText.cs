using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NumText : MonoBehaviour
{
    public Image Mid;
    private Text Num;

    // Use this for initialization
    void Start()
    {
        Num = GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        Num.text = Mathf.Round((Mid.fillAmount * 100)).ToString();
    }
}