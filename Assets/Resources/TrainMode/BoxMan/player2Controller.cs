using UnityEngine;

public class player2Controller : MonoBehaviour
{

    public bool block = true;

    public GameObject bools;
    public GameObject rightskill;
    public GameObject leftskill;
    public float force = 10000;
    public float maxspeed = 40f;
    public float AttackDistance = 13f; // 攻擊距離
    Rigidbody2D GGrigidbody2D;

    public float time; //計時   擊倒時使用
    public float speedtime = 0; //衝刺計時   擊倒時使用
    public float jumptforce = 1000; //跳躍速度控制器
    public float downpeed = 60f;
    public int hittime = 0; // 攻擊次數
    public bool hiting = false; // 判斷是否連擊
    public bool jump = false;
    public bool jumpend = false; // 補丁   執行落地動作
    public bool down = false;
    public bool rightface = false;  //  是否面相右方向
    public bool skillhited = false; // 被技能丟中

    Animator playerAnimator = new Animator();
    public static AnimatorStateInfo animatorInfo;

    void Awake()
    {
        GGrigidbody2D = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        playerAnimator = (Animator)GetComponent("Animator");
        if (col.tag == "down")
        {
            GGrigidbody2D.velocity = new Vector2(0, 0); //落地時速率重製
            jump = true;
            block = false;
            jumpend = true;
        }
        else if (col.tag == "skill")
        {
            skillhited = true;
            if (block) GameFunction.Instance.p2hited = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        playerAnimator = (Animator)GetComponent("Animator");
        animatorInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);//抓取現在再跑的動畫名
        GameFunction.Instance.pos2 = gameObject.transform.position;// PLAYER 2

        if (GameFunction.Instance.pos1.x > GameFunction.Instance.pos2.x)
        {
            rightface = true;
        }
        else
        {
            rightface = false;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (rightface) rightface = false;
            else rightface = true;
        }

        if (GameFunction.Instance.IsPlaying && block)
        {
            time = 0; // 時間初始
            hittime = 0; // 連段初始
            GameFunction.Instance.p2up = false;
            if (rightface)
            {      //**********************************  面相右  ********************************************

                //GameFunction.Instance.pos2 = gameObject.transform.position + new Vector3(13f, 0, 0);// PLAYER 2

                if (Input.GetKey(KeyCode.DownArrow) && jump) //  ----------------------------------------------------------  S
                {
                    GGrigidbody2D.velocity = new Vector2(0, 0);
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        if (Input.GetKeyDown(KeyCode.RightBracket)) // ------------------------------------------------------  技能
                        {
                            GameFunction.Instance.p2hittime = GameFunction.Instance.p2hittime + 1;
                            GGrigidbody2D.velocity = new Vector2(0, 0);
                            Instantiate(rightskill, GameFunction.Instance.pos2 + new Vector3(20f, 0, 0), gameObject.transform.rotation);
                            playerAnimator.Play("skillR");
                            block = false;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftBracket))   //  蹲下攻擊
                    {
                        GameFunction.Instance.p2hittime = GameFunction.Instance.p2hittime + 1;
                        playerAnimator.Play("downhitR");
                        block = false;
                        GameFunction.Instance.p2hit = true;
                        if (GameFunction.Instance.pos2.x + AttackDistance > GameFunction.Instance.pos1.x - 10f &&
                            GameFunction.Instance.pos2.x + AttackDistance < GameFunction.Instance.pos1.x + 10f)
                        {
                            if (GameFunction.Instance.pos2.y > GameFunction.Instance.pos1.y - 20f &&
                                GameFunction.Instance.pos2.y < GameFunction.Instance.pos1.y + 20f)  //再次確認y軸  高度
                                GameFunction.Instance.p1hited = true;
                        }
                    }
                    else if (down)    //  執行蹲下"完成"動作   避免整個蹲下動作一直輪迴
                    {
                        playerAnimator.Play("downendR");
                    }
                    else
                    {      //執行蹲下"進行中"動畫
                        GGrigidbody2D.velocity = new Vector2(0, 0);
                        playerAnimator.Play("downR");
                        block = false;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.LeftBracket)) // -----------------------------------------------------  attack(含空中)
                {
                    //Instantiate(bools, GameFunction.Instance.pos2 + new Vector3(13f, 0, 0), gameObject.transform.rotation);
                    GameFunction.Instance.p2hittime = GameFunction.Instance.p2hittime + 1;
                    if (jump)   //地面攻擊
                    {
                        GGrigidbody2D.velocity = new Vector2(0, 0);
                        playerAnimator.Play("hitR");
                    }
                    else
                    {   //  空中攻擊
                        playerAnimator.Play("jumphitR");
                    }
                    block = false;
                    GameFunction.Instance.p2hit = true;  //  通知 gamefuntion p2 在攻擊
                    if (GameFunction.Instance.pos2.x + AttackDistance > GameFunction.Instance.pos1.x - 10f &&
                        GameFunction.Instance.pos2.x + AttackDistance < GameFunction.Instance.pos1.x + 10f)
                    {
                        if (GameFunction.Instance.pos2.y > GameFunction.Instance.pos1.y - 20f &&
                            GameFunction.Instance.pos2.y < GameFunction.Instance.pos1.y + 20f)  //再次確認y軸  高度
                            GameFunction.Instance.p1hited = true;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.RightBracket) && jump) // ------------------------------------------------------  重擊
                {
                    GameFunction.Instance.p2hittime = GameFunction.Instance.p2hittime + 1;
                    if (jump) GGrigidbody2D.velocity = new Vector2(0, 0);
                    GameFunction.Instance.p2hit = true;  //  通知 gamefuntion p2 在攻擊
                    playerAnimator.Play("hitendR");
                    block = false;
                    GameFunction.Instance.p2hit = true;  //  通知 gamefuntion p1 在攻擊
                    if (GameFunction.Instance.pos2.x + AttackDistance > GameFunction.Instance.pos1.x - 13f &&
                        GameFunction.Instance.pos2.x + AttackDistance < GameFunction.Instance.pos1.x + 13f)
                    {
                        if (GameFunction.Instance.pos2.y > GameFunction.Instance.pos1.y - 20f &&
                            GameFunction.Instance.pos2.y < GameFunction.Instance.pos1.y + 20f)
                        {
                            GameFunction.Instance.p1hitended = true;
                        }  //再次確認y軸  高度
                    }
                }
                else if (Input.GetKey(KeyCode.LeftArrow) && jump) //      ------------------------------------------------------------   A
                {
                    GGrigidbody2D.velocity = new Vector2(-1 * maxspeed, GGrigidbody2D.velocity.y);
                    if (Input.GetKeyDown(KeyCode.UpArrow)) // ----------------------------------  AW
                    {
                        if (jump)
                        {
                            playerAnimator.Play("jumpR");
                            GGrigidbody2D.AddForce(transform.up * force);
                            jump = false;
                        }
                    }
                    else if (Input.GetKey(KeyCode.RightArrow)) // ----------------------  移動換方向
                    { //重製雙按鍵時會有不自然平滑移動
                        GGrigidbody2D.velocity = new Vector2(0, GGrigidbody2D.velocity.y);
                        if (jump) playerAnimator.Play("waitR");// 前後鍵同時按時不移動
                    }
                    else if (!jump)  //空中移動
                        playerAnimator.Play("jumpendR2");
                    else
                        playerAnimator.Play("leftwalkR");
                }
                else if (Input.GetKey(KeyCode.RightArrow) && jump) //        ------------------------------------------------------------  D
                {
                    GGrigidbody2D.velocity = new Vector2(maxspeed, GGrigidbody2D.velocity.y);
                    if (Input.GetKeyDown(KeyCode.UpArrow)) //  ----------------------------------  DW
                    {
                        if (jump)
                        {
                            playerAnimator.Play("jumpR");
                            GGrigidbody2D.AddForce(transform.up * force);
                            jump = false;
                        }
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow)) // ----------------------  移動換方向
                    { //重製雙按鍵時會有不自然平滑移動
                        GGrigidbody2D.velocity = new Vector2(0, GGrigidbody2D.velocity.y);
                        if (!jump) playerAnimator.Play("jumpend2R");
                        else playerAnimator.Play("waitR"); // 前後鍵同時按時不移動
                    }
                    else if (!jump)  //空中移動
                        playerAnimator.Play("jumpendR2");
                    else
                        playerAnimator.Play("rightwalkR");
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow)) // -------------------------------------------------------------  W
                {
                    if (jump)
                    {
                        playerAnimator.Play("jumpR");
                        GGrigidbody2D.AddForce(transform.up * force);
                        jump = false;
                    }
                }
                else if (Input.GetKeyUp(KeyCode.DownArrow))
                {
                    if (jump)
                    {
                        down = false;
                        playerAnimator.Play("waitR");
                        GGrigidbody2D.velocity = new Vector2(0, 0);
                    }
                }
                else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
                {
                    if (jump)
                        GGrigidbody2D.velocity = new Vector2(0, 0);
                }
                else if (!jump)
                {
                    playerAnimator.Play("jumpendR2");
                }
                else if (jump)
                { //在地板   放置動作
                    playerAnimator.Play("waitR");
                }

                //***********************************  衝刺  *******************************************

                if (Input.GetKey(KeyCode.P) && speedtime < 0.30f)
                {
                    speedtime += Time.deltaTime; ;
                    maxspeed = 140f;
                }
                else if (Input.GetKeyUp(KeyCode.P) || speedtime >= 0.30f)
                {
                    if (Input.GetKeyUp(KeyCode.P)) speedtime = 0;
                    maxspeed = 40f;
                }
                //*********************************  傷害判定區域  **************************************

                if (GameFunction.Instance.p2hited || GameFunction.Instance.p2hitended)  //受到攻擊時的判定
                {
                    GameFunction.Instance.p2hited = false;

                    GGrigidbody2D.velocity = new Vector2(0, 0);

                    if (GameFunction.Instance.p1up)
                    {
                        GGrigidbody2D.velocity = new Vector2(-100f, GGrigidbody2D.velocity.y + 5f);
                        GameFunction.Instance.p1up = false;
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        if (Input.GetKey(KeyCode.DownArrow)) playerAnimator.Play("downdefendendR");
                        else if (Input.GetKey(KeyCode.DownArrow)) playerAnimator.Play("downdefendR");
                        else if (skillhited) playerAnimator.Play("defendendR");
                        else playerAnimator.Play("defendR");
                        block = false;
                        GGrigidbody2D.velocity = new Vector2(-10f, GGrigidbody2D.velocity.y + 5f);
                    }
                    else
                    {
                        if (!jump || GameFunction.Instance.p2hitended)// 包含重擊  擊飛
                        {
                            if (jump) GGrigidbody2D.velocity = new Vector2(-100f, GGrigidbody2D.velocity.y + 5f); //地面擊飛效果
                            else GGrigidbody2D.velocity = new Vector2(-60f, GGrigidbody2D.velocity.y + 5f); //空中擊飛效果
                            playerAnimator.Play("hitdownR");
                        }
                        else if (Input.GetKey(KeyCode.DownArrow))
                        {
                            GGrigidbody2D.velocity = new Vector2(-10f, GGrigidbody2D.velocity.y + 5f);
                            playerAnimator.Play("downhitedR");
                        }
                        else
                        {
                            GGrigidbody2D.velocity = new Vector2(-10f, GGrigidbody2D.velocity.y + 5f);
                            playerAnimator.Play("hitedR");
                        }
                        block = false;
                        GameFunction.Instance.Score2(10);
                    }
                    GameFunction.Instance.p2hitended = false;
                }

                //*********************************  傷害判定區域  end  **************************************

            }          //**********************************  面向右 end   ********************************************

            else
            {           //**********************************  面向左   ********************************************

                //GameFunction.Instance.pos2 = gameObject.transform.position + new Vector3(-13f, 0, 0);// PLAYER 2  拳頭位子

                if (Input.GetKey(KeyCode.DownArrow) && jump) //  ----------------------------------------------------------  S
                {

                    GGrigidbody2D.velocity = new Vector2(0, 0);
                    if (Input.GetKey(KeyCode.LeftArrow))// ------------------------------------------------------  技能
                    {
                        if (Input.GetKeyDown(KeyCode.RightBracket))
                        {
                            GameFunction.Instance.p2hittime = GameFunction.Instance.p2hittime + 1;
                            GGrigidbody2D.velocity = new Vector2(0, 0);
                            Instantiate(leftskill, GameFunction.Instance.pos2 - new Vector3(20f, 0, 0), gameObject.transform.rotation);
                            playerAnimator.Play("skillL");
                            block = false;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftBracket))   //  蹲下攻擊
                    {
                        GameFunction.Instance.p2hittime = GameFunction.Instance.p2hittime + 1;
                        playerAnimator.Play("downhitL");
                        block = false;
                        GameFunction.Instance.p2hit = true;
                        if (GameFunction.Instance.pos2.x - AttackDistance < GameFunction.Instance.pos1.x + 10f &&
                            GameFunction.Instance.pos2.x - AttackDistance > GameFunction.Instance.pos1.x - 10f)
                        {
                            if (GameFunction.Instance.pos2.y > GameFunction.Instance.pos1.y - 20f &&
                                GameFunction.Instance.pos2.y < GameFunction.Instance.pos1.y + 20f)  //再次確認y軸  高度
                                GameFunction.Instance.p1hited = true;
                        }
                    }
                    else if (down)    //  執行蹲下"完成"動作   避免整個蹲下動作一直輪迴
                    {
                        playerAnimator.Play("downendL");
                    }
                    else
                    {      //執行蹲下"進行中"動畫
                        GGrigidbody2D.velocity = new Vector2(0, 0);
                        playerAnimator.Play("downL");
                        block = false;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.LeftBracket)) // -----------------------------------------------------  attack(含空中)
                {
                    //Instantiate(bools, GameFunction.Instance.pos2 - new Vector3(13f, 0, 0), gameObject.transform.rotation);
                    GameFunction.Instance.p2hittime = GameFunction.Instance.p2hittime + 1;
                    if (jump)   //地面攻擊
                    {
                        GGrigidbody2D.velocity = new Vector2(0, 0);
                        playerAnimator.Play("hitL");
                    }
                    else
                    {   //  空中攻擊
                        playerAnimator.Play("jumphitL");
                    }
                    block = false;
                    GameFunction.Instance.p2hit = true;  //  通知 gamefuntion p2 在攻擊
                    if (GameFunction.Instance.pos2.x - AttackDistance < GameFunction.Instance.pos1.x + 10f &&
                        GameFunction.Instance.pos2.x - AttackDistance > GameFunction.Instance.pos1.x - 10f)
                    {
                        if (GameFunction.Instance.pos2.y > GameFunction.Instance.pos1.y - 20f &&
                            GameFunction.Instance.pos2.y < GameFunction.Instance.pos1.y + 20f)  //再次確認y軸  高度
                            GameFunction.Instance.p1hited = true;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.RightBracket) && jump)// ------------------------------------------------------  重擊
                {
                    GameFunction.Instance.p2hittime = GameFunction.Instance.p2hittime + 1;
                    if (jump) GGrigidbody2D.velocity = new Vector2(0, 0);
                    GameFunction.Instance.p2hit = true;  //  通知 gamefuntion p2 在攻擊
                    playerAnimator.Play("hitendL");
                    block = false;
                    GameFunction.Instance.p2hit = true;  //  通知 gamefuntion p1 在攻擊
                    if (GameFunction.Instance.pos2.x - AttackDistance < GameFunction.Instance.pos1.x + 13f &&
                        GameFunction.Instance.pos2.x - AttackDistance > GameFunction.Instance.pos1.x - 13f)
                    {
                        if (GameFunction.Instance.pos2.y > GameFunction.Instance.pos1.y - 20f &&
                            GameFunction.Instance.pos2.y < GameFunction.Instance.pos1.y + 20f)  //再次確認y軸  高度
                        {
                            GameFunction.Instance.p1hitended = true;

                        }
                    }
                }
                else if (Input.GetKey(KeyCode.LeftArrow) && jump) //      ------------------------------------------------------------   A
                {
                    GGrigidbody2D.velocity = new Vector2(-1 * maxspeed, GGrigidbody2D.velocity.y);
                    if (Input.GetKeyDown(KeyCode.UpArrow)) // ----------------------------------  AW
                    {
                        if (jump)
                        {
                            playerAnimator.Play("jumpL");
                            GGrigidbody2D.AddForce(transform.up * force);
                            jump = false;
                        }
                    }
                    else if (Input.GetKey(KeyCode.RightArrow)) // ----------------------  移動換方向
                    { //重製雙按鍵時會有不自然平滑移動
                        GGrigidbody2D.velocity = new Vector2(0, GGrigidbody2D.velocity.y);
                        if (jump) playerAnimator.Play("waitL");// 前後鍵同時按時不移動
                    }
                    else if (!jump)  //空中移動
                        playerAnimator.Play("jumpendL2");
                    else
                        playerAnimator.Play("leftwalkL");
                }
                else if (Input.GetKey(KeyCode.RightArrow) && jump) //        ------------------------------------------------------------  D
                {
                    GGrigidbody2D.velocity = new Vector2(maxspeed, GGrigidbody2D.velocity.y);
                    if (Input.GetKeyDown(KeyCode.UpArrow)) //  ----------------------------------  DW
                    {
                        if (jump)
                        {
                            playerAnimator.Play("jumpL");
                            GGrigidbody2D.AddForce(transform.up * force);
                            jump = false;
                        }
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow)) // ----------------------  移動換方向
                    { //重製雙按鍵時會有不自然平滑移動
                        GGrigidbody2D.velocity = new Vector2(0, GGrigidbody2D.velocity.y);
                        if (!jump) playerAnimator.Play("jumpend2L");
                        else playerAnimator.Play("waitL"); // 前後鍵同時按時不移動
                    }
                    else if (!jump)  //空中移動
                        playerAnimator.Play("jumpendL2");
                    else
                        playerAnimator.Play("rightwalkL");
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow)) // -------------------------------------------------------------  W
                {
                    if (jump)
                    {
                        playerAnimator.Play("jumpL");
                        GGrigidbody2D.AddForce(transform.up * force);
                        jump = false;
                    }
                }
                else if (Input.GetKeyUp(KeyCode.DownArrow))
                {

                    if (jump)
                    {
                        down = false;
                        playerAnimator.Play("waitL");
                        GGrigidbody2D.velocity = new Vector2(0, 0);
                    }
                }
                else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
                {
                    if (jump)
                        GGrigidbody2D.velocity = new Vector2(0, 0);
                }
                else if (!jump)
                {
                    playerAnimator.Play("jumpendL2");
                }
                else if (jump)  //在地板   放置動作
                {
                    playerAnimator.Play("waitL");
                }

                //***********************************  衝刺  *******************************************

                if (Input.GetKey(KeyCode.P) && speedtime < 0.30f)
                {
                    speedtime += Time.deltaTime; ;
                    maxspeed = 140f;
                }
                else if (Input.GetKeyUp(KeyCode.P) || speedtime >= 0.30f)
                {
                    if (Input.GetKeyUp(KeyCode.P)) speedtime = 0;
                    maxspeed = 40f;
                }
                //*********************************  傷害判定區域  **************************************


                if (GameFunction.Instance.p2hited || GameFunction.Instance.p2hitended)  //受到攻擊時的判定
                {
                    GameFunction.Instance.p2hited = false;

                    GGrigidbody2D.velocity = new Vector2(0, 0);

                    if (GameFunction.Instance.p1up)
                    {
                        GGrigidbody2D.velocity = new Vector2(100f, GGrigidbody2D.velocity.y + 5f);
                        GameFunction.Instance.p1up = false;
                    }
                    else if (Input.GetKey(KeyCode.RightArrow))
                    {
                        if (Input.GetKey(KeyCode.DownArrow)) playerAnimator.Play("downdefendendL");
                        else if (Input.GetKey(KeyCode.DownArrow)) playerAnimator.Play("downdefendL");
                        else if (skillhited) playerAnimator.Play("defendendL");
                        else playerAnimator.Play("defendL");
                        block = false;
                        GGrigidbody2D.velocity = new Vector2(10f, GGrigidbody2D.velocity.y + 5f);
                    }
                    else
                    {
                        if (!jump || GameFunction.Instance.p2hitended) // 包含重擊  擊飛
                        {
                            if (jump) GGrigidbody2D.velocity = new Vector2(100f, GGrigidbody2D.velocity.y + 5f); //地面擊飛效果
                            else GGrigidbody2D.velocity = new Vector2(60f, GGrigidbody2D.velocity.y + 5f); //空中擊飛效果
                            playerAnimator.Play("hitdownL");
                        }
                        else if (Input.GetKey(KeyCode.DownArrow))
                        {
                            GGrigidbody2D.velocity = new Vector2(10f, GGrigidbody2D.velocity.y + 5f);
                            playerAnimator.Play("downhitedL");
                        }
                        else
                        {
                            GGrigidbody2D.velocity = new Vector2(10f, GGrigidbody2D.velocity.y + 5f);
                            playerAnimator.Play("hitedL");
                        }
                        block = false;
                        GameFunction.Instance.Score2(10);
                    }
                    GameFunction.Instance.p2hitended = false;
                }

                //*********************************  傷害判定區域  end  **************************************

            }           //**********************************  面向左 end   ********************************************

        }   //----------------------------------------------------------- block
        else
        {
            //********************************** 初步屏蔽初始區域  *************************************

            if (animatorInfo.IsName("waitR"))
            {
                if (hiting && hittime == 1)
                {
                    hiting = false;
                    playerAnimator.Play("hitR");
                    //------  攻擊判斷  -----
                    GameFunction.Instance.p2hittime = GameFunction.Instance.p2hittime + 1;
                    GameFunction.Instance.p2hit = true;  //  通知 gamefuntion p1 在攻擊
                    if (GameFunction.Instance.pos2.x + AttackDistance > GameFunction.Instance.pos1.x - 10f &&
                        GameFunction.Instance.pos2.x + AttackDistance < GameFunction.Instance.pos1.x + 10f)
                    {
                        if (GameFunction.Instance.pos2.y > GameFunction.Instance.pos1.y - 20f &&
                            GameFunction.Instance.pos2.y < GameFunction.Instance.pos1.y + 20f)  //再次確認y軸  高度
                            GameFunction.Instance.p1hited = true;
                    }
                }
                else if (hiting && hittime == 2)
                {
                    hiting = false;
                    playerAnimator.Play("hitendR");
                    //------  攻擊判斷  -----
                    GameFunction.Instance.p2hittime = GameFunction.Instance.p2hittime + 1;
                    GameFunction.Instance.p2hit = true;  //  通知 gamefuntion p1 在攻擊
                    if (GameFunction.Instance.pos2.x + AttackDistance > GameFunction.Instance.pos1.x - 13f &&
                        GameFunction.Instance.pos2.x + AttackDistance < GameFunction.Instance.pos1.x + 13f)
                    {
                        if (GameFunction.Instance.pos2.y > GameFunction.Instance.pos1.y - 20f &&
                            GameFunction.Instance.pos2.y < GameFunction.Instance.pos1.y + 20f)
                        {
                            GameFunction.Instance.p1hitended = true;
                        }  //再次確認y軸  高度
                    }
                }
                else
                {
                    block = true;
                    GameFunction.Instance.p2hit = false;
                    if (skillhited) skillhited = false;
                }
            }

            if (animatorInfo.IsName("waitL"))
            {
                if (hiting && hittime == 1)
                {
                    hiting = false;
                    playerAnimator.Play("hitL");
                    //------  攻擊判斷  -----
                    GameFunction.Instance.p2hittime = GameFunction.Instance.p2hittime + 1;
                    GameFunction.Instance.p2hit = true;  //  通知 gamefuntion p2 在攻擊
                    if (GameFunction.Instance.pos2.x - AttackDistance < GameFunction.Instance.pos1.x + 10f &&
                        GameFunction.Instance.pos2.x - AttackDistance > GameFunction.Instance.pos1.x - 10f)
                    {
                        if (GameFunction.Instance.pos2.y > GameFunction.Instance.pos1.y - 20f &&
                            GameFunction.Instance.pos2.y < GameFunction.Instance.pos1.y + 20f)  //再次確認y軸  高度
                            GameFunction.Instance.p1hited = true;
                    }
                }
                else if (hiting && hittime == 2)
                {
                    hiting = false;
                    playerAnimator.Play("hitendL");
                    //------  攻擊判斷  -----
                    GameFunction.Instance.p2hittime = GameFunction.Instance.p2hittime + 1;
                    GameFunction.Instance.p2hit = true;  //  通知 gamefuntion p1 在攻擊
                    if (GameFunction.Instance.pos2.x - AttackDistance < GameFunction.Instance.pos1.x + 13f &&
                        GameFunction.Instance.pos2.x - AttackDistance > GameFunction.Instance.pos1.x - 13f)
                    {
                        if (GameFunction.Instance.pos2.y > GameFunction.Instance.pos1.y - 20f &&
                            GameFunction.Instance.pos2.y < GameFunction.Instance.pos1.y + 20f)  //再次確認y軸  高度
                        {
                            GameFunction.Instance.p1hitended = true;
                        }
                    }
                }
                else
                {
                    block = true;
                    GameFunction.Instance.p2hit = false;
                    if (skillhited) skillhited = false;
                }

            }

            if (animatorInfo.IsName("hitendR2") || animatorInfo.IsName("hitendL2"))
            {
                block = true;
                GameFunction.Instance.p2hit = false;
                if (skillhited) skillhited = false;
            }

            if (animatorInfo.IsName("hitedR") || animatorInfo.IsName("hitR"))
            {

                if (GameFunction.Instance.p2hited || GameFunction.Instance.p2hitended)  //受到攻擊時的判定
                {

                    playerAnimator.Rebind();
                    GameFunction.Instance.p2hited = false;

                    GGrigidbody2D.velocity = new Vector2(0, 0);

                    if (!jump || GameFunction.Instance.p2hitended)// 包含重擊  擊飛
                    {
                        if (jump) GGrigidbody2D.velocity = new Vector2(-100f, GGrigidbody2D.velocity.y + 5f); //地面擊飛效果
                        else GGrigidbody2D.velocity = new Vector2(-60f, GGrigidbody2D.velocity.y + 5f); //空中擊飛效果
                        playerAnimator.Play("hitdownR");
                    }
                    else if (Input.GetKey(KeyCode.S))
                    {
                        GGrigidbody2D.velocity = new Vector2(-10f, GGrigidbody2D.velocity.y + 5f);
                        playerAnimator.Play("downhitedR");
                    }
                    else
                    {
                        GGrigidbody2D.velocity = new Vector2(-10f, GGrigidbody2D.velocity.y + 5f);
                        playerAnimator.Play("hitedR");
                    }
                    block = false;
                    GameFunction.Instance.Score2(10);
                }
                GameFunction.Instance.p2hitended = false;
            }

            if (animatorInfo.IsName("hitedL") || animatorInfo.IsName("hitL"))
            {
                if (GameFunction.Instance.p2hited || GameFunction.Instance.p2hitended)  //受到攻擊時的判定
                {

                    playerAnimator.Rebind();
                    GameFunction.Instance.p2hited = false;

                    GGrigidbody2D.velocity = new Vector2(0, 0);

                    if (!jump || GameFunction.Instance.p2hitended)// 包含重擊  擊飛
                    {
                        if (jump) GGrigidbody2D.velocity = new Vector2(100f, GGrigidbody2D.velocity.y + 5f); //地面擊飛效果
                        else GGrigidbody2D.velocity = new Vector2(60f, GGrigidbody2D.velocity.y + 5f); //空中擊飛效果
                        playerAnimator.Play("hitdownL");
                    }
                    else if (Input.GetKey(KeyCode.S))
                    {
                        GGrigidbody2D.velocity = new Vector2(10f, GGrigidbody2D.velocity.y + 5f);
                        playerAnimator.Play("downhitedL");
                    }
                    else
                    {
                        GGrigidbody2D.velocity = new Vector2(10f, GGrigidbody2D.velocity.y + 5f);
                        playerAnimator.Play("hitedL");
                    }
                    block = false;
                    GameFunction.Instance.Score2(10);
                }
                GameFunction.Instance.p2hitended = false;
            }
            //********************************** 面相右  屏蔽區域  *************************************

            if (rightface)
            {


                if (animatorInfo.IsName("jumphitR"))     //   跳躍攻擊進行時若是著地    要中斷
                    if (jump)
                    {
                        block = true;
                        GameFunction.Instance.p2hit = false;
                    }

                //     -----------------------------------------------------------------

                if (animatorInfo.IsName("hitdownendR2"))
                {
                    playerAnimator.Play("hitdownendR2"); //還沒著地前保持倒地動作
                    if (jump)
                    {
                        playerAnimator.Play("hitdownendR");  //著地  執行起身動作
                        GGrigidbody2D.velocity = new Vector2(0, 0);
                    }
                }
                //     -----------------------------------------------------------------

                else if (!animatorInfo.IsName("hitdownendR") && jumpend)   // 執行著地動作
                {
                    jumpend = false;
                    playerAnimator.Play("jumpendR");
                }
                //     -----------------------------------------------------------------

                if (animatorInfo.IsName("downendR"))   //使動作卡在downend   不會再一次輪回到down   用down參數判斷
                {
                    if (hiting && hittime == 1)
                    {
                        hiting = false;
                        playerAnimator.Play("downhitR");
                        //------  攻擊判斷  -----
                        GameFunction.Instance.p2hittime = GameFunction.Instance.p2hittime + 1;
                        GameFunction.Instance.p2hit = true;  //  通知 gamefuntion p1 在攻擊
                        if (GameFunction.Instance.pos2.x + AttackDistance > GameFunction.Instance.pos1.x - 10f &&
                            GameFunction.Instance.pos2.x + AttackDistance < GameFunction.Instance.pos1.x + 10f)
                        {
                            if (GameFunction.Instance.pos2.y > GameFunction.Instance.pos1.y - 20f &&
                                GameFunction.Instance.pos2.y < GameFunction.Instance.pos1.y + 20f)  //再次確認y軸  高度
                                GameFunction.Instance.p1hited = true;
                        }
                    }
                    else if (hiting && hittime == 2)
                    {
                        hiting = false;
                        playerAnimator.Play("hitendR");
                        //------  攻擊判斷  -----
                        GameFunction.Instance.p2hittime = GameFunction.Instance.p2hittime + 1;
                        GameFunction.Instance.p2hit = true;  //  通知 gamefuntion p1 在攻擊
                        if (GameFunction.Instance.pos2.x + AttackDistance > GameFunction.Instance.pos1.x - 13f &&
                            GameFunction.Instance.pos2.x + AttackDistance < GameFunction.Instance.pos1.x + 13f)
                        {
                            if (GameFunction.Instance.pos2.y > GameFunction.Instance.pos1.y - 20f &&
                                GameFunction.Instance.pos2.y < GameFunction.Instance.pos1.y + 20f)
                            {
                                GameFunction.Instance.p1hitended = true;
                            }  //再次確認y軸  高度
                        }
                    }
                    else block = true;
                    down = true;
                }


                //     --------------------------------  技能被擊中時防禦延遲  ---------------------------------

                if (skillhited)
                {
                    time += Time.deltaTime; //時間增加
                    if (time > 1f)
                    {
                        skillhited = false;
                        time = 0;
                        block = true;
                    }

                }

                //     --------------------------------  連擊  ---------------------------------
                if (animatorInfo.IsName("hitR") || animatorInfo.IsName("downhitR"))
                {
                    if (Input.GetKeyDown(KeyCode.LeftBracket) && !hiting)
                    {
                        hiting = true;
                        hittime = hittime + 1;
                    }
                }

            }
            //********************************** 面相左  屏蔽區域   *************************************



            //     -----------------------------------------------------------------
            else
            {

                if (animatorInfo.IsName("jumphitL"))     //   跳躍攻擊進行時若是著地    要中斷
                    if (jump)
                    {
                        block = true;
                        GameFunction.Instance.p2hit = false;
                    }

                //     -----------------------------------------------------------------

                if (animatorInfo.IsName("hitdownendL2"))
                {
                    playerAnimator.Play("hitdownendL2"); //還沒著地前保持倒地動作

                    if (jump)
                    {
                        GGrigidbody2D.velocity = new Vector2(0, 0);
                        playerAnimator.Play("hitdownendL");  //著地  執行起身動作
                    }

                }

                //     -----------------------------------------------------------------

                else if (!animatorInfo.IsName("hitdownendL") && jumpend)   // 執行著地動作
                {
                    jumpend = false;
                    playerAnimator.Play("jumpendL");
                }
                //     -----------------------------------------------------------------

                if (animatorInfo.IsName("downendL"))   //使動作卡在downend   不會再一次輪回到down   用down參數判斷
                {
                    if (hiting && hittime == 1)
                    {
                        hiting = false;
                        playerAnimator.Play("downhitL");
                        //------  攻擊判斷  -----
                        GameFunction.Instance.p2hittime = GameFunction.Instance.p2hittime + 1;
                        GameFunction.Instance.p2hit = true;  //  通知 gamefuntion p2 在攻擊
                        if (GameFunction.Instance.pos2.x - AttackDistance < GameFunction.Instance.pos1.x + 10f &&
                            GameFunction.Instance.pos2.x - AttackDistance > GameFunction.Instance.pos1.x - 10f)
                        {
                            if (GameFunction.Instance.pos2.y > GameFunction.Instance.pos1.y - 20f &&
                                GameFunction.Instance.pos2.y < GameFunction.Instance.pos1.y + 20f)  //再次確認y軸  高度
                                GameFunction.Instance.p1hited = true;
                        }
                    }
                    else if (hiting && hittime == 2)
                    {
                        hiting = false;
                        playerAnimator.Play("hitendL");
                        //------  攻擊判斷  -----
                        GameFunction.Instance.p2hittime = GameFunction.Instance.p2hittime + 1;
                        GameFunction.Instance.p2hit = true;  //  通知 gamefuntion p1 在攻擊
                        if (GameFunction.Instance.pos2.x - AttackDistance < GameFunction.Instance.pos1.x + 13f &&
                            GameFunction.Instance.pos2.x - AttackDistance > GameFunction.Instance.pos1.x - 13f)
                        {
                            if (GameFunction.Instance.pos2.y > GameFunction.Instance.pos1.y - 20f &&
                                GameFunction.Instance.pos2.y < GameFunction.Instance.pos1.y + 20f)  //再次確認y軸  高度
                            {
                                GameFunction.Instance.p1hitended = true;
                            }
                        }
                    }
                    else block = true;
                    down = true;
                }

                //     ------------------------------  技能被擊中時防禦延遲  -----------------------------------

                if (skillhited)
                {
                    time += Time.deltaTime; //時間增加
                    if (time > 1f)
                    {
                        skillhited = false;
                        time = 0;
                        block = true;
                    }

                }

                //     --------------------------------  連擊  ---------------------------------
                if (animatorInfo.IsName("hitL") || animatorInfo.IsName("downhitL"))
                {
                    if (Input.GetKeyDown(KeyCode.LeftBracket) && !hiting)
                    {
                        hiting = true;
                        hittime = hittime + 1;
                    }
                }
            }
            //********************************** 面相左  屏蔽區域  end  *************************************
            if (animatorInfo.IsName("hitdownendR") || animatorInfo.IsName("hitdownendL"))
            {
                if (GameFunction.Instance.pos2.x - 30f < GameFunction.Instance.pos1.x &&
                    GameFunction.Instance.pos2.x + 30f > GameFunction.Instance.pos1.x)
                {
                    GameFunction.Instance.p1hited = true;
                    GameFunction.Instance.p2up = true;
                }
            }
        } //  ! block  

        //----------------------  跳躍   ----------------------

        if (gameObject.transform.position.y > 10)
        {
            GGrigidbody2D.AddForce(transform.up * -jumptforce);
        }

    } // update
}
