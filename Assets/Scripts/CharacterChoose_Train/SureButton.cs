using UnityEngine;
using UnityEngine.SceneManagement;

public class SureButton : MonoBehaviour
{
    public bool P1IsReady = false;
    public bool P2IsReady = false;

    public void OnClick()
    {

        if (P1IsReady && P2IsReady)
            SceneManager.LoadScene("ArenaChoose_Train");
    }
}