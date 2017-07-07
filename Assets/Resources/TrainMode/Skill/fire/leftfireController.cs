using UnityEngine;
using System.Collections;

public class leftfireController : MonoBehaviour {

    Animator playerAnimator = new Animator();
    public static AnimatorStateInfo animatorInfo;
    public bool start = true;
    public bool explore = false;



    // Use this for initialization
    void Start ()
    {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "wall" || col.tag == "skill")
        {
            //Destroy(gameObject);
            start = false;
            explore = true;
        }
        else if (col.tag == "player1" || col.tag == "player2")
        {
            //Destroy(gameObject);
            start = false;
            explore = true;
        }
    }
    // Update is called once per frame
    void Update ()
    {
        playerAnimator = (Animator)GetComponent("Animator");
        animatorInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);//抓取現在再跑的動畫名
        if (start)
        {
            playerAnimator.Play("fire");
            gameObject.transform.position += new Vector3(-2f, 0, 0);
        }
        else if (explore)
        {
            playerAnimator.Play("explore");
        }
        if (animatorInfo.IsName("end")) Destroy(gameObject);
    }
}
