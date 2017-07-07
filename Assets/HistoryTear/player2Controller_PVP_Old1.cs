using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class player2Controller_PVP_Old1 : NetworkBehaviour
{

    public bool block = true;

    public GameObject bools;
    public GameObject rightskill;
    public GameObject leftskill;
    public float force = 5000;
    public float maxspeed = 20f;
    public float AttackDistance = 13f; // 攻擊距離
    Rigidbody2D GGrigidbody2D;

    public float time; //計時   擊倒時使用
    public float downpeed = 60f;
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

    void Start()
    {
        //P1HealthText = GameObject.Find ("P1Blood");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "down")
        {
            jump = true;
            block = false;
            jumpend = true;
        }
        else if (col.tag == "skill")
        {
            skillhited = true;
            CmdTellServerP2hited(true); //GameFunction_PVP.Instance.p2hited = true;
            Destroy(col.gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            playerAnimator = (Animator)GetComponent("Animator");
            animatorInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);//抓取現在再跑的動畫名

            //GameFunction_PVP.Instance.pos2 = gameObject.transform.position;// PLAYER 2
            CmdTellServerP2position();

            if (GameFunction_PVP.Instance.pos1.x > GameFunction_PVP.Instance.pos2.x)
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


            if (GameFunction_PVP.Instance.IsPlaying && block)
            {

                if (rightface)
                {      //**********************************  面相右  ********************************************

                    //GameFunction_PVP.Instance.pos2 = gameObject.transform.position + new Vector3(13f, 0, 0);// PLAYER 2

                    if (Input.GetKey(KeyCode.DownArrow) && jump) //  ----------------------------------------------------------  S
                    {
                        GGrigidbody2D.velocity = new Vector2(0, 0);
                        if (Input.GetKey(KeyCode.LeftArrow))
                        {
                            if (GameFunction_PVP.Instance.p2hit)     //  並沒有被擊中   距離不夠  但還是會做出防禦動作
                            {
                                playerAnimator.Play("downdefendR");
                            }
                            else
                            {
                                GGrigidbody2D.velocity = new Vector2(0, 0);
                                if (down) playerAnimator.Play("downendR");  //  執行蹲下完成動作   避免整個蹲下動作一直輪迴
                                else playerAnimator.Play("downR");
                                block = false;
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.P))   //  蹲下攻擊
                        {
                            playerAnimator.Play("downhitR");
                            block = false;
                            CmdTellServerP1hit(true); //GameFunction_PVP.Instance.p1hit = true;
                            if (GameFunction_PVP.Instance.pos2.x + AttackDistance > GameFunction_PVP.Instance.pos1.x - 26f)
                            {
                                if (GameFunction_PVP.Instance.pos2.y > GameFunction_PVP.Instance.pos1.y - 10f)  //再次確認y軸  高度
                                                                                                                //GameFunction_PVP.Instance.p1hited = true;
                                    CmdTellServerP1hited(true);
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
                    else if (Input.GetKeyDown(KeyCode.P)) // -----------------------------------------------------  attack(含空中)
                    {
                        Instantiate(bools, GameFunction_PVP.Instance.pos2 + new Vector3(13f, 0, 0), gameObject.transform.rotation);

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
                        CmdTellServerP1hit(true); //GameFunction_PVP.Instance.p1hit = true;  //  通知 gamefuntion p1 在攻擊
                        if (GameFunction_PVP.Instance.pos2.x + AttackDistance > GameFunction_PVP.Instance.pos1.x - 26f)
                        {
                            if (GameFunction_PVP.Instance.pos2.y > GameFunction_PVP.Instance.pos1.y - 10f)  //再次確認y軸  高度
                                CmdTellServerP1hited(true); //GameFunction_PVP.Instance.p1hited = true;
                        }
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow)) //      ------------------------------------------------------------   A
                    {/*
                if (GameFunction_PVP.Instance.p2hit)
                {
                    if (jump) GGrigidbody2D.velocity = new Vector2(0, 0);
                    else GGrigidbody2D.velocity = new Vector2(Mathf.Sign(GGrigidbody2D.velocity.x) * jumpatt, GGrigidbody2D.velocity.y);
                    playerAnimator.Play("defendR");
                }
                else
                {*/
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
                    else if (Input.GetKey(KeyCode.RightArrow)) //        ------------------------------------------------------------  D
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
                        down = false;
                        playerAnimator.Play("waitR");
                    }
                    else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
                    {
                        if (jump)
                            GGrigidbody2D.velocity = new Vector2(0, 0);
                    }
                    else if (Input.GetKeyDown(KeyCode.I))
                    {
                        Instantiate(rightskill, GameFunction_PVP.Instance.pos2 + new Vector3(20f, 0, 0), gameObject.transform.rotation);
                        playerAnimator.Play("skillR");
                        block = false;
                    }
                    else if (Input.GetKeyDown(KeyCode.O))
                    {
                        playerAnimator.Play("hitendR");
                        block = false;
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

                    if (GameFunction_PVP.Instance.p2hited)  //受到攻擊時的判定
                    {
                        CmdTellServerP2hited(false); //GameFunction_PVP.Instance.p2hited = false;

                        if (Input.GetKey(KeyCode.LeftArrow))
                        {
                            if (Input.GetKey(KeyCode.DownArrow)) playerAnimator.Play("downdefendR");
                            else playerAnimator.Play("defendR");
                            block = false;
                        }
                        else
                        {
                            if (Input.GetKey(KeyCode.DownArrow)) playerAnimator.Play("downhitedR");
                            else if (!jump)
                            {
                                GGrigidbody2D.velocity = new Vector2(-40f, GGrigidbody2D.velocity.y); //擊飛效果
                                playerAnimator.Play("hitdownR");
                            }
                            else playerAnimator.Play("hitedR");
                            block = false;
                            GameFunction_PVP.Instance.Score2();
                        }
                    }

                    //*********************************  傷害判定區域  end  **************************************

                }          //**********************************  面向右 end   ********************************************

                else
                {           //**********************************  面向左   ********************************************

                    //GameFunction_PVP.Instance.pos2 = gameObject.transform.position + new Vector3(-13f, 0, 0);// PLAYER 2  拳頭位子

                    if (Input.GetKey(KeyCode.DownArrow) && jump) //  ----------------------------------------------------------  S
                    {
                        if (Input.GetKey(KeyCode.LeftArrow))
                        {
                            GGrigidbody2D.velocity = new Vector2(0, 0);
                            if (GameFunction_PVP.Instance.p2hit)     //  並沒有被擊中   距離不夠  但還是會做出防禦動作
                            {
                                playerAnimator.Play("downdefendL");
                            }
                            else
                            {
                                GGrigidbody2D.velocity = new Vector2(0, 0);
                                if (down) playerAnimator.Play("downendL");  //  執行蹲下完成動作   避免整個蹲下動作一直輪迴
                                else playerAnimator.Play("downL");
                                block = false;
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.P))   //  蹲下攻擊
                        {
                            playerAnimator.Play("downhitL");
                            block = false;
                            CmdTellServerP1hit(true); //GameFunction_PVP.Instance.p1hit = true;
                            if (GameFunction_PVP.Instance.pos2.x - AttackDistance < GameFunction_PVP.Instance.pos1.x + 26f)
                            {
                                if (GameFunction_PVP.Instance.pos2.y > GameFunction_PVP.Instance.pos1.y - 10f)  //再次確認y軸  高度
                                    CmdTellServerP1hited(true); //GameFunction_PVP.Instance.p1hited = true;
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
                    else if (Input.GetKeyDown(KeyCode.P)) // -----------------------------------------------------  attack(含空中)
                    {
                        Instantiate(bools, GameFunction_PVP.Instance.pos2 - new Vector3(13f, 0, 0), gameObject.transform.rotation);

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
                        CmdTellServerP1hit(true); //GameFunction_PVP.Instance.p1hit = true;  //  通知 gamefuntion p1 在攻擊
                        if (GameFunction_PVP.Instance.pos2.x - AttackDistance < GameFunction_PVP.Instance.pos1.x + 26f)
                        {
                            if (GameFunction_PVP.Instance.pos2.y > GameFunction_PVP.Instance.pos1.y - 10f)  //再次確認y軸  高度
                                CmdTellServerP1hited(true); //GameFunction_PVP.Instance.p1hited = true;
                        }
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow)) //      ------------------------------------------------------------   A
                    {/*
                if (GameFunction_PVP.Instance.p2hit)
                {
                    if (jump) GGrigidbody2D.velocity = new Vector2(0, 0);
                    else GGrigidbody2D.velocity = new Vector2(Mathf.Sign(GGrigidbody2D.velocity.x) * jumpatt, GGrigidbody2D.velocity.y);
                    playerAnimator.Play("defendR");
                }
                else
                {*/
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
                    else if (Input.GetKey(KeyCode.RightArrow)) //        ------------------------------------------------------------  D
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
                        down = false;
                        playerAnimator.Play("waitL");
                    }
                    else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
                    {
                        if (jump)
                            GGrigidbody2D.velocity = new Vector2(0, 0);
                    }
                    else if (Input.GetKeyDown(KeyCode.I))
                    {
                        Instantiate(leftskill, GameFunction_PVP.Instance.pos2 - new Vector3(20f, 0, 0), gameObject.transform.rotation);
                        playerAnimator.Play("skillL");
                        block = false;
                    }
                    else if (Input.GetKeyDown(KeyCode.O))
                    {
                        playerAnimator.Play("hitendL");
                        block = false;
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

                    if (GameFunction_PVP.Instance.p2hited)  //受到攻擊時的判定
                    {
                        CmdTellServerP2hited(false); //GameFunction_PVP.Instance.p2hited = false;

                        if (Input.GetKey(KeyCode.RightArrow))
                        {
                            if (Input.GetKey(KeyCode.DownArrow)) playerAnimator.Play("downdefendL");
                            else playerAnimator.Play("defendL");
                            block = false;
                        }
                        else
                        {
                            if (Input.GetKey(KeyCode.DownArrow)) playerAnimator.Play("downhitedL");
                            else if (!jump)
                            {
                                GGrigidbody2D.velocity = new Vector2(40f, GGrigidbody2D.velocity.y); //擊飛效果
                                playerAnimator.Play("hitdownL");
                            }
                            else playerAnimator.Play("hitedL");
                            block = false;
                            GameFunction_PVP.Instance.Score2();
                        }
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
                    CmdTellServerP1hit(false); //GameFunction_PVP.Instance.p1hit = false;
                }

                if (animatorInfo.IsName("jumpendR2"))
                {
                    block = true;
                    CmdTellServerP1hit(false); //GameFunction_PVP.Instance.p1hit = false;
                }

                if (animatorInfo.IsName("waitL"))
                {
                    block = true;
                    CmdTellServerP1hit(false); //GameFunction_PVP.Instance.p1hit = false;
                }

                if (animatorInfo.IsName("jumpendL2"))
                {
                    block = true;
                    CmdTellServerP1hit(false); //GameFunction_PVP.Instance.p1hit = false;
                }


                //********************************** 面相右  屏蔽區域  *************************************

                if (rightface)
                {


                    if (animatorInfo.IsName("jumphitR"))     //   跳躍攻擊進行時若是著地    要中斷
                        if (jump)
                        {
                            block = true;
                            CmdTellServerP1hit(false); //GameFunction_PVP.Instance.p1hit = false;
                        }

                    //     -----------------------------------------------------------------

                    if (animatorInfo.IsName("hitdownendR2"))
                    {
                        playerAnimator.Play("hitdownendR2");//還沒著地前保持倒地動作
                        if (jump) playerAnimator.Play("hitdownendR");  //著地  執行起身動作

                        time += Time.deltaTime; //時間增加
                        if (time == 1f)
                            GGrigidbody2D.velocity = new Vector2(0, 0);
                        else if (time > 4f)
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

                    if (animatorInfo.IsName("downdefendR") && skillhited)
                    {
                        playerAnimator.Play("downdefendR");
                        time += Time.deltaTime; //時間增加
                        if (time > 4f)
                        {
                            skillhited = false;
                            time = 0;
                            block = true;
                        }

                    }
                    else if (animatorInfo.IsName("defendR") && skillhited)
                    {
                        playerAnimator.Play("defendR");
                        time += Time.deltaTime; //時間增加
                        if (time > 4f)
                        {
                            skillhited = false;
                            time = 0;
                            block = true;
                        }

                    }
                    else if (animatorInfo.IsName("defendR") || animatorInfo.IsName("downdefendR"))  //  若是確實被攻擊到了   要做到p2攻擊時間結束  無法提早動作
                                                                                                    //  進到這裡只有確實被攻擊到   block確實被屏蔽
                        if (!GameFunction_PVP.Instance.p1hit)
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
                            CmdTellServerP1hit(false); //GameFunction_PVP.Instance.p1hit = false;
                        }

                    //     -----------------------------------------------------------------

                    if (animatorInfo.IsName("hitdownendL2"))
                    {
                        playerAnimator.Play("hitdownendL2");//還沒著地前保持倒地動作

                        if (jump) playerAnimator.Play("hitdownendL");  //著地  執行起身動作

                        time += Time.deltaTime; //時間增加
                        if (time == 1f)
                            GGrigidbody2D.velocity = new Vector2(0, 0);
                        else if (time > 4f)
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

                    if (animatorInfo.IsName("downdefendL") && skillhited)
                    {
                        playerAnimator.Play("downdefendL");
                        time += Time.deltaTime; //時間增加
                        if (time > 4f)
                        {
                            skillhited = false;
                            time = 0;
                            block = true;
                        }

                    }
                    else if (animatorInfo.IsName("defendL") && skillhited)
                    {
                        playerAnimator.Play("defendL");
                        time += Time.deltaTime; //時間增加
                        if (time > 1f)
                        {
                            skillhited = false;
                            time = 0;
                            block = true;
                        }

                    }
                    else if (animatorInfo.IsName("defendL") || animatorInfo.IsName("downdefendL"))  //  若是確實被攻擊到了   要做到p2攻擊時間結束  無法提早動作
                                                                                                    //  進到這裡只有確實被攻擊到   block確實被屏蔽
                        if (!GameFunction_PVP.Instance.p1hit)
                            block = true;

                }
                //********************************** 面相左  屏蔽區域  end  *************************************
            } //  ! block  
        }
    } // update

    [Command]
    void CmdTellServerP2position()
    {
        GameFunction_PVP.Instance.pos2 = gameObject.transform.position;
    }

    [Command]
    void CmdTellServerP1hited(bool H)
    {
        GameFunction_PVP.Instance.p1hited = H;
    }

    [Command]
    void CmdTellServerP1hit(bool H)
    {
        GameFunction_PVP.Instance.p1hit = H;
    }

    [Command]
    public void CmdTellServerP2hited(bool H)
    {
        GameFunction_PVP.Instance.p2hited = H;
    }

    [Command]
    void CmdTellServerP2hit(bool H)
    {
        GameFunction_PVP.Instance.p2hit = H;
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
}