using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//すり抜け床を実装するためにプレイヤーにつけるコンポーネント

public class Scaffold_p : MonoBehaviour
{
    //変数宣言
    private bool On_Sca;　　　//すり抜け床に乗っているかどうか
    private bool Active_Sca;     //当たり判定の復活の処理中か
    //コンポーネント格納のため
    private Collider2D P_Col;   //プレイヤー
    private Collider2D Sca_Col; //すり抜け床

    //時間調整用の変数
    public float waitTime = 0.3f;

    private bool through = false;
    public void SetThrough(bool _through) { through = _through; }

    // Start is called before the first frame update
    void Start()
    {
        //変数の初期化
        On_Sca = false;
        Active_Sca = false;
        P_Col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //フラグがONなら
        if ( On_Sca==true )
        {
            //下ボタン（確認のためキーボードの"Ｓ"）を押したら
            if(through)
            {
                //当たり判定をOffにする
                Physics2D.IgnoreCollision(P_Col, Sca_Col,true);
                Active_Sca = true;
                //当たり判定を再度Onにする
                StartCoroutine(ActiveCollision());
                through = false;
            }
        }

    }

    //当たり判定を再度Onにする関数
    IEnumerator ActiveCollision()
    {
        //0.3秒の処理遅延
        yield return new WaitForSeconds(waitTime);
        //当たり判定を再度Onにする
        Physics2D.IgnoreCollision(P_Col, Sca_Col, false);
        //フラグの切り替え
        On_Sca = false;
        Active_Sca = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //プレイヤーがすり抜け床に乗っていることを伝える(判定の復活中には処理しない)
        if (collision.gameObject.tag=="Scaffold" && Active_Sca == false)
        {
            Debug.Log(Active_Sca);
            if (On_Sca == false)
            {
                //乗っている床のコンポーネントを取得
                //フラグの切り替え
                On_Sca = true;
                Sca_Col = collision.gameObject.GetComponent<Collider2D>();
                Debug.Log(Sca_Col);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //プレイヤーがすり抜け床から離れたことをを伝える
        if (collision.gameObject.tag == "Scaffold" )
        {
            //フラグの切り替え
            On_Sca = false;
        }
    }

    // 2024/02/08　岩本：追加：フラグの再設定用関数（MidResultManagerで使います）
    public void SetOn_Sca(bool flag)
    {
        On_Sca = flag;
    }

    public void SetActive_Sca(bool flag)
    {
        Active_Sca = flag;
    }

}
