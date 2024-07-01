using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_Attack : MonoBehaviour
{
    GameObject Player_obj;
    Player player;

    //�i�[�p�ϐ�
    Magic magic;
    //���E�̏��
    [HideInInspector]public bool Orientation;
    [HideInInspector] public bool move_switch;

    //�c���ւ̈ړ�
    Vector2 speed;
    [SerializeField] float speed_up; //���߂��ۂ̉����̔{��

    private bool is_Move;   //�ړ��t���O

    //�З�
    int MagicPower;//��b�З�
    //--------------------------------
    [SerializeField] int Charge_p_2;  //2�i�K�ڂ̈З�
    [SerializeField] int Charge_p_3;  //3�i�K�ڂ̈З�

    //���ߎ��Ԃ��i�[
    [SerializeField] float Charge_T_2;//2�i�K��
    [SerializeField] float Charge_T_3;//3�i�K�� 
    private int Charge_step;//�`���[�W�i�K

    private float Count_t;   //���Ԃ��J�E���g


    //[SerializeField] Sprite[] magicImages;          // ���p�̊O��


    // Start is called before the first frame update
    void Start()
    {
        magic = GetComponent<Magic>();
        Player_obj = magic.Player;
        player = Player_obj.GetComponent<Player>();
        speed = magic.GetSpeed();
        MagicPower = magic.GetPower();
        //�ϐ��̏�����
        Count_t = 0.0f;
        Charge_step = 0;
        is_Move = false;
        move_switch = true;
        //�v���C���[�Ɛe�q�֌W�ɂ���
        gameObject.transform.SetParent(Player_obj.transform);
        //
        //GetComponent<SpriteRenderer>().sprite = magicImages[Player_obj.GetComponent<PlayerDB>().MyNumber];
        //�{�^���𗣂��܂ŏ��ǂɓ�����Ȃ��悤�ɂ���
        magic.SetF_Destroy(false);
    }

    // Update is called once per frame
    void Update()
    {
        //���Ԃ̃J�E���g
        Count_t += Time.deltaTime;
        //�ړ�(�{�^���𗣂Ȃ����Ƃ��t���O��On�ɂ���)
        if(player.Release(player.GetButton(1)))
        {
            is_Move = true;
            //�e�q�֌W������
            gameObject.transform.parent = null;
            //���E�擾
            Orientation=player.GetOrientation();
            magic.SetOrientation(Orientation);
            //�Ǐ��ɓ�����悤�ɂ���
            magic.SetF_Destroy(true);
        }
        //�ړ�
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
        //�`���[�W�i�K��ύX(�{�^���������̂�)
        if (is_Move == false)
        {
            Charge_Step();
        }
    }

    //�`���[�W�i�K���Ǘ�����ϐ�
    private void Charge_Step()
    {
        //�w�莞�Ԃ��߂���Ǝ��̒i�K�֐i��
        //1->2�i�K��
        if(Count_t>= Charge_T_2 && Charge_step==0)
        {
            //�З͂ƒi�K�ƃX�s�[�h�̍X�V
            ++Charge_step;
            speed.x = speed.x * speed_up;
            //�З̓Z�b�g
            magic.SetPower(Charge_p_2);
            //---------------------------
            Debug.Log(MagicPower);
        }
        //�Q->3�i�K��
        if (Count_t >= Charge_T_2 + Charge_T_3 && Charge_step == 1)
        {
            //�З͂ƒi�K�ƃX�s�[�h�̍X�V
            ++Charge_step;
            speed.x = speed.x * speed_up;
            //�З̓Z�b�g
            magic.SetPower(Charge_p_3);
            //---------------------------
            Debug.Log(MagicPower);
        }
    }

    // 2024/02/08 ��{�F�ǉ��@�`���[�W�i�K���������ϐ����擾����֐�
    public int GetCharge_step()
    {
        return Charge_step;
    }
    // isMove���擾����֐�
    public bool Getis_Move()
    {
        return is_Move;
    }

}
