using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiShield : MonoBehaviour
{
    //�v���C���[�̏�ԁi���E�̌����̎擾�ɕK�v�j
    GameObject Player_obj;
    Player player;
    //���E�̏��
    [HideInInspector] public bool Orientation;

    Vector3 speed;

    //�����ʒu�i�[�p
    Vector3 popPos;

    //
    Rigidbody2D rb;
    //����
    [SerializeField] float InitialVel;
    //�p�x
    [SerializeField] float angle;

    ////�����
    //[SerializeField] float deltaHigh;
 
    Magic magicSclipt;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        magicSclipt = GetComponent<Magic>();
        Player_obj = magicSclipt.Player;
        player = Player_obj.GetComponent<Player>();
        //���E�̌������擾
        Orientation = player.GetOrientation();
        //12/22�@11:17 ���݃��\�b�h���Ȃ����߃R�����g�A�E�g
        speed = magicSclipt.GetSpeed();
        popPos = transform.position;
        //�������̏ꍇ
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
