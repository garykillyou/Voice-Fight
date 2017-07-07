using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollView_PVP : MonoBehaviour, IEndDragHandler, IDragHandler
{
    private Scrollbar bar;
    private float maxMove;
    private float final;
    private float prefinal = 0f;

    public GameObject movie1;
    public GameObject movie2;
    public GameObject movie3;

    // Use this for initialization
    void Start()
    {
        bar = GetComponentInChildren<Scrollbar>();
        maxMove = 0.005f;
        Destroy(GameObject.FindGameObjectWithTag("BackgroundSong"));
        movie1.GetComponent<movie_PVP>().movieSource.Play();
        movie1.GetComponent<movie_PVP>().audioSource.Play();
    }

    public void OnDrag(PointerEventData data)
    {
        GameObject.Find("Arena_PVP").GetComponent<Arena_PVP>().CmdSVDrag(bar.value);

    }

    //Do this when the user stops dragging this UI Element.
    public void OnEndDrag(PointerEventData data)
    {


        if (bar.value < 0.25f)
        {
            GameObject.Find("Arena_PVP").GetComponent<Arena_PVP>().CmdSVEndDrag(0f);
        }
        else if (bar.value >= 0.25f && bar.value < 0.75f)
        {
            GameObject.Find("Arena_PVP").GetComponent<Arena_PVP>().CmdSVEndDrag(0.5f);
        }
        else if (bar.value >= 0.75f)
        {
            GameObject.Find("Arena_PVP").GetComponent<Arena_PVP>().CmdSVEndDrag(1f);
        }

        //if (GetComponentInParent<Canvas_PVP> ().isLocalPlayer) IsEndDrag();

    }

    public void IsDrag(float barValue)
    {
        bar.value = barValue;
    }

    public void IsEndDrag(float V)
    {
        if (bar.value < 0.25f)
        {
            final = V;
            InvokeRepeating("smoothMoveL", 0.0f, 0.01f);
            Debug.Log("1");
        }
        else if (bar.value >= 0.25f && bar.value < 0.75f)
        {
            final = V;
            if (bar.value < 0.5f)
                InvokeRepeating("smoothMoveR", 0.0f, 0.01f);
            else
                InvokeRepeating("smoothMoveL", 0.0f, 0.01f);
            Debug.Log("2");
        }
        else if (bar.value >= 0.75f)
        {
            final = V;
            InvokeRepeating("smoothMoveR", 0.0f, 0.01f);
            Debug.Log("3");
        }

        Debug.Log("Stopped dragging " + this.name + "!");
    }

    private void smoothMoveL()
    {
        if (bar.value > final)
        {
            /*Debug.Log (bar.value);
			Debug.Log (final);*/
            bar.value -= maxMove;
        }
        else
        {

            bar.value = final;
            /*MV = GetComponentsInChildren<MovieTexture> ();
			foreach (MovieTexture joint in MV)
				joint.Stop ();
			AV = GetComponentsInChildren<AudioSource> ();
			foreach (AudioSource joint in AV)
				joint.Stop ();*/
            if (prefinal != final)
            {
                movie1.GetComponent<movie_PVP>().movieSource.Stop();
                movie1.GetComponent<movie_PVP>().audioSource.Stop();

                movie2.GetComponent<movie_PVP>().movieSource.Stop();
                movie2.GetComponent<movie_PVP>().audioSource.Stop();

                movie3.GetComponent<movie_PVP>().movieSource.Stop();
                movie3.GetComponent<movie_PVP>().audioSource.Stop();

                if (bar.value == 0f)
                {
                    movie1.GetComponent<movie_PVP>().movieSource.Play();
                    movie1.GetComponent<movie_PVP>().audioSource.Play();
                }
                else if (bar.value == 0.5f)
                {
                    movie2.GetComponent<movie_PVP>().movieSource.Play();
                    movie2.GetComponent<movie_PVP>().audioSource.Play();
                }
                else if (bar.value == 1f)
                {
                    movie3.GetComponent<movie_PVP>().movieSource.Play();
                    movie3.GetComponent<movie_PVP>().audioSource.Play();
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
            /*Debug.Log (bar.value);
			Debug.Log (final);*/
            bar.value += maxMove;
        }
        else
        {

            bar.value = final;
            if (prefinal != final)
            {
                movie1.GetComponent<movie_PVP>().movieSource.Stop();
                movie1.GetComponent<movie_PVP>().audioSource.Stop();

                movie2.GetComponent<movie_PVP>().movieSource.Stop();
                movie2.GetComponent<movie_PVP>().audioSource.Stop();

                movie3.GetComponent<movie_PVP>().movieSource.Stop();
                movie3.GetComponent<movie_PVP>().audioSource.Stop();

                if (bar.value == 0f)
                {
                    movie1.GetComponent<movie_PVP>().movieSource.Play();
                    movie1.GetComponent<movie_PVP>().audioSource.Play();
                }
                else if (bar.value == 0.5f)
                {
                    movie2.GetComponent<movie_PVP>().movieSource.Play();
                    movie2.GetComponent<movie_PVP>().audioSource.Play();
                }
                else if (bar.value == 1f)
                {
                    movie3.GetComponent<movie_PVP>().movieSource.Play();
                    movie3.GetComponent<movie_PVP>().audioSource.Play();
                }
            }
            prefinal = final;
            CancelInvoke();
        }
    }
}