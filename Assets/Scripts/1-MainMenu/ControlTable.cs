using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlTable : MonoBehaviour
{

    public GameObject Up1;
    public GameObject Down1;
    public GameObject Left1;
    public GameObject Right1;
    public GameObject A1;
    public GameObject A2;
    public GameObject A3;

    public GameObject Up2;
    public GameObject Down2;
    public GameObject Left2;
    public GameObject Right2;
    public GameObject B1;
    public GameObject B2;
    public GameObject B3;

    // Update is called once per frame
    void Update()
    {
        // P1
        if (Input.GetKeyDown(KeyCode.A))
        {
            Left1.GetComponent<Image>().color = Color.blue;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            Left1.GetComponent<Image>().color = Color.black;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Down1.GetComponent<Image>().color = Color.blue;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            Down1.GetComponent<Image>().color = Color.black;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Right1.GetComponent<Image>().color = Color.blue;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            Right1.GetComponent<Image>().color = Color.black;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Up1.GetComponent<Image>().color = Color.blue;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            Up1.GetComponent<Image>().color = Color.black;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            A1.GetComponent<Image>().color = Color.blue;
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            A1.GetComponent<Image>().color = Color.black;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            A2.GetComponent<Image>().color = Color.blue;
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            A2.GetComponent<Image>().color = Color.black;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            A3.GetComponent<Image>().color = Color.blue;
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            A3.GetComponent<Image>().color = Color.black;
        }

        //P2
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Left2.GetComponent<Image>().color = Color.blue;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Left2.GetComponent<Image>().color = Color.black;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Down2.GetComponent<Image>().color = Color.blue;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            Down2.GetComponent<Image>().color = Color.black;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Right2.GetComponent<Image>().color = Color.blue;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            Right2.GetComponent<Image>().color = Color.black;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Up2.GetComponent<Image>().color = Color.blue;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            Up2.GetComponent<Image>().color = Color.black;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            B1.GetComponent<Image>().color = Color.blue;
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            B1.GetComponent<Image>().color = Color.black;
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            B2.GetComponent<Image>().color = Color.blue;
        }
        if (Input.GetKeyUp(KeyCode.LeftBracket))
        {
            B2.GetComponent<Image>().color = Color.black;
        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            B3.GetComponent<Image>().color = Color.blue;
        }
        if (Input.GetKeyUp(KeyCode.RightBracket))
        {
            B3.GetComponent<Image>().color = Color.black;
        }
    }
}