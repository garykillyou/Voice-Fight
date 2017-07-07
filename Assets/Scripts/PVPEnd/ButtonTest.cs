using UnityEngine;
using SmartLocalization;
using SmartLocalization.Editor;
using System.Collections;

public class ButtonTest : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        LanguageManager.Instance.ChangeLanguage("zh-TW");
    }

    public void OnClick1()
    {
        LanguageManager.Instance.ChangeLanguage("en");
    }
    
    public void changeText(string localizedKey)
    {
        GameObject.Find("ResultText").GetComponent<LocalizedText>().localizedKey = localizedKey;
        LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture.languageCode);
        //GameObject.Find("ResulrText").GetComponent<Text>().text = LanguageManager.Instance.GetTextValue(localizedKey);
    }

    public void P1Win()
    {
        GameObject.Find("P1ResultText1").GetComponent<LocalizedText>().localizedKey = "Win";
        GameObject.Find("P2ResultText1").GetComponent<LocalizedText>().localizedKey = "Lose";
        LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture.languageCode);
    }

    public void P2Win()
    {
        GameObject.Find("P2ResultText1").GetComponent<LocalizedText>().localizedKey = "Win";
        GameObject.Find("P1ResultText1").GetComponent<LocalizedText>().localizedKey = "Lose";
        LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture.languageCode);
    }
}
