using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abyss : MonoBehaviour
{
    PlayerDB PlDb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            GameObject Pl_Obj = collision.gameObject;
            PlDb = Pl_Obj.GetComponent<PlayerDB>();
            //プレイヤーのHPを取得しその分を引くことによって、死亡させる
            float PlHP = PlDb.MyHp; 
            PlDb.SetHp(-PlHP);
        }
        ////奈落処理(プレイヤーを既定の位置に戻す処理)
        //GameObject py_obj = collision.gameObject;
        //py_obj.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        Debug.Log("Death");
    }
}
