using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//���蔲�������������邽�߂Ƀv���C���[�ɂ���R���|�[�l���g

public class Scaffold_p : MonoBehaviour
{
    //�ϐ��錾
    private bool On_Sca;�@�@�@//���蔲�����ɏ���Ă��邩�ǂ���
    private bool Active_Sca;     //�����蔻��̕����̏�������
    //�R���|�[�l���g�i�[�̂���
    private Collider2D P_Col;   //�v���C���[
    private Collider2D Sca_Col; //���蔲����

    //���Ԓ����p�̕ϐ�
    public float waitTime = 0.3f;

    private bool through = false;
    public void SetThrough(bool _through) { through = _through; }

    // Start is called before the first frame update
    void Start()
    {
        //�ϐ��̏�����
        On_Sca = false;
        Active_Sca = false;
        P_Col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //�t���O��ON�Ȃ�
        if ( On_Sca==true )
        {
            //���{�^���i�m�F�̂��߃L�[�{�[�h��"�r"�j����������
            if(through)
            {
                //�����蔻���Off�ɂ���
                Physics2D.IgnoreCollision(P_Col, Sca_Col,true);
                Active_Sca = true;
                //�����蔻����ēxOn�ɂ���
                StartCoroutine(ActiveCollision());
                through = false;
            }
        }

    }

    //�����蔻����ēxOn�ɂ���֐�
    IEnumerator ActiveCollision()
    {
        //0.3�b�̏����x��
        yield return new WaitForSeconds(waitTime);
        //�����蔻����ēxOn�ɂ���
        Physics2D.IgnoreCollision(P_Col, Sca_Col, false);
        //�t���O�̐؂�ւ�
        On_Sca = false;
        Active_Sca = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�v���C���[�����蔲�����ɏ���Ă��邱�Ƃ�`����(����̕������ɂ͏������Ȃ�)
        if (collision.gameObject.tag=="Scaffold" && Active_Sca == false)
        {
            Debug.Log(Active_Sca);
            if (On_Sca == false)
            {
                //����Ă��鏰�̃R���|�[�l���g���擾
                //�t���O�̐؂�ւ�
                On_Sca = true;
                Sca_Col = collision.gameObject.GetComponent<Collider2D>();
                Debug.Log(Sca_Col);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //�v���C���[�����蔲�������痣�ꂽ���Ƃ���`����
        if (collision.gameObject.tag == "Scaffold" )
        {
            //�t���O�̐؂�ւ�
            On_Sca = false;
        }
    }

    // 2024/02/08�@��{�F�ǉ��F�t���O�̍Đݒ�p�֐��iMidResultManager�Ŏg���܂��j
    public void SetOn_Sca(bool flag)
    {
        On_Sca = flag;
    }

    public void SetActive_Sca(bool flag)
    {
        Active_Sca = flag;
    }

}
