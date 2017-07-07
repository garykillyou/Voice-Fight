using UnityEngine;
using SmartLocalization;

public class ScreenButton : MonoBehaviour
{

    public void ChangeScreenResolution(int W)
    {
        if (W == 1920) Screen.SetResolution(1920, 1080, false);
        else if (W == 1600) Screen.SetResolution(1600, 900, false);
        else if (W == 1024) Screen.SetResolution(1024, 576, false);
    }

    public void m_ChangeLanguage(string language)
    {
        PlayerPrefs.SetString("lang", language);
        LanguageManager.Instance.ChangeLanguage(language);
    }

    public void DisableOptionCanvas()
    {
        GetComponentInParent<Canvas>().enabled = false;
    }
}