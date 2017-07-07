using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class player : NetworkBehaviour
{
	public Sprite[] playerSprite;

	public GameObject player2;
	public GameObject bools;
	public bool block = true;
    
	public float force = 250f;
	public float moveforce = 8f;
	public float maxspeed = 3f;
	Rigidbody2D GGrigidbody2D;

	public bool jump = true;

	Animator playerAnimator = new Animator ();
	public static AnimatorStateInfo animatorInfo;

	private GameObject P1HealthText;

	[SyncVar (hook = "OnGetDmg")] private int P1health = 100;


	// Use this for initialization
	void Awake ()
	{
		GGrigidbody2D = GetComponent<Rigidbody2D> ();
	}

	void Start ()
	{
		player2 = GameObject.FindGameObjectWithTag ("player2");
		P1HealthText = GameObject.Find ("P1Blood");
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "down") {
			jump = true;
		}
	}
    

	// Update is called once per frame
	void Update ()
	{
		//Sprite[] playerSprite = Resources.LoadAll("allpicture");////(1)
		//SpriteRenderer playerSpriteRenderer = (SpriteRenderer)GetComponent("SpriteRenderer");


		if (isLocalPlayer) {
			playerAnimator = (Animator)GetComponent ("Animator");
			animatorInfo = playerAnimator.GetCurrentAnimatorStateInfo (0);//抓取現在再跑的動畫名

			if (block) {
				if (Input.GetKeyDown (KeyCode.Space)) {
					block = false;
					GGrigidbody2D.velocity = new Vector2 (0, 0);
					playerAnimator.Play ("hit");
					CmdHitPlayer2 ();
				} else if (Input.GetKey (KeyCode.S) && jump) {
					GGrigidbody2D.velocity = new Vector2 (0, 0);
					playerAnimator.Play ("down");
					//playerSpriteRenderer.sprite = playerSprite[7];////(3)
				} else if (Input.GetKey (KeyCode.A)) {
					//rigidbody2D.velocity = new Vector2( 0,0 ) ;
					GGrigidbody2D.AddForce (transform.right * moveforce * -1);
					//rigidbody2D.isKinematic = true;
					//gameObject.transform.position += new Vector3(-0.1f, 0, 0);
					if (Input.GetKeyDown (KeyCode.W)) {
						playerAnimator.Play ("jump");
						if (jump) {
							GGrigidbody2D.AddForce (transform.up * force);
							jump = false;
						}
					} else if (Input.GetKey (KeyCode.D)) { //重製雙按鍵時會有不自然平滑移動
						GGrigidbody2D.velocity = new Vector2 (0, 0);
						GGrigidbody2D.AddForce (transform.right * moveforce * 1);
					} else if (!jump)
						playerAnimator.Play ("jump");
					else
						playerAnimator.Play ("walk");
					//playerSpriteRenderer.sprite = playerSprite[11];
				} else if (Input.GetKey (KeyCode.D)) {
					//rigidbody2D.velocity = new Vector2(0, 0);
					GGrigidbody2D.AddForce (transform.right * moveforce);
					//gameObject.transform.position += new Vector3(0.1f, 0, 0);
					if (Input.GetKeyDown (KeyCode.W)) {
						playerAnimator.Play ("jump");
						if (jump) {
							GGrigidbody2D.AddForce (transform.up * force);
							jump = false;
						}
					} else if (Input.GetKey (KeyCode.A)) { //重製雙按鍵時會有不自然平滑移動
						GGrigidbody2D.velocity = new Vector2 (0, 0);
						GGrigidbody2D.AddForce (transform.right * moveforce * -1);
					} else if (!jump)
						playerAnimator.Play ("jump");
                //else if (player2.Instance.animatorInfo.IsName("hit2")) playerAnimator.Play("down");
                else
						playerAnimator.Play ("walk");
					//playerSpriteRenderer.sprite = playerSprite[11];
				} else if (Input.GetKeyDown (KeyCode.W)) {
					playerAnimator.Play ("jump");
					if (jump) {
						GGrigidbody2D.AddForce (transform.up * force);
						jump = false;
					}
					//playerSpriteRenderer.sprite = playerSprite[3];////(3)
				} else if (Input.GetKeyUp (KeyCode.S)) {
					playerAnimator.Play ("wait");
					//playerSpriteRenderer.sprite = playerSprite[7];////(3)
				} else if (Input.GetKeyUp (KeyCode.A) || Input.GetKeyUp (KeyCode.D)) {
					if (jump)
						GGrigidbody2D.velocity = new Vector2 (0, 0);
				} else if (jump) { //在地板   放置動作
					playerAnimator.Play ("wait");
				}


				if (Input.GetKeyDown (KeyCode.I))
					block = false;

				//**********************************************************************************
				if (Mathf.Abs (GGrigidbody2D.velocity.x) > maxspeed)
					GGrigidbody2D.velocity = new Vector2 (Mathf.Sign (GGrigidbody2D.velocity.x) * maxspeed, GGrigidbody2D.velocity.y); 

				//**********************************************************************************
            

				/*if (Input.GetKeyDown (KeyCode.P)) {
					Vector3 pos1 = gameObject.transform.position;//+ new Vector3(-1f, 0, 0)
					Vector3 pos = player2.transform.position + new Vector3 (-0.7f, 0, 0);
					if (pos.x < pos1.x + 1f && pos.x > pos1.x - 1f) {
						playerAnimator.Play ("hited");
						block = false;
						Gamefunction.Instance.Score1 ();

					} else
						Instantiate (bools, pos, player2.transform.rotation);


					//Instantiate(bools, pos1, gameObject.transform.rotation);
				}*/


				//**********************************************************************************
			} else {
				if (animatorInfo.IsName ("wait"))
					block = true;
			}
		}
	}

	public void isHit ()
	{

		playerAnimator = (Animator)GetComponent ("Animator");

		Vector3 pos1 = gameObject.transform.position;//+ new Vector3(-1f, 0, 0)
		Vector3 pos = player2.transform.position + new Vector3 (-0.7f, 0, 0);
		if (pos.x < pos1.x + 1f && pos.x > pos1.x - 1f) {
			playerAnimator.Play ("hited");
			Debug.Log ("hit by 2");
			block = false;
			CmdTellServerWhoDmg ();
			//Gamefunction.Instance.Score1 ();

		} else
			Instantiate (bools, pos, player2.transform.rotation);
	}

	public void GetDmg ()
	{
		P1health -= 10;

	}

	void OnGetDmg (int dmg)
	{
		P1health = dmg;
		P1HealthText.GetComponent<BloodSlider> ().HP = P1health;
	}

	[Command]
	void CmdHitPlayer2 ()
	{
		GameObject go = GameObject.FindGameObjectWithTag ("player2");
		go.GetComponent<player2> ().isHit ();
	}

	[Command]
	void CmdTellServerWhoDmg ()
	{
		GameObject go = GameObject.FindGameObjectWithTag ("player1");
		go.GetComponent<player> ().GetDmg ();
	}
}
