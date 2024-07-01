using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    //プレイヤーのprefab
    [HideInInspector]public GameObject Player;

    // 変数宣言
    [SerializeField] Vector2 offsetMagicPosition;   // 魔術の生成位置
    [SerializeField] int power;                     // 威力
    [SerializeField] Vector2 speed;                 // 速度
    [SerializeField] float destroySecond = 10.0f;   // 消滅時間
    [SerializeField] float coolTime = 1.0f;         // クールタイム
    [SerializeField] string magicTag = "Magic";     // 魔術のタグ名
    [SerializeField] bool FloorDestroy = true;      // 床、壁に当たり判定をつけるか
    [SerializeField] bool pCol_Destroy = true;      // プレイヤーと当たった瞬間に消すか
    private bool orientation = false;               // 魔術の向き
    private int pNum;                               // 生成プレイヤーの番号

    // 相殺エフェクト
    [SerializeField] GameObject Counterbalancing;

    private void Update()
    {
        destroySecond -= Time.deltaTime;
        if (destroySecond <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    // Set Function
    public void SetPower(int _power) { power = _power; }
    public void SetOrientation(bool _orientation) { orientation = _orientation; }
    public void setPlayerObj(GameObject obj) { Player = obj; }
    public void setPNum(int P_NUM) { pNum = P_NUM; }
    public void SetF_Destroy(bool fd) { FloorDestroy = fd; }

    // Get Function
    public Vector2 GetMagicPosition() { return offsetMagicPosition; }
    public int GetPower() { return power; }
    public Vector2 GetSpeed() { return speed; }
    public float GetCoolTime() { return coolTime; }
    public int GetPNum() { return pNum; }
    public bool GetOrientation() { return orientation; }

    // 撃破プレイヤー判別用（岩本）
    public string GetPName() { return Player.name; }

    //Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(magicTag))
        {
            Debug.Log(pNum != collision.GetComponent<Magic>().GetPNum());
            //生成プレイヤーの番号と異なる場合のみ処理
            if(pNum!= collision.GetComponent<Magic>().pNum)
            {
                int ThisPower = this.GetPower();
                int OtherPower = collision.GetComponent<Magic>().GetPower();
                if (ThisPower - OtherPower <= 0)
                {
                    if(Counterbalancing != null)
                    {
                        GameObject efect = Instantiate(Counterbalancing);
                        efect.transform.position = this.transform.position;
                    }
                    Destroy(gameObject);
                }
                else
                {
                    collision.GetComponent<Magic>().SetPower(ThisPower - OtherPower);
                }
            }
        }
        //床壁に接触したらデストロイ
        if(collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Wall")
		{
            if(FloorDestroy)
            {
                if (Counterbalancing != null)
                {
                    GameObject efect = Instantiate(Counterbalancing);
                    efect.transform.position = this.transform.position;
                }
                Destroy(this.gameObject);
            }
        }
        //プレイヤーに触れたらデストロイ
        if(collision.gameObject.tag=="Player")
        {
            if (pCol_Destroy)
            {
                if (Counterbalancing != null)
                {
                    GameObject efect = Instantiate(Counterbalancing);
                    efect.transform.position = this.transform.position;
                }
                Destroy(this.gameObject);
            }
        }
    }
	private void OnTriggerStay2D(Collider2D collision)
	{
        //床壁に接触したらデストロイ
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Wall")
        {
            if (FloorDestroy)
            {
                if (Counterbalancing != null)
                {
                    GameObject efect = Instantiate(Counterbalancing);
                    efect.transform.position = this.transform.position;
                }
                Destroy(this.gameObject);
            }
        }
    }
}
