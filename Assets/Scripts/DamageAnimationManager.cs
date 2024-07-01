using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAnimationManager : MonoBehaviour
{
    // 変数宣言
    PlayerDB playerDB;      // プレイヤーの現在HPの取得用
    Animator animator;      // アニメーターのパラメーター操作用

    private float Hp;         // HPの変化を監視する用



    // Start is called before the first frame update
    void Start()
    {
        // コンポーネント、スクリプトの取得
        playerDB = GetComponent<PlayerDB>();
        animator = GetComponent<Animator>();

        // 最初のHPを取得
        Hp = playerDB.MyHp;

    }

    // Update is called once per frame
    void Update()
    {
        DamageActionProcessing();
    }

    private void DamageActionProcessing()
    {
        // アニメーションフラグが入っている場合、フラグを切る
        if (animator.GetBool("damage"))
        {
            SetAnimationStatus("damage", false);
        }

        // HPの値が変動した時（ダメージを受けた時）
        if (Hp != playerDB.MyHp)
        {
            // 監視用HPの値を更新する
            Hp = playerDB.MyHp;

            // アニメーションフラグを入れる
            SetAnimationStatus("damage", true);
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
}
