using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunction : MonoBehaviour {

    public GameObject VoiceR;

    public void ChangeScene(string SceneName)
    {
        VoiceR.GetComponent<VoiceRecognition>().Stop();
        SceneManager.LoadScene(SceneName);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void OptionOnClick()
    {
        GameObject.Find("OptionCanvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("OptionCanvas").transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("OptionCanvas").transform.localPosition = new Vector3(0, 0);
    }
}
