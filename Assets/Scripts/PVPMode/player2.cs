using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class player2 : NetworkBehaviour
{
	public Sprite[] playerSprite;

	public GameObject player1;
	public GameObject bools;
	public bool block = true;
    
	public float force = 250f;
	public float moveforce = 8f;
	public float maxspeed = 3f;
	Rigidbody2D GGrigidbody2D;

	public bool jump = true;

	Animator playerAnimator = new Animator ();
	public static AnimatorStateInfo animatorInfo;


	private GameObject P2HealthText;

	[SyncVar (hook = "OnGetDmg")] private int P2health = 100;

	// Use this for initialization
	void Awake ()
	{
		GGrigidbody2D = GetComponent<Rigidbody2D> ();
	}

	void Start ()
	{
		player1 = GameObject.FindGameObjectWithTag ("player1");

		P2HealthText = GameObject.Find ("P2Blood");
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "down") {
			//Gamefunction.Instance.AddScore();
			jump = true;
		}
	}



	// Update is called once per frame
	void Update ()
	{
		if (isLocalPlayer) {
			//Sprite[] playerSprite = Resources.LoadAll("allpicture");////(1)
			//SpriteRenderer playerSpriteRenderer = (SpriteRenderer)GetComponent("SpriteRenderer");
			playerAnimator = (Animator)GetComponent ("Animator");
			animatorInfo = playerAnimator.GetCurrentAnimatorStateInfo (0);//抓取現在再跑的動畫名

			if (block) {
				if (Input.GetKey (KeyCode.P)) {
					GGrigidbody2D.velocity = new Vector2 (0, 0);
					playerAnimator.Play ("hit2");
					block = false;
					CmdHitPlayer1 ();
				} else if (Input.GetKey (KeyCode.DownArrow) && jump) {
					GGrigidbody2D.velocity = new Vector2 (0, 0);
					playerAnimator.Play ("down2");
					//playerSpriteRenderer.sprite = playerSprite[7];////(3)
				} else if (Input.GetKey (KeyCode.LeftArrow)) {

					GGrigidbody2D.AddForce (transform.right * moveforce * -1);
                

					if (Input.GetKeyDown (KeyCode.UpArrow)) {
						playerAnimator.Play ("jump2");
						if (jump) {
							GGrigidbody2D.AddForce (transform.up * force);
							jump = false;
						}
					} else if (Input.GetKey (KeyCode.RightArrow)) { //重製雙按鍵時會有不自然平滑移動
						GGrigidbody2D.velocity = new Vector2 (0, 0);
						GGrigidbody2D.AddForce (transform.right * moveforce * 1);
					} else if (!jump)
						playerAnimator.Play ("jump2");
					else
						playerAnimator.Play ("walk2");
					//playerSpriteRenderer.sprite = playerSprite[11];
				} else if (Input.GetKey (KeyCode.RightArrow)) {

					GGrigidbody2D.AddForce (transform.right * moveforce);

					if (Input.GetKeyDown (KeyCode.UpArrow)) {
						playerAnimator.Play ("jump2");
						if (jump) {
							GGrigidbody2D.AddForce (transform.up * force);
							jump = false;
						}
					} else if (Input.GetKey (KeyCode.LeftArrow)) { //重製雙按鍵時會有不自然平滑移動
						GGrigidbody2D.velocity = new Vector2 (0, 0);
						GGrigidbody2D.AddForce (transform.right * moveforce * -1);
					} else if (!jump)
						playerAnimator.Play ("jump2");
					else
						playerAnimator.Play ("walk2");
					//playerSpriteRenderer.sprite = playerSprite[11];
				} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
					playerAnimator.Play ("jump2");
					if (jump) {
						GGrigidbody2D.AddForce (transform.up * force);
						jump = false;
					}
					//playerSpriteRenderer.sprite = playerSprite[3];////(3)
				} else if (Input.GetKeyUp (KeyCode.DownArrow)) {
					playerAnimator.Play ("wait2");
					//playerSpriteRenderer.sprite = playerSprite[7];////(3)
				} else if (Input.GetKeyUp (KeyCode.RightArrow) || Input.GetKeyUp (KeyCode.LeftArrow)) {

					if (jump)
						GGrigidbody2D.velocity = new Vector2 (0, 0);
				} else if (jump) { //在地板   放置動作
					playerAnimator.Play ("wait2");
				}



				//**********************************************************************************
				if (Mathf.Abs (GGrigidbody2D.velocity.x) > maxspeed)
					GGrigidbody2D.velocity = new Vector2 (Mathf.Sign (GGrigidbody2D.velocity.x) * maxspeed, GGrigidbody2D.velocity.y);
				//**********************************************************************************


				/*if (Input.GetKeyDown (KeyCode.Space)) {
					Vector3 pos1 = gameObject.transform.position;//+ new Vector3(-1f, 0, 0)
					Vector3 pos = player1.transform.position + new Vector3 (0.7f, 0, 0);
					if (pos.x < pos1.x + 1f && pos.x > pos1.x - 1f) {
						playerAnimator.Play ("hited2");
						block = false;
						Gamefunction.Instance.Score2 ();
					} else
						Instantiate (bools, pos, player1.transform.rotation);


					//Instantiate(bools, pos1, gameObject.transform.rotation);
				}*/


				//**********************************************************************************
			} else {

				if (animatorInfo.IsName ("wait2"))
					block = true;
			}
		}
	}

	public void isHit ()
	{
		
		playerAnimator = (Animator)GetComponent ("Animator");

		Vector3 pos1 = gameObject.transform.position;//+ new Vector3(-1f, 0, 0)
		Vector3 pos = player1.transform.position + new Vector3 (0.7f, 0, 0);
		if (pos.x < pos1.x + 1f && pos.x > pos1.x - 1f) {
			playerAnimator.Play ("hited2");
			Debug.Log ("hit by 1");
			block = false;
			CmdTellServerWhoDmg ();
			//Gamefunction.Instance.Score2 ();
		} else
			Instantiate (bools, pos, player1.transform.rotation);
	}

	public void GetDmg ()
	{
		P2health -= 10;

	}

	void OnGetDmg (int dmg)
	{
		P2health = dmg;
		P2HealthText.GetComponent<BloodSlider> ().HP = P2health;
	}

	[Command]
	void CmdHitPlayer1 ()
	{
		GameObject go = GameObject.FindGameObjectWithTag ("player1");
		go.GetComponent<player> ().isHit ();
	}

	[Command]
	void CmdTellServerWhoDmg ()
	{
		GameObject go = GameObject.FindGameObjectWithTag ("player2");
		go.GetComponent<player2> ().GetDmg ();
	}

}
