using UnityEngine;
using UnityEngine.UI;

public class SureButton_PVP : MonoBehaviour
{

    public Scrollbar bar;

    public void OnClick()
    {
        GameObject.Find("Arena_PVP").GetComponent<Arena_PVP>().CmdSBClick();
    }

    public void IsClick()
    {
        CancelInvoke();

        if (bar.value >= -0.01f && bar.value <= 0.01f)
        {
            PlayerPrefs.SetInt("ArenaNum", 1);
            print("ArenaNum is 1");
            GameObject.Find("Arena_PVP").GetComponent<Arena_PVP>().CmdSBClickEnd("PVPMode1");
        }
        else if (bar.value >= 0.49f && bar.value <= 0.51f)
        {
            PlayerPrefs.SetInt("ArenaNum", 2);
            print("ArenaNum is 2");
            GameObject.Find("Arena_PVP").GetComponent<Arena_PVP>().CmdSBClickEnd("PVPMode2");
        }
        else if (bar.value >= 0.99f && bar.value <= 1.01f)
        {
            PlayerPrefs.SetInt("ArenaNum", 3);
            print("ArenaNum is 3");
            GameObject.Find("Arena_PVP").GetComponent<Arena_PVP>().CmdSBClickEnd("PVPMode3");
        }
    }
}