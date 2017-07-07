using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class MovieWall : MonoBehaviour
{

    public MovieTexture movieSource;
    private AudioSource audioSource;

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("BackgroundSong") != null)
        {
            Destroy(GameObject.FindGameObjectWithTag("BackgroundSong"));
        }

        /*if (PlayerPrefs.GetInt("ArenaNum") == 1)
        {
            movieSource = Resources.Load("TrainMode/movie1") as MovieTexture;
            //GetComponent<RawImage>().enabled = true;
            print("Arena is 1");
        }
        else if (PlayerPrefs.GetInt("ArenaNum") == 2)
        {
            movieSource = Resources.Load("TrainMode/movie2") as MovieTexture;
            //GetComponent<RawImage>().enabled = true;
            print("Arena is 2");
        }
        else if (PlayerPrefs.GetInt("ArenaNum") == 3)
        {
            movieSource = Resources.Load("TrainMode/movie3") as MovieTexture;
            //GetComponent<RawImage>().enabled = true;
            print("Arena is 3");
        }
        else
        {
            movieSource = Resources.Load("TrainMode/movie3") as MovieTexture;
            //GetComponent<RawImage>().enabled = true;
            print("Arena is 0");
        }*/

        print("Arena is " + PlayerPrefs.GetInt("ArenaNum") );

        GetComponent<RawImage>().texture = movieSource;
        movieSource.loop = true;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = movieSource.audioClip;

    }

    public void Play()
    {
        movieSource.Stop();
        audioSource.Stop();
        movieSource.Play();
        audioSource.Play();
    }
}