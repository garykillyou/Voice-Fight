using UnityEngine;
using UnityEngine.Networking;

public class player1Controller_PVP : NetworkBehaviour
{

    public bool block = true;

    public GameObject bools;
    public GameObject rightskill;
    public GameObject leftskill;
    public float force = 10000;
    public float maxspeed = 40f;
    public float AttackDistance = 13f; // 攻擊距離
    Rigidbody2D GGrigidbody2D;

    public float time = 0; //計時   擊倒時使用
    public float speedtime = 0; //衝刺計時   擊倒時使用
    public float jumptforce = 1000; //跳躍速度控制器
    public float downpeed = 60f;
    public int hittime = 0; // 攻擊次數
    public bool hiting = false; // 判斷是否連擊
    public bool jump = false;
    public bool jumpend = false; // 補丁   執行落地動作
    public bool down = false;
    public bool rightface = true;  //  是否面相右方向
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
            jumpend = true; // 執行落地動作
        }
        else if (col.tag == "skill")
        {

            skillhited = true;
            //checkR(); //執行傷害確認
            if (block) CmdTellServerP1hited(true); // GameFunction_PVP.Instance.p1hited = true;
        }
        else if (col.tag == "player2") //跳躍時碰撞
        {
            GGrigidbody2D.velocity = new Vector2(0, GGrigidbody2D.velocity.y);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            playerAnimator = (Animator)GetComponent("Animator");
            animatorInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);//抓取現在再跑的動畫名

            //GameFunction_PVP.Instance.pos1 = gameObject.transform.position;//PLAYER1 
            CmdTellServerP1position();

            if (GameFunction_PVP.Instance.pos1.x > GameFunction_PVP.Instance.pos2.x)
            {
                rightface = false;
            }
            else
            {
                rightface = true;
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                if (rightface) rightface = false;
                else rightface = true;
            }


            if (GameFunction_PVP.Instance.IsPlaying && block)
            {

                if (rightface)
                {      //**********************************  面相右  ********************************************

                    //GameFunction_PVP.Instance.pos1 = gameObject.transform.position + new Vector3(13f, 0, 0);//PLAYER1 

                    if (Input.GetKey(KeyCode.S) && jump) //  ----------------------------------------------------------  S
                    {
                        GGrigidbody2D.velocity = new Vector2(0, 0);
                        if (Input.GetKeyDown(KeyCode.Space))   //  蹲下攻擊
                        {
                            //GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                            CmdTellServerP1hitTime();
                            playerAnimator.Play("downhitR");
                            block = false;
                            CmdTellServerP1hit(true); // GameFunction_PVP.Instance.p1hit = true;
                            if (GameFunction_PVP.Instance.pos1.x + AttackDistance > GameFunction_PVP.Instance.pos2.x - 10f)
                            {
                                if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 10f)  //再次確認y軸  高度
                                    CmdTellServerP2hited(true); // GameFunction_PVP.Instance.p2hited = true;
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
                    else if (Input.GetKeyDown(KeyCode.Space)) // -----------------------------------------------------  attack(含空中)
                    {
                        //GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                        CmdTellServerP1hitTime();
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
                        CmdTellServerP1hit(true); // GameFunction_PVP.Instance.p1hit = true;  //  通知 gamefuntion p1 在攻擊
                        if (GameFunction_PVP.Instance.pos1.x + AttackDistance > GameFunction_PVP.Instance.pos2.x - 10f &&
                            GameFunction_PVP.Instance.pos1.x + AttackDistance < GameFunction_PVP.Instance.pos2.x + 10f)
                        {
                            if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                CmdTellServerP2hited(true); // GameFunction_PVP.Instance.p2hited = true;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.Z) && jump) // ------------------------------------------------------  技能
                    {
                        //GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                        CmdTellServerP1hitTime();
                        GGrigidbody2D.velocity = new Vector2(0, 0);
                        //Instantiate(rightskill, GameFunction_PVP.Instance.pos1 + new Vector3(20f, 0, 0), gameObject.transform.rotation);
                        CmdTellServerRightSkill();
                        playerAnimator.Play("skillR");
                        block = false;
                    }
                    else if (Input.GetKeyDown(KeyCode.X) && jump) // ------------------------------------------------------------  重擊
                    {
                        //GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                        CmdTellServerP1hitTime();
                        if (jump) GGrigidbody2D.velocity = new Vector2(0, 0);
                        CmdTellServerP1hit(true); // GameFunction_PVP.Instance.p1hit = true;  //  通知 gamefuntion p1 在攻擊
                        playerAnimator.Play("hitendR");
                        block = false;
                        CmdTellServerP1hit(true); // GameFunction_PVP.Instance.p1hit = true;  //  通知 gamefuntion p1 在攻擊
                        if (GameFunction_PVP.Instance.pos1.x + AttackDistance > GameFunction_PVP.Instance.pos2.x - 20f &&
                            GameFunction_PVP.Instance.pos1.x + AttackDistance < GameFunction_PVP.Instance.pos2.x + 20f)
                        {
                            if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)
                            {
                                CmdTellServerP2hitended(true); // GameFunction_PVP.Instance.p2hitended = true;
                            }  //再次確認y軸  高度
                        }
                    }
                    else if (Input.GetKey(KeyCode.A) && jump) //      ------------------------------------------------------------   A
                    {
                        GGrigidbody2D.velocity = new Vector2(-1 * maxspeed, GGrigidbody2D.velocity.y);
                        if (Input.GetKeyDown(KeyCode.W)) // ----------------------------------  AW
                        {
                            if (jump)
                            {
                                playerAnimator.Play("jumpR");
                                GGrigidbody2D.AddForce(transform.up * force);
                                jump = false;
                            }
                        }
                        else if (Input.GetKey(KeyCode.D)) // ----------------------  移動換方向
                        { //重製雙按鍵時會有不自然平滑移動
                            GGrigidbody2D.velocity = new Vector2(0, GGrigidbody2D.velocity.y);
                            if (jump) playerAnimator.Play("waitR");// 前後鍵同時按時不移動
                        }
                        else if (!jump)  //空中移動
                            playerAnimator.Play("jumpendR2");
                        else
                            playerAnimator.Play("leftwalkR");
                    }
                    else if (Input.GetKey(KeyCode.D) && jump) //        ------------------------------------------------------------  D
                    {
                        GGrigidbody2D.velocity = new Vector2(maxspeed, GGrigidbody2D.velocity.y);
                        if (Input.GetKeyDown(KeyCode.W)) //  ----------------------------------  DW
                        {
                            if (jump)
                            {
                                playerAnimator.Play("jumpR");
                                GGrigidbody2D.AddForce(transform.up * force);
                                jump = false;
                            }
                        }
                        else if (Input.GetKey(KeyCode.A)) // ----------------------  移動換方向
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
                    else if (Input.GetKeyDown(KeyCode.W)) // -------------------------------------------------------------  W
                    {
                        if (jump)
                        {
                            playerAnimator.Play("jumpR");
                            GGrigidbody2D.AddForce(transform.up * force);
                            jump = false;
                        }
                    }
                    else if (Input.GetKeyUp(KeyCode.S))
                    {
                        if (jump)
                        {
                            down = false;
                            GGrigidbody2D.velocity = new Vector2(0, 0);
                            playerAnimator.Play("waitR");

                        }

                    }
                    else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
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

                    //*********************************  傷害判定區域  **************************************

                    if (GameFunction_PVP.Instance.p1hited || GameFunction_PVP.Instance.p1hitended)  //受到攻擊時的判定
                    {
                        CmdTellServerP1hited(false); // GameFunction_PVP.Instance.p1hited = false;

                        GGrigidbody2D.velocity = new Vector2(0, 0);

                        if (Input.GetKey(KeyCode.A))
                        {
                            if (Input.GetKey(KeyCode.S)) playerAnimator.Play("downdefendR");
                            else playerAnimator.Play("defendR");
                            block = false;
                        }
                        else
                        {

                            if (!jump || GameFunction_PVP.Instance.p1hitended)// 包含重擊  擊飛
                            {
                                GGrigidbody2D.velocity = new Vector2(-40f, GGrigidbody2D.velocity.y + 5f); //擊飛效果
                                playerAnimator.Play("hitdownR");
                            }
                            else if (Input.GetKey(KeyCode.S)) playerAnimator.Play("downhitedR");
                            else playerAnimator.Play("hitedR");
                            block = false;
                            GameFunction_PVP.Instance.Score1();
                        }
                        CmdTellServerP1hitended(false); // GameFunction_PVP.Instance.p1hitended = false;
                    }

                    //checkR();
                    //*********************************  傷害判定區域  end  **************************************

                }          //**********************************  面向右 end   ********************************************

                else
                {           //**********************************  面向左   ********************************************

                    //GameFunction_PVP.Instance.pos1 = gameObject.transform.position + new Vector3(-13f, 0, 0);//PLAYER1 

                    if (Input.GetKey(KeyCode.S) && jump) //  ----------------------------------------------------------  S
                    {

                        GGrigidbody2D.velocity = new Vector2(0, 0);
                        if (Input.GetKeyDown(KeyCode.Space))   //  蹲下攻擊
                        {
                            //GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                            CmdTellServerP1hitTime();
                            playerAnimator.Play("downhitL");
                            block = false;
                            CmdTellServerP1hit(true); // GameFunction_PVP.Instance.p1hit = true;
                            if (GameFunction_PVP.Instance.pos1.x - AttackDistance > GameFunction_PVP.Instance.pos2.x - 10f &&
                                GameFunction_PVP.Instance.pos1.x - AttackDistance < GameFunction_PVP.Instance.pos2.x + 10f)
                            {
                                if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                    GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                    CmdTellServerP2hited(true); // GameFunction_PVP.Instance.p2hited = true;
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
                    else if (Input.GetKeyDown(KeyCode.Space)) // -----------------------------------------------------  attack(含空中)
                    {
                        //GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                        CmdTellServerP1hitTime();
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
                        CmdTellServerP1hit(true); // GameFunction_PVP.Instance.p1hit = true;  //  通知 gamefuntion p1 在攻擊
                        if (GameFunction_PVP.Instance.pos1.x - AttackDistance > GameFunction_PVP.Instance.pos2.x - 10f &&
                            GameFunction_PVP.Instance.pos1.x - AttackDistance < GameFunction_PVP.Instance.pos2.x + 10f)
                        {
                            if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                CmdTellServerP2hited(true); // GameFunction_PVP.Instance.p2hited = true;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.Z) && jump)  // ------------------------------------------------------  技能
                    {
                        //GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                        CmdTellServerP1hitTime();
                        GGrigidbody2D.velocity = new Vector2(0, 0);
                        //Instantiate(leftskill, GameFunction_PVP.Instance.pos1 - new Vector3(20f, 0, 0), gameObject.transform.rotation);
                        CmdTellServerLeftSkill();
                        playerAnimator.Play("skillL");
                        block = false;
                    }
                    else if (Input.GetKeyDown(KeyCode.X) && jump)   // ------------------------------------------------------  重擊
                    {
                        //GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                        CmdTellServerP1hitTime();
                        if (jump) GGrigidbody2D.velocity = new Vector2(0, 0);
                        CmdTellServerP1hit(true); // GameFunction_PVP.Instance.p1hit = true;  //  通知 gamefuntion p1 在攻擊
                        playerAnimator.Play("hitendL");
                        block = false;
                        CmdTellServerP1hit(true); // GameFunction_PVP.Instance.p1hit = true;  //  通知 gamefuntion p1 在攻擊
                        if (GameFunction_PVP.Instance.pos1.x - AttackDistance > GameFunction_PVP.Instance.pos2.x - 20f &&
                            GameFunction_PVP.Instance.pos1.x - AttackDistance < GameFunction_PVP.Instance.pos2.x + 20f)
                        {
                            if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)
                            {
                                CmdTellServerP2hitended(true); // GameFunction_PVP.Instance.p2hitended = true;
                            }  //再次確認y軸  高度
                        }
                    }
                    else if (Input.GetKey(KeyCode.A) && jump) //      ------------------------------------------------------------   A
                    {
                        GGrigidbody2D.velocity = new Vector2(-1 * maxspeed, GGrigidbody2D.velocity.y);
                        if (Input.GetKeyDown(KeyCode.W)) // ----------------------------------  AW
                        {
                            if (jump)
                            {
                                playerAnimator.Play("jumpL");
                                GGrigidbody2D.AddForce(transform.up * force);
                                jump = false;
                            }
                        }
                        else if (Input.GetKey(KeyCode.D)) // ----------------------  移動換方向
                        { //重製雙按鍵時會有不自然平滑移動
                            GGrigidbody2D.velocity = new Vector2(0, GGrigidbody2D.velocity.y);
                            if (jump) playerAnimator.Play("waitL");// 前後鍵同時按時不移動
                        }
                        else if (!jump)  //空中移動
                            playerAnimator.Play("jumpendL2");
                        else
                            playerAnimator.Play("leftwalkL");
                    }
                    else if (Input.GetKey(KeyCode.D) && jump) //        ------------------------------------------------------------  D
                    {
                        GGrigidbody2D.velocity = new Vector2(maxspeed, GGrigidbody2D.velocity.y);
                        if (Input.GetKeyDown(KeyCode.W)) //  ----------------------------------  DW
                        {
                            if (jump)
                            {
                                playerAnimator.Play("jumpL");
                                GGrigidbody2D.AddForce(transform.up * force);
                                jump = false;
                            }
                        }
                        else if (Input.GetKey(KeyCode.A)) // ----------------------  移動換方向
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
                    else if (Input.GetKeyDown(KeyCode.W)) // -------------------------------------------------------------  W
                    {
                        if (jump)
                        {
                            playerAnimator.Play("jumpL");
                            GGrigidbody2D.AddForce(transform.up * force);
                            jump = false;
                        }
                    }
                    else if (Input.GetKeyUp(KeyCode.S))
                    {
                        if (jump)
                        {
                            down = false;
                            GGrigidbody2D.velocity = new Vector2(0, 0);
                            playerAnimator.Play("waitL");
                        }
                    }
                    else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
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

                    //*********************************  傷害判定區域  **************************************

                    if (GameFunction_PVP.Instance.p1hited || GameFunction_PVP.Instance.p1hitended)  //受到攻擊時的判定
                    {
                        CmdTellServerP1hited(false); // GameFunction_PVP.Instance.p1hited = false;

                        GGrigidbody2D.velocity = new Vector2(0, 0);

                        if (Input.GetKey(KeyCode.D))
                        {
                            if (Input.GetKey(KeyCode.S)) playerAnimator.Play("downdefendL");
                            else playerAnimator.Play("defendL");
                            block = false;
                        }
                        else
                        {
                            if (!jump || GameFunction_PVP.Instance.p1hitended)// 包含重擊  擊飛
                            {
                                GGrigidbody2D.velocity = new Vector2(-40f, GGrigidbody2D.velocity.y + 5f); //擊飛效果
                                playerAnimator.Play("hitdownL");
                            }
                            else if (Input.GetKey(KeyCode.S)) playerAnimator.Play("downhitedL");
                            else playerAnimator.Play("hitedL");
                            block = false;
                            GameFunction_PVP.Instance.Score1();
                        }
                        CmdTellServerP1hitended(false); // GameFunction_PVP.Instance.p1hitended = false;
                    }

                    //*********************************  傷害判定區域  end  **************************************

                }           //**********************************  面向左 end   ********************************************


            }   // block
            else
            {
                //********************************** 初步屏蔽初始區域  *************************************
                if (animatorInfo.IsName("waitR"))
                {
                    block = true;
                    CmdTellServerP1hit(false); // GameFunction_PVP.Instance.p1hit = false;
                    if (skillhited) skillhited = false;
                }
                if (animatorInfo.IsName("waitL"))
                {
                    block = true;
                    CmdTellServerP1hit(false); // GameFunction_PVP.Instance.p1hit = false;
                    if (skillhited) skillhited = false;
                }

                //********************************** 面相右  屏蔽區域  *************************************

                if (rightface)
                {


                    if (animatorInfo.IsName("jumphitR"))     //   跳躍攻擊進行時若是著地    要中斷
                        if (jump)
                        {
                            block = true;
                            CmdTellServerP1hit(false); // GameFunction_PVP.Instance.p1hit = false;
                        }

                    //     -----------------------------------------------------------------

                    if (animatorInfo.IsName("hitdownendR2"))
                    {
                        playerAnimator.Play("hitdownendR2"); //還沒著地前保持倒地動作
                        if (jump) playerAnimator.Play("hitdownendR");  //著地  執行起身動作

                        time += Time.deltaTime; //時間增加
                        if (time == 1f)
                            GGrigidbody2D.velocity = new Vector2(0, 0);
                        else if (time == 4f)
                        {
                            block = true;
                            time = 0;
                        }
                    }

                    //     -----------------------------------------------------------------

                    if (animatorInfo.IsName("downendR"))   //使動作卡在downend   不會再一次輪回到down   用down參數判斷
                    {
                        block = true;
                        down = true;
                    }

                    //     -----------------------------------------------------------------

                    if (jumpend)   // 執行著地動作
                    {
                        jumpend = false;
                        playerAnimator.Play("jumpendR");
                    }

                    //     --------------------------------  技能被擊中時防禦延遲  ---------------------------------


                    if (skillhited)
                    {
                        time += Time.deltaTime; //時間增加
                        if (time > 2f)
                        {
                            skillhited = false;
                            time = 0;
                            block = true;
                        }

                    }
                    else if (animatorInfo.IsName("defendendR") || animatorInfo.IsName("downdefendendR"))  //  若是確實被攻擊到了   要做到p2攻擊時間結束  無法提早動作
                                                                                                          //  進到這裡只有確實被攻擊到   block確實被屏蔽
                        if (!GameFunction_PVP.Instance.p2hit)
                            block = true;



                }
                //********************************** 面相左  屏蔽區域   *************************************



                //     -----------------------------------------------------------------
                else
                {

                    if (animatorInfo.IsName("jumphitL"))     //   跳躍攻擊進行時若是著地    要中斷
                        if (jump)
                        {
                            block = true;
                            CmdTellServerP1hit(false); // GameFunction_PVP.Instance.p1hit = false;
                        }

                    //     -----------------------------------------------------------------

                    if (animatorInfo.IsName("hitdownendL2"))
                    {
                        playerAnimator.Play("hitdownendL2"); //還沒著地前保持倒地動作

                        if (jump) playerAnimator.Play("hitdownendL");  //著地  執行起身動作

                        time += Time.deltaTime; //時間增加
                        if (time == 1f)
                            GGrigidbody2D.velocity = new Vector2(0, 0);
                        else if (time == 4f)
                        {
                            block = true;
                            time = 0;
                        }
                    }

                    //     -----------------------------------------------------------------

                    if (animatorInfo.IsName("downendL"))   //使動作卡在downend   不會再一次輪回到down   用down參數判斷
                    {
                        block = true;
                        down = true;
                    }

                    //     -----------------------------------------------------------------

                    if (jumpend)   // 執行著地動作
                    {
                        jumpend = false;
                        playerAnimator.Play("jumpendL");
                    }

                    //     ------------------------------  技能被擊中時防禦延遲  -----------------------------------

                    if (skillhited)
                    {
                        time += Time.deltaTime; //時間增加
                        if (time > 2f)
                        {
                            skillhited = false;
                            time = 0;
                            block = true;
                        }

                    }
                    else if (animatorInfo.IsName("defendendL") || animatorInfo.IsName("downdefendendL"))  //  若是確實被攻擊到了   要做到p2攻擊時間結束  無法提早動作
                                                                                                          //  進到這裡只有確實被攻擊到   block確實被屏蔽
                        if (!GameFunction_PVP.Instance.p2hit)
                            block = true;

                }
                //********************************** 面相左  屏蔽區域  end  *************************************
            } //  ! block  
        }
    } // update
      /*
      public void checkR() // 面向右的傷害確認
      {
          playerAnimator = (Animator)GetComponent("Animator");

          if (GameFunction_PVP.Instance.p1hited || GameFunction_PVP.Instance.p1hitended)  //受到攻擊時的判定
          {
              CmdTellServerP1hited(false); // GameFunction_PVP.Instance.p1hited = false;

              GGrigidbody2D.velocity = new Vector2(0, 0);

              if (Input.GetKey(KeyCode.A))
              {
                  if (Input.GetKey(KeyCode.S)) playerAnimator.Play("downdefendR");
                  else playerAnimator.Play("defendR");
                  block = false;
              }
              else
              {

                  if (!jump || GameFunction_PVP.Instance.p1hitended)// 包含重擊  擊飛
                  {
                      GGrigidbody2D.velocity = new Vector2(-40f, GGrigidbody2D.velocity.y + 5f); //擊飛效果
                      playerAnimator.Play("hitdownR");
                  }
                  else if (Input.GetKey(KeyCode.S)) playerAnimator.Play("downhitedR");
                  else playerAnimator.Play("hitedR");
                  block = false;
                  GameFunction_PVP.Instance.Score1();
              }
              CmdTellServerP1hitended(false); // GameFunction_PVP.Instance.p1hitended = false;
          }
      }
      */

    [Command]
    void CmdTellServerP1hitTime()
    {
        GameFunction_PVP.Instance.p1hittime += 1;
    }

    [Command]
    void CmdTellServerP1position()
    {
        GameFunction_PVP.Instance.pos1 = gameObject.transform.position;
    }

    [Command]
    public void CmdTellServerP1hited(bool H)
    {
        GameFunction_PVP.Instance.p1hited = H;
    }

    [Command]
    public void CmdTellServerP1hit(bool H)
    {
        GameFunction_PVP.Instance.p1hit = H;
    }

    [Command]
    public void CmdTellServerP1hitended(bool H)
    {
        GameFunction_PVP.Instance.p1hitended = H;
    }

    [Command]
    public void CmdTellServerP2hited(bool H)
    {
        GameFunction_PVP.Instance.p2hited = H;
    }

    [Command]
    public void CmdTellServerP2hit(bool H)
    {
        GameFunction_PVP.Instance.p2hit = H;
    }

    [Command]
    public void CmdTellServerP2hitended(bool H)
    {
        GameFunction_PVP.Instance.p2hitended = H;
    }

    [Command]
    public void CmdTellServerP1Score(int damage)
    {
        GameFunction_PVP.Instance.Scores1 -= damage;
    }

    [Command]
    public void CmdTellServerP2Score(int damage)
    {
        GameFunction_PVP.Instance.Scores2 -= damage;
    }

    [Command]
    public void CmdTellServerRightSkill()
    {
        GameObject skill = GameObject.Instantiate(rightskill, GameFunction_PVP.Instance.pos1 + new Vector3(20f, 0, 0), Quaternion.identity) as GameObject;
        NetworkServer.Spawn(skill);
    }

    [Command]
    public void CmdTellServerLeftSkill()
    {
        GameObject skill = GameObject.Instantiate(leftskill, GameFunction_PVP.Instance.pos1 - new Vector3(20f, 0, 0), Quaternion.identity) as GameObject;
        NetworkServer.Spawn(skill);
    }

    [Command]
    public void CmdTellServerGameOver()
    {
        GameFunction_PVP.Instance.IsPlaying = false;
        GameObject.Find("CounterText").GetComponent<CounterText>().endCC = true;
    }
}