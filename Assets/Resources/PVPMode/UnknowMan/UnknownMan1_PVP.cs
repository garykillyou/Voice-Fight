using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Windows.Speech;

public class UnknownMan1_PVP : NetworkBehaviour
{

    public bool block = true;

    public GameObject darkskill;
    public GameObject darkbomb;
    public GameObject darkend;
    public float force = 10000;
    public float maxspeed = 50f;
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

    KeywordRecognizer m_KeywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    public bool voiceAtk = false;

    void Awake()
    {
        GGrigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {
            if (PhraseRecognitionSystem.isSupported)
            {
                keywords.Add("GG", () =>
                {
                    JumpCalled();
                });

                keywords.Add("white", () =>
                {
                    FireCalled();
                });

                m_KeywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
                m_KeywordRecognizer.OnPhraseRecognized += KeywordRecognizerOnPhraseRecognized;
                m_KeywordRecognizer.Start();
            }
        }
    }

    void KeywordRecognizerOnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    void GoCalled()
    {
        print("You just said GO !!!");
        voiceAtk = true;
    }

    void JumpCalled()
    {
        print("You just said Jump !!!");
        voiceAtk = true;
    }

    void FireCalled()
    {
        print("You just said Fire !!!");
        voiceAtk = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        playerAnimator = (Animator)GetComponent("Animator");
        animatorInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);//抓取現在再跑的動畫名

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
            if (block) CmdTellServerP1hited(true);//GameFunction_PVP.Instance.p1hited = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            playerAnimator = (Animator)GetComponent("Animator");
            animatorInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);//抓取現在再跑的動畫名
            // GameFunction_PVP.Instance.pos1 = gameObject.transform.position;//PLAYER1
            CmdTellServerP1position();

            //----------------------  跳躍   ----------------------

            if (gameObject.transform.position.y > 10)
            {
                GGrigidbody2D.AddForce(transform.up * -jumptforce);
            }

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
                time = 0; // 時間初始
                hittime = 0; // 連段初始
                hiting = false;
                CmdTellServerP1Up(false); //GameFunction_PVP.Instance.p1up = false;
                if (rightface)
                {      //**********************************  面相右  ********************************************

                    //GameFunction_PVP.Instance.pos1 = gameObject.transform.position + new Vector3(13f, 0, 0);//PLAYER1 

                    if (Input.GetKey(KeyCode.S) && jump) //  ----------------------------------------------------------  S
                    {
                        GGrigidbody2D.velocity = new Vector2(0, 0);
                        if (down)    //  執行蹲下"完成"動作   避免整個蹲下動作一直輪迴
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
                    else if (Input.GetKeyDown(KeyCode.F)) // -----------------------------------------------
                    {
                        if (jump)   //地面攻擊
                        {
                            GGrigidbody2D.velocity = new Vector2(0, 0);
                            playerAnimator.Play("hitR");
                        }
                        else
                        {   //  空中攻擊
                            playerAnimator.Play("hitR");
                        }
                        block = false;
                        CmdTellServerP1hit(true); //GameFunction_PVP.Instance.p1hit = true;  //  通知 gamefuntion p1 在攻擊

                        //Instantiate(darkskill, GameFunction_PVP.Instance.pos1 + new Vector3(20f, 0, 0), gameObject.transform.rotation);
                        CmdTellServerDarkskill(new Vector3(20f, 0, 0));
                    }
                    else if (Input.GetKeyDown(KeyCode.G)) // ------------------------------------------------
                    {
                        if (jump)   //地面攻擊
                        {
                            GGrigidbody2D.velocity = new Vector2(0, 0);
                            playerAnimator.Play("hitR");
                        }
                        else
                        {   //  空中攻擊
                            playerAnimator.Play("hitR");
                        }
                        block = false;
                        CmdTellServerP1hit(true); //GameFunction_PVP.Instance.p1hit = true;  //  通知 gamefuntion p1 在攻擊
                                                  //Instantiate(darkskill, GameFunction_PVP.Instance.pos1 + new Vector3(70f, 0, 0), gameObject.transform.rotation);
                        CmdTellServerDarkskill(new Vector3(70f, 0, 0));
                    }
                    else if (Input.GetKeyDown(KeyCode.H)) // ---------------------------------------------
                    {
                        if (jump)   //地面攻擊
                        {
                            GGrigidbody2D.velocity = new Vector2(0, 0);
                            playerAnimator.Play("hitR");
                        }
                        else
                        {   //  空中攻擊
                            playerAnimator.Play("hitR");
                        }
                        block = false;
                        CmdTellServerP1hit(true); //GameFunction_PVP.Instance.p1hit = true;  //  通知 gamefuntion p1 在攻擊
                                                  //Instantiate(darkskill, GameFunction_PVP.Instance.pos1 + new Vector3(120f, 0, 0), gameObject.transform.rotation);
                        CmdTellServerDarkskill(new Vector3(120f, 0, 0));
                    }
                    else if (voiceAtk) // ----------如果要載地板上才能用舊加上[&&jump]------------
                    {
                        CmdTellServerP1Score(-15);
                        voiceAtk = false;
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
                        CmdTellServerP1hited(false);//GameFunction_PVP.Instance.p1hited = false;

                        GGrigidbody2D.velocity = new Vector2(0, 0);

                        if (GameFunction_PVP.Instance.p2up)
                        {
                            GGrigidbody2D.velocity = new Vector2(-100f, GGrigidbody2D.velocity.y + 5f);
                            CmdTellServerP2Up(false); //GameFunction_PVP.Instance.p2up = false;
                        }
                        else if (Input.GetKey(KeyCode.A))
                        {
                            if (Input.GetKey(KeyCode.S) && skillhited) playerAnimator.Play("downdefendendR");
                            else if (Input.GetKey(KeyCode.S)) playerAnimator.Play("downdefendR");
                            else if (skillhited) playerAnimator.Play("defendendR");
                            else playerAnimator.Play("defendR");
                            block = false;
                            GGrigidbody2D.velocity = new Vector2(-10f, GGrigidbody2D.velocity.y + 5f);
                        }
                        else
                        {

                            if (!jump || GameFunction_PVP.Instance.p1hitended)// 包含重擊  擊飛
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
                            //GameFunction_PVP.Instance.Score1();
                            print("u1-1");
                            GameFunction_PVP.Instance.p1hited = false;
                            CmdTellServerP1hited(false);
                            CmdTellServerP1Score(10);
                        }
                        CmdTellServerP1hitended(false); //GameFunction_PVP.Instance.p1hitended = false;
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

                        if (down)    //  執行蹲下"完成"動作   避免整個蹲下動作一直輪迴
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
                    else if (Input.GetKeyDown(KeyCode.F))   // ---------------------------------------------
                    {
                        if (jump)   //地面攻擊
                        {
                            GGrigidbody2D.velocity = new Vector2(0, 0);
                            playerAnimator.Play("hitL");
                        }
                        else
                        {   //  空中攻擊
                            playerAnimator.Play("hitL");
                        }
                        block = false;
                        CmdTellServerP1hit(true); //GameFunction_PVP.Instance.p1hit = true;  //  通知 gamefuntion p1 在攻擊
                        //Instantiate(darkskill, GameFunction_PVP.Instance.pos1 + new Vector3(-120f, 0, 0), gameObject.transform.rotation);
                        CmdTellServerDarkskill(new Vector3(-120f, 0, 0));
                    }
                    else if (Input.GetKeyDown(KeyCode.G)) // ---------------------------------------------------
                    {
                        if (jump)   //地面攻擊
                        {
                            GGrigidbody2D.velocity = new Vector2(0, 0);
                            playerAnimator.Play("hitL");
                        }
                        else
                        {   //  空中攻擊
                            playerAnimator.Play("hitL");
                        }
                        block = false;
                        CmdTellServerP1hit(true); //GameFunction_PVP.Instance.p1hit = true;  //  通知 gamefuntion p1 在攻擊
                        //Instantiate(darkskill, GameFunction_PVP.Instance.pos1 + new Vector3(-70f, 0, 0), gameObject.transform.rotation);
                        CmdTellServerDarkskill(new Vector3(-70f, 0, 0));
                    }
                    else if (Input.GetKeyDown(KeyCode.H))   //--------------------------------------------------------
                    {
                        if (jump)   //地面攻擊
                        {
                            GGrigidbody2D.velocity = new Vector2(0, 0);
                            playerAnimator.Play("hitL");
                        }
                        else
                        {   //  空中攻擊
                            playerAnimator.Play("hitL");
                        }
                        block = false;
                        CmdTellServerP1hit(true); //GameFunction_PVP.Instance.p1hit = true;  //  通知 gamefuntion p1 在攻擊
                        //Instantiate(darkskill, GameFunction_PVP.Instance.pos1 + new Vector3(-20f, 0, 0), gameObject.transform.rotation);
                        CmdTellServerDarkskill(new Vector3(-20f, 0, 0));

                    }
                    else if (voiceAtk) // ----------如果要載地板上才能用舊加上[&&jump]------------
                    {
                        CmdTellServerP1Score(-15);
                        voiceAtk = false;
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
                        CmdTellServerP1hited(false);//GameFunction_PVP.Instance.p1hited = false;

                        GGrigidbody2D.velocity = new Vector2(0, 0);

                        if (GameFunction_PVP.Instance.p2up)
                        {
                            GGrigidbody2D.velocity = new Vector2(100f, GGrigidbody2D.velocity.y + 5f);
                            CmdTellServerP2Up(false); //GameFunction_PVP.Instance.p2up = false;
                        }
                        else if (Input.GetKey(KeyCode.D))
                        {
                            if (Input.GetKey(KeyCode.S)) playerAnimator.Play("downdefendendL");
                            else if (Input.GetKey(KeyCode.S)) playerAnimator.Play("downdefendL");
                            else if (skillhited) playerAnimator.Play("defendendL");
                            else playerAnimator.Play("defendL");
                            block = false;
                            GGrigidbody2D.velocity = new Vector2(10f, GGrigidbody2D.velocity.y + 5f);
                        }
                        else
                        {
                            if (!jump || GameFunction_PVP.Instance.p1hitended)// 包含重擊  擊飛
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
                            //GameFunction_PVP.Instance.Score1();
                            print("u1-2");
                            GameFunction_PVP.Instance.p1hited = false;
                            CmdTellServerP1hited(false);
                            CmdTellServerP1Score(10);
                        }
                        CmdTellServerP1hitended(false); //GameFunction_PVP.Instance.p1hitended = false;
                    }

                    //*********************************  傷害判定區域  end  **************************************

                }           //**********************************  面向左 end   ********************************************

            }   // --------------------------------------------- block
            else
            {
                //********************************** 初步屏蔽初始區域  *************************************
                if (animatorInfo.IsName("waitR"))
                {
                    block = true;
                    CmdTellServerP1hit(false); //GameFunction_PVP.Instance.p1hit = false;
                    if (skillhited) skillhited = false;
                }

                if (animatorInfo.IsName("waitL"))
                {
                    block = true;
                    CmdTellServerP1hit(false); //GameFunction_PVP.Instance.p1hit = false;
                    if (skillhited) skillhited = false;
                }

                if (animatorInfo.IsName("hitendR2") || animatorInfo.IsName("hitendL2"))
                {
                    block = true;
                    CmdTellServerP1hit(false); //GameFunction_PVP.Instance.p1hit = false;
                    if (skillhited) skillhited = false;
                }

                if (animatorInfo.IsName("hitedR") || animatorInfo.IsName("hitR2"))
                {
                    if (GameFunction_PVP.Instance.p1hited || GameFunction_PVP.Instance.p1hitended)  //受到攻擊時的判定
                    {

                        playerAnimator.Rebind();
                        CmdTellServerP1hited(false);//GameFunction_PVP.Instance.p1hited = false;

                        GGrigidbody2D.velocity = new Vector2(0, 0);

                        if (!jump || GameFunction_PVP.Instance.p1hitended)// 包含重擊  擊飛
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
                        //GameFunction_PVP.Instance.Score1();
                        print("u1-3");
                        GameFunction_PVP.Instance.p1hited = false;
                        CmdTellServerP1hited(false);
                        CmdTellServerP1Score(10);
                    }
                    CmdTellServerP1hitended(false); //GameFunction_PVP.Instance.p1hitended = false;
                }

                if (animatorInfo.IsName("hitedL") || animatorInfo.IsName("hitL2"))
                {
                    if (GameFunction_PVP.Instance.p1hited || GameFunction_PVP.Instance.p1hitended)  //受到攻擊時的判定
                    {

                        playerAnimator.Rebind();
                        CmdTellServerP1hited(false);//GameFunction_PVP.Instance.p1hited = false;

                        GGrigidbody2D.velocity = new Vector2(0, 0);

                        if (!jump || GameFunction_PVP.Instance.p1hitended)// 包含重擊  擊飛
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
                        //GameFunction_PVP.Instance.Score1();
                        print("u1-4");
                        GameFunction_PVP.Instance.p1hited = false;
                        CmdTellServerP1hited(false);
                        CmdTellServerP1Score(10);
                    }
                    CmdTellServerP1hitended(false); //GameFunction_PVP.Instance.p1hitended = false;
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
                        if (!GameFunction_PVP.Instance.p1hit)
                        {
                            jumpend = false;
                            playerAnimator.Play("jumpendR");
                        }
                        else jumpend = false;
                    }

                    //     -----------------------------------------------------------------

                    if (animatorInfo.IsName("downendR"))   //使動作卡在downend   不會再一次輪回到down   用down參數判斷
                    {
                        block = true;
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

                    //     --------------------------------  攻擊  ---------------------------------
                    if (animatorInfo.IsName("hitR2"))
                    {
                        //    第一次攻擊
                        if (Input.GetKeyDown(KeyCode.F) && !hiting)
                        {
                            hiting = true;
                            hittime = hittime + 1;
                            CmdTellServerP1hitTime();//GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                            playerAnimator.Rebind();
                            playerAnimator.Play("hitR2");
                            //Instantiate(darkbomb, GameFunction_PVP.Instance.pos1 + new Vector3(20f, 0, 0), gameObject.transform.rotation);
                            CmdTellServerDarkbomb(new Vector3(20f, 0, 0));
                            if (GameFunction_PVP.Instance.pos1.x + 20f > GameFunction_PVP.Instance.pos2.x - 15f &&
                                GameFunction_PVP.Instance.pos1.x + 20f < GameFunction_PVP.Instance.pos2.x + 15f)
                            {
                                if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                    GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                    CmdTellServerP2hited(true); //GameFunction_PVP.Instance.p2hited = true;
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.G) && !hiting)
                        {
                            hiting = true;
                            hittime = hittime + 1;
                            CmdTellServerP1hitTime();//GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                            playerAnimator.Rebind();
                            playerAnimator.Play("hitR2");
                            //Instantiate(darkbomb, GameFunction_PVP.Instance.pos1 + new Vector3(70f, 0, 0), gameObject.transform.rotation);
                            CmdTellServerDarkbomb(new Vector3(70f, 0, 0));
                            if (GameFunction_PVP.Instance.pos1.x + 70f > GameFunction_PVP.Instance.pos2.x - 15f &&
                                GameFunction_PVP.Instance.pos1.x + 70f < GameFunction_PVP.Instance.pos2.x + 15f)
                            {
                                if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                    GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                    CmdTellServerP2hited(true); //GameFunction_PVP.Instance.p2hited = true;
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.H) && !hiting)
                        {
                            hiting = true;
                            hittime = hittime + 1;
                            CmdTellServerP1hitTime();//GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                            playerAnimator.Rebind();
                            playerAnimator.Play("hitR2");
                            //Instantiate(darkbomb, GameFunction_PVP.Instance.pos1 + new Vector3(120f, 0, 0), gameObject.transform.rotation);
                            CmdTellServerDarkbomb(new Vector3(120f, 0, 0));
                            if (GameFunction_PVP.Instance.pos1.x + 120f > GameFunction_PVP.Instance.pos2.x - 15f &&
                                GameFunction_PVP.Instance.pos1.x + 120f < GameFunction_PVP.Instance.pos2.x + 15f)
                            {
                                if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                    GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                    CmdTellServerP2hited(true); //GameFunction_PVP.Instance.p2hited = true;
                            }
                        }
                        //--------------------------------------------   連擊   -----------------------------------
                        else if (hiting && hittime == 1)
                        {
                            //hiting = false;
                            CmdTellServerP1hit(true); //GameFunction_PVP.Instance.p1hit = true;  //  通知 gamefuntion p1 在攻擊
                            if (Input.GetKeyDown(KeyCode.F))
                            {
                                hiting = true;
                                hittime = hittime + 1;
                                CmdTellServerP1hitTime();//GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                                playerAnimator.Rebind();
                                playerAnimator.Play("hitR2");
                                //Instantiate(darkbomb, GameFunction_PVP.Instance.pos1 + new Vector3(20f, 0, 0), gameObject.transform.rotation);
                                CmdTellServerDarkbomb(new Vector3(20f, 0, 0));
                                if (GameFunction_PVP.Instance.pos1.x + 20f > GameFunction_PVP.Instance.pos2.x - 15f &&
                                    GameFunction_PVP.Instance.pos1.x + 20f < GameFunction_PVP.Instance.pos2.x + 15f)
                                {
                                    if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                        GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                        CmdTellServerP2hited(true); //GameFunction_PVP.Instance.p2hited = true;
                                }
                            }
                            else if (Input.GetKeyDown(KeyCode.G))
                            {
                                hiting = true;
                                hittime = hittime + 1;
                                CmdTellServerP1hitTime();//GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                                playerAnimator.Rebind();
                                playerAnimator.Play("hitR2");
                                //Instantiate(darkbomb, GameFunction_PVP.Instance.pos1 + new Vector3(70f, 0, 0), gameObject.transform.rotation);
                                CmdTellServerDarkbomb(new Vector3(70f, 0, 0));
                                if (GameFunction_PVP.Instance.pos1.x + 70f > GameFunction_PVP.Instance.pos2.x - 15f &&
                                    GameFunction_PVP.Instance.pos1.x + 70f < GameFunction_PVP.Instance.pos2.x + 15f)
                                {
                                    if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                        GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                        CmdTellServerP2hited(true); //GameFunction_PVP.Instance.p2hited = true;
                                }
                            }
                            else if (Input.GetKeyDown(KeyCode.H))
                            {
                                hiting = true;
                                hittime = hittime + 1;
                                CmdTellServerP1hitTime();//GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                                playerAnimator.Rebind();
                                playerAnimator.Play("hitR2");
                                //Instantiate(darkbomb, GameFunction_PVP.Instance.pos1 + new Vector3(120f, 0, 0), gameObject.transform.rotation);
                                CmdTellServerDarkbomb(new Vector3(120f, 0, 0));
                                if (GameFunction_PVP.Instance.pos1.x + 120f > GameFunction_PVP.Instance.pos2.x - 15f &&
                                    GameFunction_PVP.Instance.pos1.x + 120f < GameFunction_PVP.Instance.pos2.x + 15f)
                                {
                                    if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                        GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                        CmdTellServerP2hited(true); //GameFunction_PVP.Instance.p2hited = true;
                                }
                            }
                        }
                        else if (hiting && hittime == 2)
                        {
                            //hiting = false;
                            CmdTellServerP1hit(true); //GameFunction_PVP.Instance.p1hit = true;  //  通知 gamefuntion p1 在攻擊
                            if (Input.GetKeyDown(KeyCode.F))
                            {
                                hittime = hittime + 1;
                                CmdTellServerP1hitTime();//GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                                playerAnimator.Rebind();
                                playerAnimator.Play("hitR2");
                                //Instantiate(darkend, GameFunction_PVP.Instance.pos1 + new Vector3(20f, 0, 0), gameObject.transform.rotation);
                                CmdTellServerDarkend(new Vector3(20f, 0, 0));
                                if (GameFunction_PVP.Instance.pos1.x + 20f > GameFunction_PVP.Instance.pos2.x - 20f &&
                                    GameFunction_PVP.Instance.pos1.x + 20f < GameFunction_PVP.Instance.pos2.x + 20f)
                                {
                                    if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                        GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                        CmdTellServerP2hitended(true); //GameFunction_PVP.Instance.p2hitended = true;;
                                }
                            }
                            else if (Input.GetKeyDown(KeyCode.G))
                            {
                                hittime = hittime + 1;
                                CmdTellServerP1hitTime();//GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                                playerAnimator.Rebind();
                                playerAnimator.Play("hitR2");
                                //Instantiate(darkend, GameFunction_PVP.Instance.pos1 + new Vector3(70f, 0, 0), gameObject.transform.rotation);
                                CmdTellServerDarkend(new Vector3(70f, 0, 0));
                                if (GameFunction_PVP.Instance.pos1.x + 70f > GameFunction_PVP.Instance.pos2.x - 20f &&
                                    GameFunction_PVP.Instance.pos1.x + 70f < GameFunction_PVP.Instance.pos2.x + 20f)
                                {
                                    if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                        GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                        CmdTellServerP2hitended(true); //GameFunction_PVP.Instance.p2hitended = true;;
                                }
                            }
                            else if (Input.GetKeyDown(KeyCode.H))
                            {
                                hittime = hittime + 1;
                                CmdTellServerP1hitTime();//GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                                playerAnimator.Rebind();
                                playerAnimator.Play("hitR2");
                                //Instantiate(darkend, GameFunction_PVP.Instance.pos1 + new Vector3(120f, 0, 0), gameObject.transform.rotation);
                                CmdTellServerDarkend(new Vector3(120f, 0, 0));
                                if (GameFunction_PVP.Instance.pos1.x + 120f > GameFunction_PVP.Instance.pos2.x - 20f &&
                                    GameFunction_PVP.Instance.pos1.x + 120f < GameFunction_PVP.Instance.pos2.x + 20f)
                                {
                                    if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                        GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                        CmdTellServerP2hitended(true); //GameFunction_PVP.Instance.p2hitended = true;;
                                }
                            }
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
                            CmdTellServerP1hit(false); //GameFunction_PVP.Instance.p1hit = false;
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
                        if (!GameFunction_PVP.Instance.p1hit)
                        {
                            jumpend = false;
                            playerAnimator.Play("jumpendL");
                        }
                        else jumpend = false;
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
                        if (time > 1f)
                        {
                            skillhited = false;
                            time = 0;
                            block = true;
                        }

                    }

                    //     --------------------------------  連擊  ---------------------------------
                    if (animatorInfo.IsName("hitL2"))
                    {
                        //    第一次攻擊
                        if (Input.GetKeyDown(KeyCode.F) && !hiting)
                        {
                            hiting = true;
                            hittime = hittime + 1;
                            CmdTellServerP1hitTime();//GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                            playerAnimator.Rebind();
                            playerAnimator.Play("hitL2");
                            //Instantiate(darkbomb, GameFunction_PVP.Instance.pos1 + new Vector3(-120f, 0, 0), gameObject.transform.rotation);
                            CmdTellServerDarkbomb(new Vector3(-120f, 0, 0));
                            if (GameFunction_PVP.Instance.pos1.x - 120f > GameFunction_PVP.Instance.pos2.x - 15f &&
                                GameFunction_PVP.Instance.pos1.x - 120f < GameFunction_PVP.Instance.pos2.x + 15f)
                            {
                                if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                    GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                    CmdTellServerP2hited(true); //GameFunction_PVP.Instance.p2hited = true;
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.G) && !hiting)
                        {
                            hiting = true;
                            hittime = hittime + 1;
                            CmdTellServerP1hitTime();//GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                            playerAnimator.Rebind();
                            playerAnimator.Play("hitL2");
                            //Instantiate(darkbomb, GameFunction_PVP.Instance.pos1 + new Vector3(-70f, 0, 0), gameObject.transform.rotation);
                            CmdTellServerDarkbomb(new Vector3(-70f, 0, 0));
                            if (GameFunction_PVP.Instance.pos1.x - 70f > GameFunction_PVP.Instance.pos2.x - 15f &&
                                GameFunction_PVP.Instance.pos1.x - 70f < GameFunction_PVP.Instance.pos2.x + 15f)
                            {
                                if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                    GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                    CmdTellServerP2hited(true); //GameFunction_PVP.Instance.p2hited = true;
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.H) && !hiting)
                        {
                            hiting = true;
                            hittime = hittime + 1;
                            CmdTellServerP1hitTime();//GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                            playerAnimator.Rebind();
                            playerAnimator.Play("hitL2");
                            //Instantiate(darkbomb, GameFunction_PVP.Instance.pos1 + new Vector3(-20f, 0, 0), gameObject.transform.rotation);
                            CmdTellServerDarkbomb(new Vector3(-20f, 0, 0));
                            if (GameFunction_PVP.Instance.pos1.x - 20f > GameFunction_PVP.Instance.pos2.x - 15f &&
                                GameFunction_PVP.Instance.pos1.x - 20f < GameFunction_PVP.Instance.pos2.x + 15f)
                            {
                                if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                    GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                    CmdTellServerP2hited(true); //GameFunction_PVP.Instance.p2hited = true;
                            }
                        }
                        //--------------------------------------------   連擊   -----------------------------------
                        else if (hiting && hittime == 1)
                        {
                            hiting = false;
                            CmdTellServerP1hit(true); //GameFunction_PVP.Instance.p1hit = true;  //  通知 gamefuntion p1 在攻擊
                            if (Input.GetKeyDown(KeyCode.F))
                            {
                                hiting = true;
                                hittime = hittime + 1;
                                playerAnimator.Rebind();
                                playerAnimator.Play("hitL2");
                                //Instantiate(darkbomb, GameFunction_PVP.Instance.pos1 + new Vector3(-120f, 0, 0), gameObject.transform.rotation);
                                CmdTellServerDarkbomb(new Vector3(-120f, 0, 0));
                                if (GameFunction_PVP.Instance.pos1.x - 120f > GameFunction_PVP.Instance.pos2.x - 15f &&
                                    GameFunction_PVP.Instance.pos1.x - 120f < GameFunction_PVP.Instance.pos2.x + 15f)
                                {
                                    if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                        GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                        CmdTellServerP2hited(true); //GameFunction_PVP.Instance.p2hited = true;
                                }
                            }
                            else if (Input.GetKeyDown(KeyCode.G))
                            {
                                hiting = true;
                                hittime = hittime + 1;
                                CmdTellServerP1hitTime();//GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                                playerAnimator.Rebind();
                                playerAnimator.Play("hitL2");
                                //Instantiate(darkbomb, GameFunction_PVP.Instance.pos1 + new Vector3(-70f, 0, 0), gameObject.transform.rotation);
                                CmdTellServerDarkbomb(new Vector3(-70f, 0, 0));
                                if (GameFunction_PVP.Instance.pos1.x - 70f > GameFunction_PVP.Instance.pos2.x - 15f &&
                                    GameFunction_PVP.Instance.pos1.x - 70f < GameFunction_PVP.Instance.pos2.x + 15f)
                                {
                                    if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                        GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                        CmdTellServerP2hited(true); //GameFunction_PVP.Instance.p2hited = true;
                                }
                            }
                            else if (Input.GetKeyDown(KeyCode.H))
                            {
                                hiting = true;
                                hittime = hittime + 1;
                                CmdTellServerP1hitTime();//GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                                playerAnimator.Rebind();
                                playerAnimator.Play("hitL2");
                                //Instantiate(darkbomb, GameFunction_PVP.Instance.pos1 + new Vector3(-20f, 0, 0), gameObject.transform.rotation);
                                CmdTellServerDarkbomb(new Vector3(-20f, 0, 0));
                                if (GameFunction_PVP.Instance.pos1.x - 20f > GameFunction_PVP.Instance.pos2.x - 15f &&
                                    GameFunction_PVP.Instance.pos1.x - 20f < GameFunction_PVP.Instance.pos2.x + 15f)
                                {
                                    if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                        GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                        CmdTellServerP2hited(true); //GameFunction_PVP.Instance.p2hited = true;
                                }
                            }
                        }
                        else if (hiting && hittime == 2)
                        {
                            //hiting = false;
                            CmdTellServerP1hit(true); //GameFunction_PVP.Instance.p1hit = true;  //  通知 gamefuntion p1 在攻擊
                            if (Input.GetKeyDown(KeyCode.F))
                            {
                                hittime = hittime + 1;
                                CmdTellServerP1hitTime();//GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                                playerAnimator.Rebind();
                                playerAnimator.Play("hitL2");
                                //Instantiate(darkend, GameFunction_PVP.Instance.pos1 + new Vector3(-120f, 0, 0), gameObject.transform.rotation);
                                CmdTellServerDarkend(new Vector3(-120f, 0, 0));
                                if (GameFunction_PVP.Instance.pos1.x - 120f > GameFunction_PVP.Instance.pos2.x - 20f &&
                                    GameFunction_PVP.Instance.pos1.x - 120f < GameFunction_PVP.Instance.pos2.x + 20f)
                                {
                                    if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                        GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                        CmdTellServerP2hitended(true); //GameFunction_PVP.Instance.p2hitended = true;;
                                }
                            }
                            else if (Input.GetKeyDown(KeyCode.G))
                            {
                                hittime = hittime + 1;
                                CmdTellServerP1hitTime();//GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                                playerAnimator.Rebind();
                                playerAnimator.Play("hitL2");
                                //Instantiate(darkend, GameFunction_PVP.Instance.pos1 + new Vector3(-70f, 0, 0), gameObject.transform.rotation);
                                CmdTellServerDarkend(new Vector3(-70f, 0, 0));
                                if (GameFunction_PVP.Instance.pos1.x - 70f > GameFunction_PVP.Instance.pos2.x - 20f &&
                                    GameFunction_PVP.Instance.pos1.x - 70f < GameFunction_PVP.Instance.pos2.x + 20f)
                                {
                                    if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                        GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                        CmdTellServerP2hitended(true); //GameFunction_PVP.Instance.p2hitended = true;;
                                }
                            }
                            else if (Input.GetKeyDown(KeyCode.H))
                            {
                                hittime = hittime + 1;
                                CmdTellServerP1hitTime();//GameFunction.Instance.p1hittime = GameFunction.Instance.p1hittime + 1;
                                playerAnimator.Rebind();
                                playerAnimator.Play("hitL2");
                                //Instantiate(darkend, GameFunction_PVP.Instance.pos1 + new Vector3(-20f, 0, 0), gameObject.transform.rotation);
                                CmdTellServerDarkend(new Vector3(-20f, 0, 0));
                                if (GameFunction_PVP.Instance.pos1.x - 20f > GameFunction_PVP.Instance.pos2.x - 20f &&
                                    GameFunction_PVP.Instance.pos1.x - 20f < GameFunction_PVP.Instance.pos2.x + 20f)
                                {
                                    if (GameFunction_PVP.Instance.pos1.y > GameFunction_PVP.Instance.pos2.y - 20f &&
                                        GameFunction_PVP.Instance.pos1.y < GameFunction_PVP.Instance.pos2.y + 20f)  //再次確認y軸  高度
                                        CmdTellServerP2hitended(true); //GameFunction_PVP.Instance.p2hitended = true;;
                                }
                            }
                        }
                    }

                }
                //********************************** 面相左  屏蔽區域  end  *************************************
                if (animatorInfo.IsName("hitdownendR") || animatorInfo.IsName("hitdownendL"))
                {
                    if (GameFunction_PVP.Instance.pos1.x - 30f < GameFunction_PVP.Instance.pos2.x &&
                        GameFunction_PVP.Instance.pos1.x + 30f > GameFunction_PVP.Instance.pos2.x)
                    {
                        CmdTellServerP2hited(true); //GameFunction_PVP.Instance.p2hited = true;
                        CmdTellServerP1Up(true); //GameFunction_PVP.Instance.p1up = true;
                    }
                }
            } //  ! block  



        } // update
    }

    //===聯機專用===
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
    public void CmdTellServerP1Up(bool H)
    {
        GameFunction_PVP.Instance.p1up = H;
    }

    [Command]
    public void CmdTellServerP2Up(bool H)
    {
        GameFunction_PVP.Instance.p2up = H;
    }

    [Command]
    public void CmdTellServerDarkskill(Vector3 N)
    {
        GameObject skill = GameObject.Instantiate(darkskill, GameFunction_PVP.Instance.pos1 + N, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(skill);
    }

    [Command]
    public void CmdTellServerDarkbomb(Vector3 N)
    {
        GameObject skill = GameObject.Instantiate(darkbomb, GameFunction_PVP.Instance.pos1 + N, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(skill);
    }

    [Command]
    public void CmdTellServerDarkend(Vector3 N)
    {
        GameObject skill = GameObject.Instantiate(darkend, GameFunction_PVP.Instance.pos1 + N, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(skill);
    }

    [Command]
    public void CmdTellServerGameOver()
    {
        GameFunction_PVP.Instance.IsPlaying = false;
        GameObject.Find("CounterText").GetComponent<CounterText>().endCC = true;
    }
}