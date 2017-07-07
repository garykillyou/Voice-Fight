using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SureButton_Train : MonoBehaviour
{

    public Scrollbar bar;

    public void OnClick()
    {

        if (bar.value > -0.01f && bar.value < 0.01f)
        {
            PlayerPrefs.SetInt("ArenaNum", 1);
            Debug.Log("Arena is 1");
            SceneManager.LoadScene("TrainMode1");
        }
        else if (bar.value > 0.49f && bar.value < 0.51f)
        {
            PlayerPrefs.SetInt("ArenaNum", 2);
            Debug.Log("Arena is 2");
            SceneManager.LoadScene("TrainMode2");
        }
        else if (bar.value > 0.99f && bar.value < 1.01f)
        {
            PlayerPrefs.SetInt("ArenaNum", 3);
            Debug.Log("Arena is 3");
            SceneManager.LoadScene("TrainMode3");
        }
    }
}