using UnityEngine;
using SmartLocalization;
using SmartLocalization.Editor;
using UnityEngine.SceneManagement;

public class ResultShow : MonoBehaviour
{

    public GameObject P1result1;
    public GameObject P2result1;

    public GameObject MidImage1;
    public GameObject MidImage2;

    private int win;

    // Use this for initialization
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Cursor") != null) GameObject.FindGameObjectWithTag("Cursor").GetComponent<MouseTo>().SetVisable(true);

        //PlayerPrefs.SetInt("WhoWin", 1);
        win = PlayerPrefs.GetInt("WhoWin");

        if (win == 1)
        {
            P1result1.GetComponent<LocalizedText>().localizedKey = "Win";
            P2result1.GetComponent<LocalizedText>().localizedKey = "Lose";
            LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture.languageCode);
        }
        else if (win == 2)
        {
            P2result1.GetComponent<LocalizedText>().localizedKey = "Win";
            P1result1.GetComponent<LocalizedText>().localizedKey = "Lose";
            LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture.languageCode);
        }

        Invoke("CallStartUp", 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClickToMain()
    {
        SceneManager.LoadScene("1-MainMenu");
    }

    void CallStartUp()
    {
        MidImage1.GetComponent<MidImage>().StartUp();
        MidImage2.GetComponent<MidImage>().StartUp();
    }
}
