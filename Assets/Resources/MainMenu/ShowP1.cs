using UnityEngine;

public class ShowP1 : MonoBehaviour
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

    void Start()
    {

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
        }
    }


    // Update is called once per frame
    void Update()
    {
        playerAnimator = (Animator)GetComponent("Animator");
        animatorInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);//抓取現在再跑的動畫名

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (rightface) rightface = false;
            else rightface = true;
        }


        if (block)
        {
            time = 0; // 時間初始
            hittime = 0; // 連段初始

            //GameFunction.Instance.pos1 = gameObject.transform.position + new Vector3(13f, 0, 0);//PLAYER1 

            if (Input.GetKey(KeyCode.S) && jump) //  ----------------------------------------------------------  S
            {
                GGrigidbody2D.velocity = new Vector2(0, 0);
                if (Input.GetKey(KeyCode.D)) // ------------------------------------------------------  技能
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        Instantiate(rightskill, gameObject.transform.position + new Vector3(20f, 0, 0), gameObject.transform.rotation);
                        playerAnimator.Play("skillR");
                        block = false;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.G))   //  蹲下攻擊
                {
                    playerAnimator.Play("downhitR");
                    block = false;
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
            else if (Input.GetKeyDown(KeyCode.G)) // -----------------------------------------------------  attack(含空中)
            {
                //Instantiate(bools, GameFunction.Instance.pos1 + new Vector3(13f, 0, 0), gameObject.transform.rotation);
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
            }
            else if (Input.GetKeyDown(KeyCode.F) && jump) // ------------------------------------------------------------  重擊
            {
                if (jump) GGrigidbody2D.velocity = new Vector2(0, 0);
                playerAnimator.Play("hitendR");
                block = false;
            }
            else if (Input.GetKeyDown(KeyCode.Z)) // ----------如果要載地板上才能用舊加上[&&jump]------------
            {
                //********************************************
                //********************************************
                //********************************************
                //********************************************
                //********************************************
                //********************************************
                //********************************************
                //********************************************
                //********************************************
                //********************************************
            }
            else if (Input.GetKey(KeyCode.A) && jump) //      ------------------------------------------------------------   A
            {
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

            //***********************************  衝刺  *******************************************

            if (Input.GetKey(KeyCode.H) && speedtime < 0.30f)
            {
                speedtime += Time.deltaTime; ;
                maxspeed = 120f;
            }
            else if (Input.GetKeyUp(KeyCode.H) || speedtime >= 0.30f)
            {
                if (Input.GetKeyUp(KeyCode.H)) speedtime = 0;
                maxspeed = 40f;
            }



        }   // --------------------------------------------- block
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
                }
                else if (hiting && hittime == 2)
                {
                    hiting = false;
                    playerAnimator.Play("hitendR");
                }
                else
                {
                    block = true;
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
                }
                else if (hiting && hittime == 2)
                {
                    hiting = false;
                    playerAnimator.Play("hitendL");
                    //------  攻擊判斷  -----
                }
                else
                {
                    block = true;
                    if (skillhited) skillhited = false;
                }

            }

            if (animatorInfo.IsName("hitendR2") || animatorInfo.IsName("hitendL2"))
            {
                block = true;
                if (skillhited) skillhited = false;
            }


            //********************************** 面相右  屏蔽區域  *************************************

            if (rightface)
            {


                if (animatorInfo.IsName("jumphitR"))     //   跳躍攻擊進行時若是著地    要中斷
                    if (jump)
                    {
                        block = true;
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
                    }
                    else if (hiting && hittime == 2)
                    {
                        hiting = false;
                        playerAnimator.Play("hitendR");
                        //------  攻擊判斷  -----
                    }
                    else
                    {
                        block = true;
                        down = true;
                    }
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
                    if (Input.GetKeyDown(KeyCode.G) && !hiting)
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
                    }
                    else if (hiting && hittime == 2)
                    {
                        hiting = false;
                        playerAnimator.Play("hitendL");
                        //------  攻擊判斷  -----
                    }
                    else block = true;
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
                    if (Input.GetKeyDown(KeyCode.G) && !hiting)
                    {
                        hiting = true;
                        hittime = hittime + 1;
                    }
                }

            }
            //********************************** 面相左  屏蔽區域  end  *************************************


        } //  ! block  

        //----------------------  跳躍   ----------------------

        if (gameObject.transform.position.y > 10)
        {
            GGrigidbody2D.AddForce(transform.up * -jumptforce);
        }

    } // update
}
