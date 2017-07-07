using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using SmartLocalization;
using SmartLocalization.Editor;

public class VoiceRecognition : MonoBehaviour
{

    KeywordRecognizer m_KeywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    
    // Use this for initialization
    void Start()
    {
        if (!PhraseRecognitionSystem.isSupported)
        {
            GetComponent<LocalizedText>().localizedKey = "VoiceUnsupport";
        }
        else
        {
            GetComponent<LocalizedText>().localizedKey = "VoiceSupport";

            keywords.Add("box", () =>
            {
                QQCalled();
            });

            keywords.Add("white", () =>
            {
                FireCalled();
            });

            keywords.Add("yes", () =>
            {
                GGCalled();
            });


            m_KeywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
            m_KeywordRecognizer.OnPhraseRecognized += KeywordRecognizerOnPhraseRecognized;
            m_KeywordRecognizer.Start();
        }
        LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture.languageCode);
    }

    void KeywordRecognizerOnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    public void Stop()
    {
        if(PhraseRecognitionSystem.isSupported) m_KeywordRecognizer.Stop();
    }

    void GoCalled()
    {
        print("You just said GO !!!");
        GetComponent<Text>().text = "GO!!!";
    }

    void JumpCalled()
    {
        print("You just said Jump !!!");
        GetComponent<Text>().text = "Jump!!!";
    }

    void FireCalled()
    {
        print("You just said Fire !!!");
        GetComponent<Text>().text = "Fire!!!";
    }

    void GGCalled()
    {
        print("You just said GG !!!");
        GetComponent<Text>().text = "GG!!!";
    }

    void QQCalled()
    {
        print("You just said QQ !!!");
        GetComponent<Text>().text = "QQ!!!";
    }
}