using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterButton_PVP : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public int Num;

    private bool clicked;

    // Use this for initialization
    void Start()
    {
        clicked = false;
    }

    public void onClick()
    {
        print("click~~~" + Num);

        GameObject.FindGameObjectWithTag("player1Cursor").GetComponent<MouseTo_PVP_Host>().Onclick(Num);
        GameObject.FindGameObjectWithTag("player2Cursor").GetComponent<MouseTo_PVP_Client>().Onclick(Num);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!clicked)
            GetComponent<Image>().color = new Color(0, 0, 0, 1);
    }
}