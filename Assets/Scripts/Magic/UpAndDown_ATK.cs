using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDown_ATK : MonoBehaviour
{
    //プレイヤーのobj,sc
    GameObject Player_obj;
    Player player;
    bool isJump = false;

    //アタックのオブジェクト
    [SerializeField]  GameObject UpATK_obj;
    [SerializeField]  GameObject DownATK_obj;
    Magic magicSclipt;

    //当たり判定を切るため
    private Collider2D p_Col;
    private Collider2D m_Col;

    private int pNum;

    // Start is called before the first frame update
    void Start()
    {
        magicSclipt = GetComponent<Magic>();
        Player_obj = magicSclipt.Player;
        player = Player_obj.GetComponent<Player>();
        pNum = GetComponent<Magic>().GetPNum();
        //当たり判定を切るために格納
        p_Col = player.GetComponent<Collider2D>();

        ATK_instance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //プレイヤーのジャンプの状態で変化する攻撃
    void ATK_instance()
    {
        GameObject magic;
        isJump = Player_obj.GetComponent<Player>().isJumping();
        Debug.Log(isJump);
        //上攻撃
        if (!isJump)
        {
            magic = Instantiate(UpATK_obj, transform.position, transform.rotation);
            Debug.Log("ATK_UP");
        }
        //下攻撃
        else
        {
            magic = Instantiate(DownATK_obj, transform.position, transform.rotation);
            Debug.Log("ATK_Down");
        }
        magic.GetComponent<Magic>().setPNum(pNum);
        //自身の物に当たらないようにするため当たり判定を切る
        m_Col = magic.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(p_Col, m_Col, true);
    }
}
