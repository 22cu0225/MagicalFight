using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Down_attack : MonoBehaviour
{
    //左右の情報
    [HideInInspector] public bool Orientation;

    //挙動制御のための変数
    public float range_h;   //上への射程
    public float range_W;   //横への射程
    //--------------------------------------
    public float range_h_T; //伸びきるまでの時間
    public float wait_T;    //発生までの時間
    public float duration_T;//持続時間

    private float Time_Count; //経過時間

    // Start is called before the first frame update
    void Start()
    {
        Time_Count = 0.0f;
        range_h = range_h / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //時間のカウント
        Time_Count+=Time.deltaTime;

        //当たり判定関係の処理
        ATK_col();

        //オブジェクト破棄の処理
        delete();
    }

    //オブジェクト破棄の処理
    private void delete()
    {
        //消滅時のアニメーションを設定する場合ここに書く
        
        //指定した時間を過ぎると破壊
        if (Time_Count >= duration_T+ wait_T)
        {
            Destroy(gameObject);
        }
    }

    //当たり判定に関する判定
    private void ATK_col()
    {
        //上へ伸びる処理
        if (Time_Count >= wait_T)
        {
            //上へ伸びる処理
            transform.localScale = new Vector3(range_W, range_h, 0.0f);
        }
    }
}
