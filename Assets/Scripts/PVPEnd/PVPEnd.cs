using SmartLocalization;
using SmartLocalization.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PVPEnd : NetworkBehaviour
{
    [SyncVar(hook = "OnPlayAgain")]
    public int PlayAgain = 0;

    public GameObject PlayerText;
    public GameObject ResultText;
    public GameObject MidImage;
    public GameObject ButtonOneMore;
    public GameObject ButtonExit;

    private int who;
    private GameObject NetworkMM;

    // Use this for initialization
    void Start()
    {
        NetworkMM = GameObject.Find("NetworkManager");

        who = NetworkMM.GetComponent<NetworkManager_My>().Player;

        if (who == 1)
        {
            GameObject.FindGameObjectWithTag("player1Cursor").GetComponent<MouseTo_PVP_Host>().SetVisable(true);

            PlayerText.GetComponent<LocalizedText>().localizedKey = "Player1";
            if (PlayerPrefs.GetInt("WhoWin") == 1)
            {
                ResultText.GetComponent<LocalizedText>().localizedKey = "Win";
            }
            else if (PlayerPrefs.GetInt("WhoWin") == 2)
            {
                ResultText.GetComponent<LocalizedText>().localizedKey = "Lose";
            }
            LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture.languageCode);

        }
        else if (who == 2)
        {
            GameObject.FindGameObjectWithTag("player2Cursor").GetComponent<MouseTo_PVP_Client>().SetVisable(true);

            PlayerText.GetComponent<LocalizedText>().localizedKey = "Player2";
            if (PlayerPrefs.GetInt("WhoWin") == 1)
            {
                ResultText.GetComponent<LocalizedText>().localizedKey = "Lose";
            }
            else if (PlayerPrefs.GetInt("WhoWin") == 2)
            {
                ResultText.GetComponent<LocalizedText>().localizedKey = "Win";
            }
            LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture.languageCode);
        }

        Invoke("CallStartUp", 0.5f);
    }

    void CallStartUp()
    {
        MidImage.GetComponent<MidImage_PVP>().StartUp();
    }

    public void P1Disconnect()
    {
        if (who == 1)
        {
            NetworkMM.GetComponent<NetworkManager_Custom>().DisconnectHostExit();
        }
        else if (who == 2)
        {
            ButtonOneMore.GetComponent<Button>().interactable = false;
            ButtonOneMore.GetComponentInChildren<Text>().text = "對手離開了";
            NetworkMM.GetComponent<NetworkManager_Custom>().DisconnectClientExit_PVPEnd();
        }
    }

    public void OtherDisconnect()
    {
        ButtonOneMore.GetComponent<Button>().interactable = false;
        ButtonOneMore.GetComponentInChildren<LocalizedText>().localizedKey = "Enemy left";
        LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture.languageCode);
    }

    public void WantPlayAgain()
    {
        ButtonOneMore.GetComponent<Button>().interactable = false;
        ButtonOneMore.GetComponentInChildren<LocalizedText>().localizedKey = "Wait Sure";
        LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture.languageCode);
        if (who == 1)
        {
            GameObject.FindGameObjectWithTag("player1Cursor").GetComponent<MouseTo_PVP_Host>().TellServerP1WantToPlayAgain();
        }
        else if (who == 2)
        {
            GameObject.FindGameObjectWithTag("player2Cursor").GetComponent<MouseTo_PVP_Client>().TellServerP2WantToPlayAgain();
        }
    }

    void OnPlayAgain(int PA)
    {
        ButtonOneMore.GetComponent<Button>().onClick.RemoveAllListeners();

        if (PA == 1 & who == 2)
        {
            ButtonOneMore.GetComponentInChildren<LocalizedText>().localizedKey = "Enemy want again";
        }
        else if (PA == 2 & who == 1)
        {
            ButtonOneMore.GetComponentInChildren<LocalizedText>().localizedKey = "Enemy want again";
        }

        LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture.languageCode);

        ButtonOneMore.GetComponent<Button>().onClick.AddListener(TellSurePlayAgain);

        GameObject.FindGameObjectWithTag("player1Cursor").GetComponent<MouseTo_PVP_Host>().imagePath = 0;
        GameObject.FindGameObjectWithTag("player1Cursor").GetComponent<MouseTo_PVP_Host>().determineP1 = false;
        GameObject.FindGameObjectWithTag("player2Cursor").GetComponent<MouseTo_PVP_Client>().imagePath = 0;
        GameObject.FindGameObjectWithTag("player2Cursor").GetComponent<MouseTo_PVP_Client>().determineP2 = false;
    }

    void TellSurePlayAgain()
    {
        GameObject.FindGameObjectWithTag("player1Cursor").GetComponent<MouseTo_PVP_Host>().TellServerChangeScene("CharacterChoose_PVP");
        GameObject.FindGameObjectWithTag("player2Cursor").GetComponent<MouseTo_PVP_Client>().TellServerChangeScene("CharacterChoose_PVP");
    }

    public void Exit()
    {

        print("Player" + who + "Disconnect");

        if (who == 1) NetworkMM.GetComponent<NetworkManager_My>().DisconnectHostExit();
        else if (who == 2) NetworkMM.GetComponent<NetworkManager_My>().DisconnectClientExit();

    }
}