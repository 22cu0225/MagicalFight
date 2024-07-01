using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MidResultManager : MonoBehaviour
{
    // 変数宣言
    // -------------------------------------------------------------------------------------------------------------------

    // プレイヤーオブジェクトの取得用変数
    [HideInInspector] public List<GameObject> pObj;
    // 中間リザルトの表示オブジェクトの取得用変数
    //[HideInInspector] public GameObject midObj;

    // 中間リザルトで各プレイヤー勝利の表示オブジェクト取得用変数
    [HideInInspector] public List<GameObject> winnnerObj;

    // 中間リザルトで表示する引き分け用オブジェクト変数
    [HideInInspector] public GameObject DrowObj;

    // 各スクリプト取得用
    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public InstanceObjectManager instanceObjectManager;
    [HideInInspector] public PlayerDefeaterManager playerDefeaterManager;

    // 複数取得するもの
    [HideInInspector] public List<PlayerDB> playerDB;
    [HideInInspector] public List<SpriteRenderer> spriteRenderer;
    [HideInInspector] public List<Animator> animator;
    [HideInInspector] public List<PlayerDeadProcessing> playerDeadProcessing;
    [HideInInspector] public List<Scaffold_p> scaffold_p;

    

    // 現ゲームの参加人数をインゲーム開始時に入れておく変数
    private int PlayerMemberNum;

    // プレイヤーの生存数及び生存しているプレイヤーの番号と生存確認用変数
    private int PlayerAliveCounter;
    private int PlayerAliveNum;
    [HideInInspector] public List<bool> PlayerAliveChack;

    // 各プレイヤーの開始時の座標を入れておく変数(インゲームリトライ処理時にプレイヤーをスタート位置に配置するため)
    [HideInInspector] public List<Vector3> PlayerStartPos;     

    // 中間リザルト開始フラグ
    private bool MidResultFlag = false;

    // プレイヤーデータ取得確認用フラグ
    private bool GetPlayerDBChack = false;

    // 中間リザルト処理を重複して行わないようにするための確認用フラグ
    private bool MidResultEndFlag = false;

    // 中間リザルトの表示時間
    [SerializeField] private float MidResultTime;


    // (仮)
    //[SerializeField,Header("最大勝利数の仮置き")] private int WinPoint;

    // -------------------------------------------------------------------------------------------------------------------
    // 関数
    // -------------------------------------------------------------------------------------------------------------------

    // インゲームのリトライ処理
    void InGameRetry()
    {
        // プレイヤー数毎に行うリトライ処理
        for(int i = 0; i < PlayerMemberNum; i++)
        {
            pObj[i].SetActive(true);                            // 死亡時、非表示にされたプレイヤーオブジェクトを表示する（SetActiveをtrueに設定）

            // 死亡アニメーション周りのフラグ設定 ----------------------
            
            animator[i].SetBool("dead", false);                 // アニメーションフラグ：死亡アニメーションを切る

            playerDeadProcessing[i].PlayerDestroy = false;

            playerDeadProcessing[i].Alpha = 1.0f;               // プレイヤーオブジェクトの不透明度を戻す（透明じゃなくす）

            spriteRenderer[i].color = new Color(spriteRenderer[i].color.r, spriteRenderer[i].color.g, spriteRenderer[i].color.b, 1.0f);

            // -------------------------------------------------------

            playerDB[i].SetIsAlive(true);                       // PlayerDB内の生存判定をtrueに戻す

            playerDB[i].SetHp(playerDB[i].MyMaxHp);             // プレイヤーのHPを初期値に戻す

            pObj[i].transform.position = PlayerStartPos[i];     // プレイヤーの位置を初期地点に戻す

            scaffold_p[i].SetActive_Sca(false);                 // すり抜け床の処理に使っている変数の初期化

            scaffold_p[i].SetOn_Sca(false);                     // すり抜け床の処理に使っている変数の初期化

            PlayerAliveChack[i] = true;                         // 生存判定フラグをtrueに戻す

            playerDB[i].MyHpBar.SetActive(true);                // 各プレイヤーに対応するHPバーのSetActiveをtrueにする（表示されるようにする）

            playerDB[i].MyHpBar.GetComponent<Slider>().value = 1; //hpバーの表示を更新

        }

        // シーン全体のリトライ処理
        // 中間リザルトの表示を消す
        if(PlayerAliveNum != -1)
		{
            instanceObjectManager.ObjectSetActive(winnnerObj[PlayerAliveNum], false);
        }
		else    // 引き分けの場合
		{
            instanceObjectManager.ObjectSetActive(DrowObj, false);
		}

        // 各種フラグと値を初期化
        PlayerAliveCounter = PlayerMemberNum;
        MidResultFlag = false;
        MidResultEndFlag = false;
        PlayerAliveNum = 0;

        // リトライ処理を繰り返し呼び出されないようにするためInvokeをキャンセル
        CancelInvoke();

        // 既定の勝利回数を満たしたプレイヤーが存在する場合                
        for(int i = 0; i < PlayerMemberNum; i++)
        {
            // 各プレイヤーの勝利回数をログに出力
            Debug.Log("Player" + i + ":winpoint = " + playerDB[i].MyWinPoints);

            // GameManager内の最大勝利数の値以上に、いずれかのプレイヤーの勝利数が到達したら、そのプレイヤーの要素番号をSetSingleton内の変数に格納し、シーンを遷移させる
            if (playerDB[i].MyWinPoints >= gameManager.GetMaxWinPoints())
            {
                // 勝利プレイヤーの撃破数を数える(別スクリプトの関数を起動)
                playerDefeaterManager.KillCountProcessing(pObj[i].name);

                GameObject.Find("SceneManager").GetComponent<SetSingleton>().SetWinnerNum(i);
                GameObject.Find("SceneManager").GetComponent<SceneLoadManager>().WinnerFlag = true;

                break;
            }
        }
    }
    
    // 中間リザルト処理
    void MidResult()
    {
        // 中間リザルト処理が行われていない場合
        if (MidResultEndFlag == false)
        {            
            Debug.Log("MidResult");

            // 生存しているプレイヤーの要素番号を取得する
            PlayerAliveNum = GetPlayerAliveNum();

            if(PlayerAliveNum != -1)
			{
                // 生存しているプレイヤーに勝ち点を１付与する
                playerDB[PlayerAliveNum].SetWinPoints(playerDB[PlayerAliveNum].MyWinPoints + 1);

                // 中間リザルトの表示
                instanceObjectManager.ObjectSetActive(winnnerObj[PlayerAliveNum], true);
            }
			else
			{
                // 引き分けの表示をする
                instanceObjectManager.ObjectSetActive(DrowObj, true);
			}



            // ゲームマネージャー側の中間リトライ処理を行う
            //gameManager.MidResult();
            
            // 中間リザルト処理完了フラグをtrueに設定
            MidResultEndFlag = true;
        }

        // Invokeを使用し、4秒後にインゲームリトライ処理を行う
        Invoke("InGameRetry", MidResultTime);

    }

    // 各プレイヤー生存確認処理
    void PlayerChack()
    {
        for(int i = 0; i < PlayerMemberNum; i++)    // プレイヤー参加数分現在生存しているかを確認する
        {
            if(playerDB[i].MyHp <= 0 && PlayerAliveChack[i] == true)    // 現在生存しておりかつ、HPが０になったプレイヤーの場合
            {
                // 生存数カウント変数をデクリメントし、この要素番号のプレイヤーオブジェクトの生存フラグをfalseにする
                PlayerAliveCounter--;
                PlayerAliveChack[i] = false;

                Debug.Log("Player" + i + "：isDead");
            }
        }

        if(PlayerAliveCounter <= 1)       // 生存しているプレイヤーの数が1人以下になった場合
        {
            MidResultFlag = true;       // 中間リザルト処理開始フラグを上げる
        }
    }

    // 生き残ったプレイヤーの要素番号を戻り値として返す処理
    int GetPlayerAliveNum()
    {
        // プレイヤー数の分だけ繰り返し、生存しているプレイヤーがいたら戻り値として要素番号を返す
        for (int i = 0; i < PlayerMemberNum; i++)
        {
            if(PlayerAliveChack[i] == true)
            {
                return i;
            }
        }
        // 仮に生存しているプレイヤーがいなかった場合は-1を返す
        return -1;
    }

    // プレイヤーオブジェクトと各プレイヤーの情報を管理するスクリプトと必要な情報を取得
    void GetPlayersComponent()
    {
        // ---------------------------------------------------------------------------------------
        // プレイヤーオブジェクトの名称変更に伴い、プレイヤーオブジェクトの取得タイミングと処理を変更
        pObj.Add(GameObject.Find("TaikiAka_0(Clone)"));
        pObj.Add(GameObject.Find("TaikiAo_0(Clone)"));
        pObj.Add(GameObject.Find("TaikiKiro_0(Clone)"));
        pObj.Add(GameObject.Find("TaikiMidori_0(Clone)"));
        // ---------------------------------------------------------------------------------------

        // プレイヤーの参加人数に応じて
        for (int i = 0; i < PlayerMemberNum; i++)
        {
            Debug.Log("forChack：" + i);

            //pObj.Add(GameObject.Find("Player(Clone)"));             // Player(Clone)という名前のオブジェクトを探し、取得する（プレイヤーオブジェクトの取得）

            pObj[i].name = "Player" + i;                            // 名前でプレイヤーオブジェクトの取得をする際に重複しないようにプレイヤーオブジェクトの名前を変更する（要素番号を振る）

            if(pObj[i] != null)
            {
                Debug.Log("pObj[i]_NULLCheck = true");

                // 各プレイヤーにアタッチされたスクリプトを取得する
                playerDB.Add(pObj[i].GetComponent<PlayerDB>());                         // i番目のプレイヤーオブジェクトの PlayerDB スクリプトを取得する
                spriteRenderer.Add(pObj[i].GetComponent<SpriteRenderer>());             // スプライトレンダラーを取得
                animator.Add(pObj[i].GetComponent<Animator>());                         // アニメーターを取得
                playerDeadProcessing.Add(pObj[i].GetComponent<PlayerDeadProcessing>()); // 死亡時アニメーション等処理取得
                scaffold_p.Add(pObj[i].GetComponent<Scaffold_p>());                     // すり抜け床処理取得

                PlayerStartPos.Add(pObj[i].transform.position);         // i番目のプレイヤーのスタート時点の座標を取得する ← この行でエラーが発生中（NullReferenceException: Object reference not set to an instance of an object）
                                                                        //                                               ↑ 解決：PlayerStartPosのアクセス指定子をpublicに変更

                PlayerAliveChack.Add(true);                             // i番目のプレイヤーの生存判定フラグをtrueに設定する

                Debug.Log("SetPlayer" + i + "PlayerDB");
            }            
        }

        // プレイヤー生存カウンター変数の値に、参加人数を代入する
        PlayerAliveCounter = PlayerMemberNum;

        // プレイヤー情報設定確認フラグをtrueにする
        GetPlayerDBChack = true;
    }

    // -------------------------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    // 開始時処理
    void Start()
    {
        // ゲームマネージャースクリプトを取得
        gameManager = GetComponent<GameManager>();
        // オブジェクト生成スクリプトを取得
        instanceObjectManager = GetComponent<InstanceObjectManager>();
        // 撃破者と撃破数の管理スクリプト取得
        playerDefeaterManager = GetComponent<PlayerDefeaterManager>();

        // プレイヤー生存数確認用変数の値に、参加人数分の値を代入
        PlayerMemberNum = gameManager.playerCount;

        Debug.Log("PlayerCount = " + gameManager.playerCount);
        Debug.Log("PlayerMemberNum = " + PlayerMemberNum);

        // 中間表示のためのオブジェクトを生成しておく
        instanceObjectManager.InstanceObject();

        // 中間リザルトで表示するオブジェクトのActive状態の操作を簡単にするため、生成したオブジェクトを取得
        //midObj = instanceObjectManager.GetInstansedObject(0);

        // 中間リザルトで表示するオブジェクトのActive状態の操作を簡単にするため、生成した各オブジェクトを取得
        for(int i = 0; i < PlayerMemberNum; i++)
        {
            winnnerObj.Add(instanceObjectManager.GetInstansedObject(i));
        }

        // 中間リザルトで表示する引き分け時のオブジェクトを取得
        DrowObj = instanceObjectManager.GetInstansedObject(4);

        // 中間リザルト時間が未設定の場合、初期値として４秒を入れる
        if(MidResultTime <= 0)
        {
            MidResultTime = 4.0f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤー参加数と各プレイヤー情報の設定（初回処理）
        if(GetPlayerDBChack == false)
        {
            GetPlayersComponent();
        }

        // プレイヤーの生存状況の監視
        PlayerChack();

        // 中間リザルト開始フラグがたったら中間リザルト処理
        if(MidResultFlag == true)
        {
            MidResult();
        }

        // --------------------------------------------------------------------------------------------------

        // デバッグ用(左シフトを押しながら対応するキーを入力で処理実行)
        if(Input.GetKey(KeyCode.LeftShift))
        {
            // プレイヤー１のHPを０にする
            if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.Log("Input_K");
                pObj[0].GetComponent<PlayerDB>().SetHp(0);
                Debug.Log("Player0_HP:" + pObj[0].GetComponent<PlayerDB>().MyHp);
            }
            // プレイヤー２のHPを０にする
            if(Input.GetKeyDown(KeyCode.O))
            {
                Debug.Log("Input_O");
                pObj[1].GetComponent<PlayerDB>().SetHp(0);
                Debug.Log("Player1_HP:" + pObj[1].GetComponent<PlayerDB>().MyHp);
            }
            // プレイヤー３のHPを０にする
            if (Input.GetKeyDown(KeyCode.J))
            {
                Debug.Log("Input_J");
                pObj[2].GetComponent<PlayerDB>().SetHp(0);
                Debug.Log("Player2_HP:" + pObj[2].GetComponent<PlayerDB>().MyHp);
            }
            // プレイヤー４のHPを０にする
            if (Input.GetKeyDown(KeyCode.I))
            {
                Debug.Log("Input_I");
                pObj[3].GetComponent<PlayerDB>().SetHp(0);
                Debug.Log("Player3_HP:" + pObj[3].GetComponent<PlayerDB>().MyHp);
            }
            // プレイヤー参加数とプレイヤー生存数をログに出力
            if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log("PlayerAliveCounter:" + PlayerAliveCounter);
                Debug.Log("PlayerMemberNum:" + PlayerMemberNum);
            }
            // プレイヤー１の座標をログに出力
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("Player0_Pos:" + pObj[0].transform.position);
            }
        }
        
    }
}

// メモ
// 中間リザルト処理：完成（2023/12/25）
// 
// 現時点での懸念点
// 参加プレイヤー数が増えたときに何かしらエラーが発生するかもしれない
// （参加プレイヤー数２人でしかテストできていないため）
// 
// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//
// 2024/01/12
// 更新：プレイヤーオブジェクトの名称変更に伴い、プレイヤーオブジェクトの取得タイミング及び処理を変更
// 
// 2024/01/19
// 更新：勝利したプレイヤーに応じて、中間リザルトで表示する内容が変えられるように変更
// ※処理テストの都合上、GameManager,PlayerManagerでの中間リザルト処理をコメントアウトしました（GameManager -> /* MidResurt() */　PlayerManager -> //if (currentPlayerCount == 1) GMScript.MidResult();） 
// 　Git上では GameManager,PlayerManager の変更をコミットしないので、このスクリプトを正常に動作させる場合は、上記の処理のコメントアウトをお願いします
// 
// 2024/01/22
// 更新：中間リザルト処理を実行する条件を、プレイヤーが残り1人の時からプレイヤーが残り１人以下の時に変更
// 
// 2024/01/26
// 更新：最大勝利数の値を、ゲームマネージャースクリプトから取得するように変更（GetMaxWinPoints()を使用）
// 修正：コードのコメントを一部変更
// 
// 2024/01/29
// 更新：勝利したプレイヤーの要素番号をSetSingletonスクリプト内のWinnerNum変数に渡す処理の追加
// 　　　デバッグ用処理に、プレイヤー３，プレイヤー４のHPを０にする処理を追加
// 
// 2024/02/08
// 更新：Scaffold_pを取得し、すり抜け床の処理に使っている変数の初期化処理を追加
// 