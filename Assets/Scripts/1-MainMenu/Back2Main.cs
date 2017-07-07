using UnityEngine;
using UnityEngine.SceneManagement;

public class Back2Main : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("1-MainMenu");
        }
    }
}