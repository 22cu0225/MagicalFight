using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    // 変数宣言
    private Animator animator;          // アニメーションフロー制御用
    private PlayerDB playerDB;          // プレイヤー番号取得用

    // キー入力をプレイヤースクリプトと合わせる
    [Header("キーコンフィグ")]
    [SerializeField] private string[] GamepadButton = new string[4] { "A", "B", "X", "Y" };

    private int PlayerNum;          // プレイヤー番号格納用

    private string[] AnimatorFlag = new string[4] { "charge", "up_magic", "down_magic", "damage" };         // アニメーター側が持つbool型変数の名前格納用

    private float Hp;               // HPの変化を監視する用


    // Start is called before the first frame update
    void Start()
    {
        // スクリプトやコンポーネントの取得
        animator = GetComponent<Animator>();
        playerDB = GetComponent<PlayerDB>();

        // プレイヤー番号の設定(エディタ側のキー設定により、番号は1~4の範囲)
        // ここで取得したプレイヤー番号は、ボタン入力の判断時に使う
        PlayerNum = playerDB.MyNumber + 1;

        // 最初のHPを取得
        Hp = playerDB.MyHp;
    }

    // Update is called once per frame
    void Update()
    {
        MagicAnimationProcessing();
        DamageActionProcessing();
        DebugProcessing();
    }


    // 魔法動作アニメーションの遷移条件の操作
    // ここで、入力されたボタンに応じて、アニメーターのパラメーターを操作
    // trueでアニメーション再生という流れ
    private void MagicAnimationProcessing()
    {
        // 押されたボタン：B（通常魔法）
        if (PressedButton(GamepadButton[1]))
        {
            SetAnimationStatus(AnimatorFlag[0], true);
        }
        else
        {
            SetAnimationStatus(AnimatorFlag[0], false);
        }

        // 押されたボタン：X（対空魔法）
        if (PressedButton(GamepadButton[3]))
        {
            SetAnimationStatus(AnimatorFlag[1], true);
        }
        else
        {
            SetAnimationStatus(AnimatorFlag[1], false);
        }

        // 押されたボタン：Y（対地魔法）
        if (PressedButton(GamepadButton[2]))
        {
            SetAnimationStatus(AnimatorFlag[2], true);
        }
        else
        {
            SetAnimationStatus(AnimatorFlag[2], false);
        }
    }

    // ダメージリアクションアニメーションの遷移条件の操作
    // ここで、PlayerDBのHPを監視し、HPの値に変化があったら（ダメージを受けたら）
    // アニメーターのパラメーターを操作し、アニメーションを再生するという流れ
    private void DamageActionProcessing()
    {
        // アニメーションフラグが入っている場合、フラグを切る
        if (animator.GetBool("damage"))
        {
            SetAnimationStatus(AnimatorFlag[3], false);
        }

        // HPの値が変動した時（ダメージを受けた時）
        if (Hp != playerDB.MyHp)
        {
            // 監視用HPの値を更新する
            Hp = playerDB.MyHp;

            // アニメーションフラグを入れる
            animator.SetBool(AnimatorFlag[3], true);

        }
    }

    private void SetAnimationStatus(string statusName, bool status)
    {
        if (statusName != null)
        {
            animator.SetBool(statusName, status);
        }
        else
        {
            Debug.Log("エラー：アニメーターのパラメータ名を入れる変数が未設定または配列の要素外");
        }
    }

    // プレイヤーのコントローラが入力されたかを取る関数
    private bool PressedButton(string _str)
    {
        return Input.GetButton("Player" + PlayerNum + _str);
    }


    private void DebugProcessing()
    {
        // スペースを押しながら
        if (Input.GetKey(KeyCode.Space))
        {
            // Zキーで通常魔法のアニメーション
            if (Input.GetKey(KeyCode.Z))
            {
                animator.SetBool(AnimatorFlag[0], true);
            }
            // Xキーで上魔法のアニメーション
            if (Input.GetKey(KeyCode.X))
            {
                animator.SetBool(AnimatorFlag[1], true);
            }
            // Cキーで下魔法のアニメーション
            if (Input.GetKey(KeyCode.C))
            {
                animator.SetBool(AnimatorFlag[2], true);
            }
            // Vキーでダメージリアクションのアニメーション
            if(Input.GetKey(KeyCode.V))
            {
                animator.SetBool(AnimatorFlag[3], true);
            }
        }
    }
}
