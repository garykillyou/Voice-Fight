using UnityEngine;
using UnityEngine.UI;

public class MidImage : MonoBehaviour
{
    public int who;
    public float final;
    public float speed;
    private bool gogo;
    private Image mine;

    // Use this for initialization
    void Start()
    {
        gogo = false;
        mine = GetComponent<Image>();
        if (who == 1) final = (float)PlayerPrefs.GetInt("P1AtkNum") / 100;
        else if (who == 2) final = (float)PlayerPrefs.GetInt("P2AtkNum") / 100;
        else if (who == 3) StartUp();
        Debug.Log(PlayerPrefs.GetInt("P1AtkNum"));
        Debug.Log(PlayerPrefs.GetInt("P2AtkNum"));
    }

    // Update is called once per frame
    void Update()
    {
        if (mine.fillAmount >= final)
        {
            gogo = false;
            mine.fillAmount = final;
        }

        if (gogo)
        {
            mine.fillAmount += 1f * Time.deltaTime / speed;

        }
    }

    public void StartUp()
    {
        gogo = true;
    }
}