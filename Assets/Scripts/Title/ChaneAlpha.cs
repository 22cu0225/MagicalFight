using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaneAlpha : MonoBehaviour
{
    // 変数宣言
    [SerializeField] private float ChangeValue;     // 変化量
    [SerializeField] private float ChangeTime;      // 変更するまでの時間

    private float Alpha;                            // アルファ値
    private float Timer;                            // 経過時間

    private bool Changesing;                        // 符号を変えるフラグ
    private float sing;                             // 符号を変える為の変数

    [SerializeField] private float MAX_Limits;      // アルファ値の最大値
    [SerializeField] private float min_Limits;      // アルファ値の最小値

    // コンポーネント取得用
    private SpriteRenderer SR;

    private void Start()
    {
        // 初期化
        Alpha = 0.0f;

        // コンポーネント取得
        SR = this.GetComponent<SpriteRenderer>();
        SR.color = new Color(1.0f, 1.0f, 1.0f, Alpha);
    }

    private void Update()
    {
        // 経過時間を取得
        Timer += Time.deltaTime;

        // アルファ値でフラグを切り替え
        if      (Alpha >= MAX_Limits) Changesing = false;
        else if (Alpha <= min_Limits) Changesing = true;

        // フラグで符号を切り替え
        sing = Changesing ? 1.0f : -1.0f;

        // 指定した時間が経過したら
        if (Timer >= ChangeTime)
        {
            // 指定した数だけアルファ値を更新
            Alpha += ChangeValue  * sing;
            SR.color = new Color(1.0f, 1.0f, 1.0f, Alpha);

            // Timer リセット
            Timer = 0.0f;
        }
    }

    private void FixedUpdate()
    {
        if (Alpha >= MAX_Limits) Alpha = MAX_Limits;
        if (Alpha <= min_Limits) Alpha = min_Limits;
    }
}
