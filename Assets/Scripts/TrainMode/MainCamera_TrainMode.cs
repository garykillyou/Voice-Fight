using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCamera_TrainMode : MonoBehaviour
{

    private GameObject P1;
    private GameObject P2;
    private bool start = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("1-MainMenu");
        }

        if (start && (P1.transform.position.x + P2.transform.position.x) / 2f >= -72 && (P1.transform.position.x + P2.transform.position.x) / 2f <= 72)
            transform.position = new Vector3((P1.transform.position.x + P2.transform.position.x) / 2f, 0, transform.position.z);
        
    }

    public void ConnectGameObjectAndStart()
    {
        P1 = GameObject.FindGameObjectWithTag("player1");
        P2 = GameObject.FindGameObjectWithTag("player2");
        start = true;
    }
}