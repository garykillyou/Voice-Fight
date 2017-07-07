using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public GameObject P1CharacterImage;
    public GameObject P2CharacterImage;

    public int Num;
    public bool clicked = false;
    public string ImagePathL;
    public string ImagePathR;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!clicked)
            GetComponent<Image>().color = new Color(0, 0, 0, 1);
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            P1CharacterImage.GetComponentInChildren<Text>().color = Color.clear;
            P1CharacterImage.GetComponent<RawImage>().texture = Resources.Load(ImagePathL) as Texture;
            clicked = true;
            PlayerPrefs.SetInt("P1Num", Num);
            GameObject.Find("SureButton").GetComponent<SureButton>().P1IsReady = true;
        }
        else if (data.button == PointerEventData.InputButton.Right)
        {
            P2CharacterImage.GetComponentInChildren<Text>().color = Color.clear;
            P2CharacterImage.GetComponent<RawImage>().texture = Resources.Load(ImagePathR) as Texture;
            clicked = true;
            PlayerPrefs.SetInt("P2Num", Num);
            GameObject.Find("SureButton").GetComponent<SureButton>().P2IsReady = true;
        }
    }

}