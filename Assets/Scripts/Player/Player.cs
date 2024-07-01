using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class Player : MonoBehaviour
{
    [SerializeField] private int playerNumber = 1;          // プレイヤーの番号
    
    //Move
    //-------------------------------------------------
    [SerializeField] float moveSpeed = 3;                   // 移動速度
    [SerializeField] float MmgnificationRun = 1.5f;         // 走る際の倍率
    private float moveSpeedResult;                          // 移動速度計算結果
    private bool orientation;                               // 向き  true:左　false:右
    //private bool canMove;                                 // 動けるか
    //-------------------------------------------------

    Scaffold_p scaffold;

    //Jump
    //-----------------------------------
    private Rigidbody2D playerRb;               // Rigidbody
    [SerializeField] float jumpPower = 5;       // ジャンプ力
    [SerializeField] float fallPower = 2f;      // 落下速度
    [SerializeField] int jumpLimit = 2;         // ジャンプ制限数
    private int jumpCount;                      // ジャンプ回数
    private float jumpCoolTime;                 // 次のジャンプまでのクールタイム
    private bool coolTimeStart;                 // ジャンプクールタイムが始まるか
    private bool fallStart;                     // 落下開始したか
    private bool fly;                           // 飛んでいるか
    private bool addFall;                       // 落下加速
    //-----------------------------------

    //Size
    //-----------------------------------
    private CapsuleCollider2D playerCol;        // プレイヤーのコリジョン
    private float MaxYSize;                     // プレイヤーの最大Yサイズ
    //-----------------------------------

    //--------------------------------------------
    [Header("キーコンフィグ")]
    [SerializeField] string[] GamepadButton = new string[4] {"A","B","X","Y"};
    //--------------------------------------------

    //--------------------------------------------
    [Header("TagName")]
    [SerializeField] string Floor = "Floor";
    [SerializeField] string Scaffold = "Scaffold";
    //[SerializeField] string someone = "";
    //--------------------------------------------

    //--------------------------------------------
    GameObject manager;
    PlayerManager playerManagerSclipt;
    //--------------------------------------------

    // Animation
    Animator animator;

    // ノックバック
    DamegeAction damegeAction;

    [Header("Magic")]
    [SerializeField] GameObject magicPrehubNomal;
    [SerializeField] GameObject magicPrehubTop;
    [SerializeField] GameObject magicPrehubDown;

    float[] coolTimeCounter = new float[3];
    bool[] coolDown = new bool[3];        // クールタイムに入ったか

    //当たり判定を切るため
    private Collider2D m_Col;

    [Header("Effect")]
    [SerializeField] private GameObject jumpEffectPrefub;
    private GameObject jumpEffect;

    // 倒された後操作不能にするためのフラグ（岩本）
    // PlayerDB内のMyInAliveで行う

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Manager");

        playerManagerSclipt = manager.GetComponent<PlayerManager>();
        playerNumber = GetComponent<PlayerDB>().MyNumber + 1;
        playerRb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<CapsuleCollider2D>();
        scaffold = GetComponent<Scaffold_p>();
        animator = GetComponent<Animator>();
        damegeAction = GetComponent<DamegeAction>();

        moveSpeedResult = moveSpeed;
        MaxYSize = playerCol.size.y;
        jumpCoolTime = 0.0f;
        jumpCount = 0;

        addFall = false;
        fallStart = false;
        coolTimeStart = false;
        fly = false;
        coolDown[0] = coolDown[1] = coolDown[2] = false;
    }

    // Update is called once per frame
    void Update()
    {
        // コントローラーの確認
        //Controoller();

        // 以下の処理はプレイヤーが生存している間行う（岩本）
        // 死亡時に移動や魔法の処理を行わないようにする
        if(GetComponent<PlayerDB>().MyIsAlive)
        {
            // 移動処理
            Move();
            // 左右の向き
            CangeScale(this.gameObject);
            // ジャンプ処理
            Jump();
            // 魔術処理
            Magic(magicPrehubNomal, GamepadButton[1], 0);
            Magic(magicPrehubTop, GamepadButton[2], 1);
            Magic(magicPrehubDown, GamepadButton[3], 2);
        }        
    }
    /*
    void Controoller()
    {
        // 接続されているコントローラの名前を調べる
        var controllerNames = Input.GetJoystickNames();

        // 自分の番号のコントローラが接続されていなければエラー
        if (controllerNames[playerNumber - 1] == "")
        {
            Debug.Log("Error" + playerNumber);
        }
    }
    */
    void Move()
    {
            // 入力された移動方向のベクトル
            Vector2 moveDirection = new Vector2(Input.GetAxis("Player" + playerNumber + "Horizontal"), -Input.GetAxis("Player" + playerNumber + "Vertical"));
        if (moveDirection.x > 0.2 || moveDirection.x < -0.2)
        {
            // 左右判定
            orientation = moveDirection.x > 0 ? true : false;

            // ダッシュ
            if(moveDirection.x > 0.9 || moveDirection.x < -0.9)
            {
                moveSpeedResult = moveSpeed * MmgnificationRun;
            }
            else
            {
                moveSpeedResult = moveSpeed;
            }

            // X移動
            transform.Translate(moveDirection.x * moveSpeedResult * Time.deltaTime, 0.0f, 0.0f);
            // アニメーション歩き　開始
            animator.SetBool("walk", true);
        }
        else
        {
            // アニメーション歩き　停止
            animator.SetBool("walk", false);
        }
            // crouch
            if (moveDirection.y < -0.1)
            {
                // ジャンプ時
                if(fly)
                {
                    //　落下速度上昇
                    if (!addFall)
                    {
                        playerRb.velocity += Vector2.down * fallPower*5;
                    }
					addFall = true;
                }
                // 地上
                else
                {
                    // 床抜ける
                    scaffold.SetThrough(true);

                    animator.SetFloat("speed", 2f);
                    // しゃがみアニメーション 開始
                    animator.SetBool("crouch", true);
                    // コリジョンのYを半分に
                    playerCol.size = new Vector2(playerCol.size.x, MaxYSize/2);
                    //Debug.Log("しゃがんでるよ");
                }
            }
            else
            {
            // 最大サイズより小さかったら
            if (playerCol.size.y < MaxYSize)
            {
                // 最大サイズに
                playerCol.size = new Vector2(playerCol.size.x, MaxYSize);
            }
            // しゃがみアニメーション 停止
            animator.SetBool("crouch", false);
            }
        //Debug.Log(moveDirection);
    }
    void Jump()
    {
        // ジャンプ制限数未満かつクールタイムが終わっていたら
        if (jumpCount < jumpLimit && !coolTimeStart)
        {
            // PressdButton A
            if (Pressd(GamepadButton[0]))
            {
                // エフェクトが生成されている場合は１度破棄する
                if (jumpEffect != null) Destroy(jumpEffect);
                // ジャンプエフェクト生成
                jumpEffect = Instantiate(jumpEffectPrefub, transform);
                // 上方向に力をかけジャンプ
                playerRb.velocity = Vector2.up * jumpPower;
                // カウント増加
                jumpCount++;
                // クールタイム
                coolTimeStart = true;
                // 落下速度上昇スイッチを切る
                addFall = false;
                fallStart = false;
                // ジャンプ時移動速度半減
                moveSpeedResult = moveSpeed * 0.5f;
                //Debug.Log("じゃんぷするよ");
            }
        }
        // 二度目のジャンプのクールタイム
        if (coolTimeStart)
        {
            jumpCoolTime -= Time.deltaTime;
            // カウント終了時
            if (jumpCoolTime < 0.0f)
            {
                // スイッチを切る
                coolTimeStart = false;
                jumpCoolTime = 0.3f;
            }
        }

        // 落下速度上昇
        if (playerRb.velocity.y < 0.6f　&& !fallStart)
        {
            playerRb.velocity = Vector2.down * fallPower;
            fallStart = true;
        }
    }
    void Magic(GameObject magicPrehub,string GameButton,int magicNumber)
    {
        // クールタイム
        if (coolDown[magicNumber])
        {
            coolTimeCounter[magicNumber] -= Time.deltaTime;
            if (coolTimeCounter[magicNumber] <= 0.0f)
            {
                coolDown[magicNumber] = false;
            }
        }
        // pressdButton& クールタイムがあけている
        if (Pressd(GameButton) && !coolDown[magicNumber])
        {
            // オフセット位置設定
            Vector2 OffsetPos = transform.position;
            Vector2 magicPosition = magicPrehub.GetComponent<Magic>().GetMagicPosition();
            OffsetPos += magicPosition;
            //向きに合わせて生成位置を変更
            //左向き
            if (!orientation)
            {
                OffsetPos.x = OffsetPos.x - (magicPosition.x * 2);
            }
            // 生成
            GameObject magic = Instantiate(magicPrehub, OffsetPos, transform.rotation);
            //各値を渡す
            magic.GetComponent<Magic>().setPlayerObj(gameObject);
            magic.GetComponent<Magic>().SetOrientation(orientation);
            magic.GetComponent<Magic>().setPNum(playerNumber);
            //自身の物に当たらないようにするため当たり判定を切る
            m_Col = magic.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(playerCol, m_Col, true);
            // 自身の向き変更
            CangeScale(magic);
            // coolDown開始
            coolDown[magicNumber] = true;
            coolTimeCounter[magicNumber] = magicPrehubNomal.GetComponent<Magic>().GetCoolTime();
        }
    }
    void CangeScale(GameObject target)
    {
        // targetのスケールを取得
        Vector3 cangeScale = target.transform.localScale;
        // 一旦プラスに補正
        if(cangeScale.x < 0) { cangeScale.x *= -1; }
        // スケール変更
        cangeScale.x = !orientation ? cangeScale.x : cangeScale.x * -1;
        target.transform.localScale = cangeScale;
    }

    public bool Pressd(string _str) { return Input.GetButtonDown("Player" + playerNumber + _str); }

    public bool Release(string _str) { return Input.GetButtonUp("Player" + playerNumber + _str); }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(Floor) || collision.gameObject.CompareTag(Scaffold))
        {
            jumpCount = 0;
            moveSpeedResult = moveSpeed;
            if (jumpEffect != null) Destroy(jumpEffect);
        }
        //Debug.Log(collision.gameObject.tag);
    }

	private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Floor) || collision.gameObject.CompareTag(Scaffold))
        {
            fly = false;
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Magic"))
        {
            //Debug.Log("魔術と当たった！");
            playerManagerSclipt.GetComponent<PlayerManager>().Hit(this.gameObject, collision.gameObject.GetComponent<Magic>().GetPower());
            // ノックバック
            int moveOrientation = collision.gameObject.GetComponent<Magic>().GetOrientation() ? 1 : -1; // 右か左か
            StartCoroutine(damegeAction.KnockBack(this.gameObject, moveOrientation* collision.gameObject.GetComponent<Magic>().GetPower()/2, 0.1f));
            //Magic.csに一旦移す
            //Destroy(collision);

            // 魔術と当たりHPがなくなった時、自分の名前と誰にやられたかを別スクリプトに格納
            if(GetComponent<PlayerDB>().MyHp <= 0　&& GetComponent<PlayerDB>().MyIsAlive)
            {
                GameObject.Find("Manager").GetComponent<PlayerDefeaterManager>().VictimRecord = gameObject.name;
                GameObject.Find("Manager").GetComponent<PlayerDefeaterManager>().DefeaterRecord = collision.GetComponent<Magic>().GetPName();
            }

        }
        if(collision.gameObject.CompareTag("Abyss"))
        {
            //Debug.Log("奈落");
            playerManagerSclipt.GetComponent<PlayerManager>().PlayerDestroy(gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        fly = true;
    }

    // Set Function
    // At the time of instantiate player
    public void SetPlayerNumber(int _playerNumber) { playerNumber = _playerNumber+1; }

    //Get Function
    public bool GetOrientation() { return orientation; }
    public string GetButton(int i) { return GamepadButton[i]; }
    public int GetPNum() { return playerNumber; }

    public bool isJumping() { return fly; }
}
