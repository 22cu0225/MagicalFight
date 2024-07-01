using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_Attack : MonoBehaviour
{
    GameObject Player_obj;
    Player player;

    //格納用変数
    Magic magic;
    //左右の情報
    [HideInInspector]public bool Orientation;
    [HideInInspector] public bool move_switch;

    //縦横への移動
    Vector2 speed;
    [SerializeField] float speed_up; //ためた際の加速の倍率

    private bool is_Move;   //移動フラグ

    //威力
    int MagicPower;//基礎威力
    //--------------------------------
    [SerializeField] int Charge_p_2;  //2段階目の威力
    [SerializeField] int Charge_p_3;  //3段階目の威力

    //ため時間を格納
    [SerializeField] float Charge_T_2;//2段階目
    [SerializeField] float Charge_T_3;//3段階目 
    private int Charge_step;//チャージ段階

    private float Count_t;   //時間をカウント


    //[SerializeField] Sprite[] magicImages;          // 魔術の外見


    // Start is called before the first frame update
    void Start()
    {
        magic = GetComponent<Magic>();
        Player_obj = magic.Player;
        player = Player_obj.GetComponent<Player>();
        speed = magic.GetSpeed();
        MagicPower = magic.GetPower();
        //変数の初期化
        Count_t = 0.0f;
        Charge_step = 0;
        is_Move = false;
        move_switch = true;
        //プレイヤーと親子関係にする
        gameObject.transform.SetParent(Player_obj.transform);
        //
        //GetComponent<SpriteRenderer>().sprite = magicImages[Player_obj.GetComponent<PlayerDB>().MyNumber];
        //ボタンを離すまで床壁に当たらないようにする
        magic.SetF_Destroy(false);
    }

    // Update is called once per frame
    void Update()
    {
        //時間のカウント
        Count_t += Time.deltaTime;
        //移動(ボタンを離なしたときフラグをOnにする)
        if(player.Release(player.GetButton(1)))
        {
            is_Move = true;
            //親子関係を解消
            gameObject.transform.parent = null;
            //左右取得
            Orientation=player.GetOrientation();
            magic.SetOrientation(Orientation);
            //壁床に当たるようにする
            magic.SetF_Destroy(true);
        }
        //移動
        if (is_Move == true)
        {
           if(Orientation && move_switch)
           {
               //transform.Translate(speed.x / 100.0f, speed.y / 100.0f, 0.0f);
               move_switch = false;
                Debug.Log("b_R");
           }
           if (!Orientation && move_switch)
           {
               //transform.Translate(-(speed.x / 100.0f), speed.y / 100.0f, 0.0f);
               speed.x = speed.x * -1.0f;
                move_switch = false;
                Debug.Log("b_L");
            }
            transform.Translate(speed.x * Time.deltaTime, 0.0f, 0.0f);
        }
        //チャージ段階を変更(ボタン押し中のみ)
        if (is_Move == false)
        {
            Charge_Step();
        }
    }

    //チャージ段階を管理する変数
    private void Charge_Step()
    {
        //指定時間を過ぎると次の段階へ進む
        //1->2段階目
        if(Count_t>= Charge_T_2 && Charge_step==0)
        {
            //威力と段階とスピードの更新
            ++Charge_step;
            speed.x = speed.x * speed_up;
            //威力セット
            magic.SetPower(Charge_p_2);
            //---------------------------
            Debug.Log(MagicPower);
        }
        //２->3段階目
        if (Count_t >= Charge_T_2 + Charge_T_3 && Charge_step == 1)
        {
            //威力と段階とスピードの更新
            ++Charge_step;
            speed.x = speed.x * speed_up;
            //威力セット
            magic.SetPower(Charge_p_3);
            //---------------------------
            Debug.Log(MagicPower);
        }
    }

    // 2024/02/08 岩本：追加　チャージ段階が入った変数を取得する関数
    public int GetCharge_step()
    {
        return Charge_step;
    }
    // isMoveを取得する関数
    public bool Getis_Move()
    {
        return is_Move;
    }

}
