using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Down_attack : MonoBehaviour
{
    //���E�̏��
    [HideInInspector] public bool Orientation;

    //��������̂��߂̕ϐ�
    public float range_h;   //��ւ̎˒�
    public float range_W;   //���ւ̎˒�
    //--------------------------------------
    public float range_h_T; //�L�т���܂ł̎���
    public float wait_T;    //�����܂ł̎���
    public float duration_T;//��������

    private float Time_Count; //�o�ߎ���

    // Start is called before the first frame update
    void Start()
    {
        Time_Count = 0.0f;
        range_h = range_h / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //���Ԃ̃J�E���g
        Time_Count+=Time.deltaTime;

        //�����蔻��֌W�̏���
        ATK_col();

        //�I�u�W�F�N�g�j���̏���
        delete();
    }

    //�I�u�W�F�N�g�j���̏���
    private void delete()
    {
        //���Ŏ��̃A�j���[�V������ݒ肷��ꍇ�����ɏ���
        
        //�w�肵�����Ԃ��߂���Ɣj��
        if (Time_Count >= duration_T+ wait_T)
        {
            Destroy(gameObject);
        }
    }

    //�����蔻��Ɋւ��锻��
    private void ATK_col()
    {
        //��֐L�т鏈��
        if (Time_Count >= wait_T)
        {
            //��֐L�т鏈��
            transform.localScale = new Vector3(range_W, range_h, 0.0f);
        }
    }
}
