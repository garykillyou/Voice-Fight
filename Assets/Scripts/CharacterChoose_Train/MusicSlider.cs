using UnityEngine;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour
{

    private AudioSource music;
    // Use this for initialization
    void Start()
    {
        music = GameObject.FindGameObjectWithTag("BackgroundSong").GetComponent<AudioSource>();
        //GetComponent<Slider>().value = music.volume;
        GetComponent<Slider>().onValueChanged.AddListener(musicChanged);
        
    }

    public void musicChanged(float change)
    {
        music.volume = change;
    }
}
