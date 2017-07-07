using UnityEngine;
using UnityEngine.Networking;
using SmartLocalization;

public class MouseTo : MonoBehaviour
{

    private Vector3 mouseP;
    public GameObject content;

    // Use this for initialization
    void Start()
    {
        if (PlayerPrefs.GetString("lang") == "zh-TW")
        {
            LanguageManager.Instance.ChangeLanguage("zh-TW");
        }
        else
        {
            LanguageManager.Instance.ChangeLanguage("en");
        }

        Cursor.visible = false;

        if (GameObject.Find("NetworkManager") != null)
        {
            Destroy(GameObject.Find("NetworkManager"));
            NetworkManager.Shutdown();
        }

        if (GameObject.FindGameObjectWithTag("player1Cursor") != null)
            Destroy(GameObject.FindGameObjectWithTag("player1Cursor"));

        if (GameObject.FindGameObjectWithTag("player2Cursor") != null)
            Destroy(GameObject.FindGameObjectWithTag("player2Cursor"));

        GameObject[] cursorInstant;
        cursorInstant = GameObject.FindGameObjectsWithTag("Cursor");
        if (cursorInstant.Length > 1) Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        mouseP = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100f));


        if (Input.GetMouseButtonDown(0))
        {
            /*Debug.Log (Input.mousePosition);
			Debug.Log (mouseP);*/
            content.gameObject.transform.rotation = Quaternion.Euler(0f, 180f, -10f);
        }

        if (Input.GetMouseButtonUp(0))
        {
            /*Debug.Log (Input.mousePosition);
			Debug.Log (mouseP);*/
            content.gameObject.transform.rotation = Quaternion.Euler(0f, 180f, -0f);
        }


        /*if (Input.GetMouseButton(0))
        {
            /*Debug.Log (Input.mousePosition);
			Debug.Log (mouseP);

        }*/
        /*
        if (Input.GetMouseButtonDown (1)) {
			Cursor.visible = true;
		}*/

        this.transform.position = new Vector3(mouseP.x, mouseP.y);
    }

    public void SetVisable(bool V)
    {
        content.GetComponent<SpriteRenderer>().enabled = V;
    }
}
