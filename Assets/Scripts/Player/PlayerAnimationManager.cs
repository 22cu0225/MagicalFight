using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    // �ϐ��錾
    private Animator animator;          // �A�j���[�V�����t���[����p
    private PlayerDB playerDB;          // �v���C���[�ԍ��擾�p

    // �L�[���͂��v���C���[�X�N���v�g�ƍ��킹��
    [Header("�L�[�R���t�B�O")]
    [SerializeField] private string[] GamepadButton = new string[4] { "A", "B", "X", "Y" };

    private int PlayerNum;          // �v���C���[�ԍ��i�[�p

    private string[] AnimatorFlag = new string[4] { "charge", "up_magic", "down_magic", "damage" };         // �A�j���[�^�[��������bool�^�ϐ��̖��O�i�[�p

    private float Hp;               // HP�̕ω����Ď�����p


    // Start is called before the first frame update
    void Start()
    {
        // �X�N���v�g��R���|�[�l���g�̎擾
        animator = GetComponent<Animator>();
        playerDB = GetComponent<PlayerDB>();

        // �v���C���[�ԍ��̐ݒ�(�G�f�B�^���̃L�[�ݒ�ɂ��A�ԍ���1~4�͈̔�)
        // �����Ŏ擾�����v���C���[�ԍ��́A�{�^�����͂̔��f���Ɏg��
        PlayerNum = playerDB.MyNumber + 1;

        // �ŏ���HP���擾
        Hp = playerDB.MyHp;
    }

    // Update is called once per frame
    void Update()
    {
        MagicAnimationProcessing();
        DamageActionProcessing();
        DebugProcessing();
    }


    // ���@����A�j���[�V�����̑J�ڏ����̑���
    // �����ŁA���͂��ꂽ�{�^���ɉ����āA�A�j���[�^�[�̃p�����[�^�[�𑀍�
    // true�ŃA�j���[�V�����Đ��Ƃ�������
    private void MagicAnimationProcessing()
    {
        // �����ꂽ�{�^���FB�i�ʏ햂�@�j
        if (PressedButton(GamepadButton[1]))
        {
            SetAnimationStatus(AnimatorFlag[0], true);
        }
        else
        {
            SetAnimationStatus(AnimatorFlag[0], false);
        }

        // �����ꂽ�{�^���FX�i�΋󖂖@�j
        if (PressedButton(GamepadButton[3]))
        {
            SetAnimationStatus(AnimatorFlag[1], true);
        }
        else
        {
            SetAnimationStatus(AnimatorFlag[1], false);
        }

        // �����ꂽ�{�^���FY�i�Βn���@�j
        if (PressedButton(GamepadButton[2]))
        {
            SetAnimationStatus(AnimatorFlag[2], true);
        }
        else
        {
            SetAnimationStatus(AnimatorFlag[2], false);
        }
    }

    // �_���[�W���A�N�V�����A�j���[�V�����̑J�ڏ����̑���
    // �����ŁAPlayerDB��HP���Ď����AHP�̒l�ɕω�����������i�_���[�W���󂯂���j
    // �A�j���[�^�[�̃p�����[�^�[�𑀍삵�A�A�j���[�V�������Đ�����Ƃ�������
    private void DamageActionProcessing()
    {
        // �A�j���[�V�����t���O�������Ă���ꍇ�A�t���O��؂�
        if (animator.GetBool("damage"))
        {
            SetAnimationStatus(AnimatorFlag[3], false);
        }

        // HP�̒l���ϓ��������i�_���[�W���󂯂����j
        if (Hp != playerDB.MyHp)
        {
            // �Ď��pHP�̒l���X�V����
            Hp = playerDB.MyHp;

            // �A�j���[�V�����t���O������
            animator.SetBool(AnimatorFlag[3], true);

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

    // �v���C���[�̃R���g���[�������͂��ꂽ�������֐�
    private bool PressedButton(string _str)
    {
        return Input.GetButton("Player" + PlayerNum + _str);
    }


    private void DebugProcessing()
    {
        // �X�y�[�X�������Ȃ���
        if (Input.GetKey(KeyCode.Space))
        {
            // Z�L�[�Œʏ햂�@�̃A�j���[�V����
            if (Input.GetKey(KeyCode.Z))
            {
                animator.SetBool(AnimatorFlag[0], true);
            }
            // X�L�[�ŏ㖂�@�̃A�j���[�V����
            if (Input.GetKey(KeyCode.X))
            {
                animator.SetBool(AnimatorFlag[1], true);
            }
            // C�L�[�ŉ����@�̃A�j���[�V����
            if (Input.GetKey(KeyCode.C))
            {
                animator.SetBool(AnimatorFlag[2], true);
            }
            // V�L�[�Ń_���[�W���A�N�V�����̃A�j���[�V����
            if(Input.GetKey(KeyCode.V))
            {
                animator.SetBool(AnimatorFlag[3], true);
            }
        }
    }
}
