using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicAnimationManager : MonoBehaviour
{
    // 変数宣言
    private Animator animator;          // アニメーションフロー制御用
    private PlayerDB playerDB;          // プレイヤー番号取得用

    // キー入力をプレイヤースクリプトと合わせる
    [Header("キーコンフィグ")]
    [SerializeField] private string[] GamepadButton = new string[4] { "A", "B", "X", "Y" };

    private int PlayerNum;          // プレイヤー番号格納用

    private string[] AnimatorFlag = new string[3] {"charge", "up_magic", "down_magic"};         // アニメーター側が持つbool型変数の名前格納用

    // Start is called before the first frame update
    void Start()
    {
        // スクリプトやコンポーネントの取得
        animator = GetComponent<Animator>();
        playerDB = GetComponent<PlayerDB>();

        // プレイヤー番号の設定(エディタ側のキー設定により、番号は1~4の範囲)
        PlayerNum = playerDB.MyNumber + 1;
    }

    // Update is called once per frame
    void Update()
    {
        // アニメーション遷移条件の管理処理
        MagicAnimationProcessing();

        // デバッグ用
        DebugProcessing();

    }

    // アニメーションの遷移条件の操作
    // ここで、入力されたボタンに応じて、アニメーションコントローラについている変数（bool）を操作
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



    // アニメーション変数の操作を少し簡略化するための関数
    private void SetAnimationStatus(string statusName, bool status)
    {
        if(statusName != null)
        {
            animator.SetBool(statusName, status);
        }
        else
        {
            Debug.Log("エラー：アニメーターのパラメータ名を入れる変数が未設定または配列の要素外");
        }
    }

    // どのプレイヤーのコントローラが入力されたかを取る関数群（押されている、押された、離された）
    private bool PressedButton(string _str)
    {
        return Input.GetButton("Player" + PlayerNum + _str);
    }

    // デバッグ用処理をまとめた関数
    private void DebugProcessing()
    {
        // スペースを押しながら
        if(Input.GetKey(KeyCode.Space))
        {
            // Zキーで通常魔法のアニメーション
            if(Input.GetKey(KeyCode.Z))
            {
                animator.SetBool(AnimatorFlag[0], true);
            }
            // Xキーで上魔法のアニメーション
            if (Input.GetKey(KeyCode.X))
            {
                animator.SetBool(AnimatorFlag[1], true);
            }
            // Cキーで下魔法のアニメーション
            if(Input.GetKey(KeyCode.C))
            {
                animator.SetBool(AnimatorFlag[2], true);
            }
        }
    }

}
