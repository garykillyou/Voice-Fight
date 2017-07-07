using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollView_Train : MonoBehaviour, IEndDragHandler
{
    private Scrollbar bar;
    public float moveSpeed = 0.005f;
    private float final;
    private float prefinal;

    public GameObject movie1;
    public GameObject movie2;
    public GameObject movie3;

    // Use this for initialization
    void Start()
    {
        bar = GetComponentInChildren<Scrollbar>();

        Destroy(GameObject.FindGameObjectWithTag("BackgroundSong"));

        movie1.GetComponent<movie_Train>().movieSource.Play();
        movie1.GetComponent<movie_Train>().audioSource.Play();
    }

    //Do this when the user stops dragging this UI Element.
    public void OnEndDrag(PointerEventData data)
    {
        if (bar.value >= 0f && bar.value < 0.25f)
        {
            final = 0f;
            InvokeRepeating("smoothMoveL", 0f, 0.01f);
            Debug.Log("1");
        }
        else if (bar.value >= 0.25f && bar.value < 0.75f)
        {
            final = 0.5f;
            if (bar.value < 0.5f)
                InvokeRepeating("smoothMoveR", 0f, 0.01f);
            else
                InvokeRepeating("smoothMoveL", 0f, 0.01f);
            Debug.Log("2");
        }
        else
        {
            final = 1f;
            InvokeRepeating("smoothMoveR", 0f, 0.01f);
            Debug.Log("3");
        }

        Debug.Log("Stopped dragging " + this.name + "!");
    }

    private void smoothMoveL()
    {
        if (bar.value > final)
        {
            /*Debug.Log(bar.value);
            Debug.Log(final);*/
            bar.value -= moveSpeed;
        }
        else
        {
            bar.value = final;

            if (prefinal != final)
            {

                if (final == 0f)
                {
                    movie2.GetComponent<movie_Train>().movieSource.Stop();
                    movie2.GetComponent<movie_Train>().audioSource.Stop();

                    movie1.GetComponent<movie_Train>().movieSource.Play();
                    movie1.GetComponent<movie_Train>().audioSource.Play();
                }
                else if (final == 0.5f)
                {
                    movie3.GetComponent<movie_Train>().movieSource.Stop();
                    movie3.GetComponent<movie_Train>().audioSource.Stop();

                    movie2.GetComponent<movie_Train>().movieSource.Play();
                    movie2.GetComponent<movie_Train>().audioSource.Play();
                }
            }

            prefinal = final;

            CancelInvoke();
        }
    }

    private void smoothMoveR()
    {
        if (bar.value < final)
        {
            /*Debug.Log(bar.value);
            Debug.Log(final);*/
            bar.value += moveSpeed;
        }
        else
        {
            bar.value = final;

            if (prefinal != final)
            {

                if (final == 0.5f)
                {
                    movie1.GetComponent<movie_Train>().movieSource.Stop();
                    movie1.GetComponent<movie_Train>().audioSource.Stop();

                    movie2.GetComponent<movie_Train>().movieSource.Play();
                    movie2.GetComponent<movie_Train>().audioSource.Play();
                }
                else if (final == 1f)
                {
                    movie2.GetComponent<movie_Train>().movieSource.Stop();
                    movie2.GetComponent<movie_Train>().audioSource.Stop();

                    movie3.GetComponent<movie_Train>().movieSource.Play();
                    movie3.GetComponent<movie_Train>().audioSource.Play();
                }
            }

            prefinal = final;

            CancelInvoke();
        }
    }
}
