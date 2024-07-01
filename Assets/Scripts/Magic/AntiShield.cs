using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiShield : MonoBehaviour
{
    //プレイヤーの状態（左右の向きの取得に必要）
    GameObject Player_obj;
    Player player;
    //左右の情報
    [HideInInspector] public bool Orientation;

    Vector3 speed;

    //生成位置格納用
    Vector3 popPos;

    //
    Rigidbody2D rb;
    //初速
    [SerializeField] float InitialVel;
    //角度
    [SerializeField] float angle;

    ////落ち具合
    //[SerializeField] float deltaHigh;
 
    Magic magicSclipt;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        magicSclipt = GetComponent<Magic>();
        Player_obj = magicSclipt.Player;
        player = Player_obj.GetComponent<Player>();
        //左右の向きを取得
        Orientation = player.GetOrientation();
        //12/22　11:17 現在メソッドがないためコメントアウト
        speed = magicSclipt.GetSpeed();
        popPos = transform.position;
        //左向きの場合
        if(!Orientation)
        {
            //speed.x = speed.x * -1.0f;
            angle = 180 - angle;
        }
        speed.x = InitialVel * Mathf.Cos(angle * Mathf.Deg2Rad);
        speed.y = InitialVel * Mathf.Sin(angle * Mathf.Deg2Rad);
        Debug.Log(speed);
        rb.AddForce(speed,ForceMode2D.Impulse);
    }

    void Update()
    {
        //if(popPos.y + deltaHigh > transform.position.y)
        //{
        //    transform.Translate(-speed.x, speed.y, 0.0f);
        //    Debug.Log("naname");
        //}
        //else
        //{
        //    transform.Translate(-speed.x, 0.0f, 0.0f);
        //    Debug.Log("massugu");
        //}
    }
}
