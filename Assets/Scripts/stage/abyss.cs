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
            //�v���C���[��HP���擾�����̕����������Ƃɂ���āA���S������
            float PlHP = PlDb.MyHp; 
            PlDb.SetHp(-PlHP);
        }
        ////�ޗ�����(�v���C���[������̈ʒu�ɖ߂�����)
        //GameObject py_obj = collision.gameObject;
        //py_obj.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        Debug.Log("Death");
    }
}
