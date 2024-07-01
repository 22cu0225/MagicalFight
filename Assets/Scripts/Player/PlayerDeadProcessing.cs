using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadProcessing : MonoBehaviour
{
    // 各スクリプトやコンポーネントの取得用変数
    [HideInInspector] public PlayerDB playerDB;
    [HideInInspector] public PlayerManager playerManager;
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    // オブジェクトを格納する変数
    public GameObject DeadObj;
    public GameObject PlayerObj;

    // トランスフォームを格納する変数
    Transform PlayerTransform;

    // プレイヤー死亡時に行う処理
    public bool PlayerDestroy;

    // アルファ値の操作のための変数
    [HideInInspector] public float Alpha;
    public float AlphaSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // スクリプト、コンポーネントの取得
        playerDB = GetComponent<PlayerDB>();
        playerManager = GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // エフェクト発生位置のためにプレイヤーの座標取得
        PlayerTransform = transform;

        // 不透明度操作のために現在の不透明度の取得
        Alpha = spriteRenderer.color.a;

        // プレイヤー破壊処理の１回のみ行うためのフラグ
        PlayerDestroy = false;
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーのHPが０以下になった場合
        if(playerDB.MyHp <= 0)
        {
            // 死亡時のアニメーションやエフェクトの生成、不透明度操作処理など
            ProcPlayerDeathAnim();
        }        

        // 不透明度が０以下になり、生存判定がfalseになった時
        if (Alpha <= 0.0f && PlayerDestroy == false && playerDB.MyIsAlive == false)
        {
            // プレイヤー死亡時の処理や、アニメーション等再生後の後処理
            ProcPlayerDestroy();
        }
    }

    void ProcPlayerDeathAnim()
    {
        // トランスフォームが異なる物になってしまった場合に再設定
        if (PlayerTransform != transform)
        {
            PlayerTransform = transform;
        }

        // エフェクト生成とアニメーション設定
        // 死亡判定の更新をこのタイミングで行う
        if (playerDB.MyIsAlive == true)
        {
            Instantiate(DeadObj, PlayerTransform);
            animator.SetBool("dead", true);

            // プレイヤー生存を判定する変数をfalseに設定（死亡判定の更新）
            playerDB.SetIsAlive(false);

        }
        // HPが０以下の時かつ不透明度が０より大きい間
        // 不透明度の操作(0を下回らない様に値の調整)
        if (playerDB.MyHp <= 0 && Alpha > 0.0f)
        {
            // 不透明度を下げる（下げる速度はインスペクター上で調整可能）
            Alpha -= AlphaSpeed;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Alpha);
        }
    }

    void ProcPlayerDestroy()
    {        
        // 不透明度が０を下回っていた場合、０に再設定する
        if (spriteRenderer.color.a < 0.0f)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.0f);
        }

        // アニメーション設定変数（死亡アニメーション）をfalseに設定し、死亡エフェクトを破棄する（破棄しないと残り続けてしまうため）
        animator.SetBool("dead", false);

        // プレイヤー死亡時の処理（PlayerManagerで行っていた処理をここで行う）
        if (PlayerDestroy == false)
        {
            // 1回のみ行うためフラグの設定（リトライ時にMidResultManagerによりfalseに戻る）
            PlayerDestroy = true;

            // プレイヤー死亡時の処理 -------------------------

            // プレイヤーオブジェクトの非表示
            PlayerObj.SetActive(false);
            // HPバーの非表示
            playerDB.MyHpBar.SetActive(false);

            // ----------------------------------------------

            // 正常に動作しなかったためコメントアウト（NULLReferenceの発生）
            //playerManager.PlayerDestroy(PlayerObj);
        }
    }
}
