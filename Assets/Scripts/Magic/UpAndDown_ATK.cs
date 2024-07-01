using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDown_ATK : MonoBehaviour
{
    //�v���C���[��obj,sc
    GameObject Player_obj;
    Player player;
    bool isJump = false;

    //�A�^�b�N�̃I�u�W�F�N�g
    [SerializeField]  GameObject UpATK_obj;
    [SerializeField]  GameObject DownATK_obj;
    Magic magicSclipt;

    //�����蔻���؂邽��
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
        //�����蔻���؂邽�߂Ɋi�[
        p_Col = player.GetComponent<Collider2D>();

        ATK_instance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�v���C���[�̃W�����v�̏�Ԃŕω�����U��
    void ATK_instance()
    {
        GameObject magic;
        isJump = Player_obj.GetComponent<Player>().isJumping();
        Debug.Log(isJump);
        //��U��
        if (!isJump)
        {
            magic = Instantiate(UpATK_obj, transform.position, transform.rotation);
            Debug.Log("ATK_UP");
        }
        //���U��
        else
        {
            magic = Instantiate(DownATK_obj, transform.position, transform.rotation);
            Debug.Log("ATK_Down");
        }
        magic.GetComponent<Magic>().setPNum(pNum);
        //���g�̕��ɓ�����Ȃ��悤�ɂ��邽�ߓ����蔻���؂�
        m_Col = magic.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(p_Col, m_Col, true);
    }
}
