using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAnimationManager : MonoBehaviour
{
    // �ϐ��錾
    PlayerDB playerDB;      // �v���C���[�̌���HP�̎擾�p
    Animator animator;      // �A�j���[�^�[�̃p�����[�^�[����p

    private float Hp;         // HP�̕ω����Ď�����p



    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g�A�X�N���v�g�̎擾
        playerDB = GetComponent<PlayerDB>();
        animator = GetComponent<Animator>();

        // �ŏ���HP���擾
        Hp = playerDB.MyHp;

    }

    // Update is called once per frame
    void Update()
    {
        DamageActionProcessing();
    }

    private void DamageActionProcessing()
    {
        // �A�j���[�V�����t���O�������Ă���ꍇ�A�t���O��؂�
        if (animator.GetBool("damage"))
        {
            SetAnimationStatus("damage", false);
        }

        // HP�̒l���ϓ��������i�_���[�W���󂯂����j
        if (Hp != playerDB.MyHp)
        {
            // �Ď��pHP�̒l���X�V����
            Hp = playerDB.MyHp;

            // �A�j���[�V�����t���O������
            SetAnimationStatus("damage", true);
        }
    }


    private void SetAnimationStatus(string statusName, bool status)
    {
        if (statusName != null)
        {
            animator.SetBool(statusName, status);
        }
        else
        {
            Debug.Log("�G���[�F�A�j���[�^�[�̃p�����[�^��������ϐ������ݒ�܂��͔z��̗v�f�O");
        }
    }
}
