using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicAnimationManager : MonoBehaviour
{
    // �ϐ��錾
    private Animator animator;          // �A�j���[�V�����t���[����p
    private PlayerDB playerDB;          // �v���C���[�ԍ��擾�p

    // �L�[���͂��v���C���[�X�N���v�g�ƍ��킹��
    [Header("�L�[�R���t�B�O")]
    [SerializeField] private string[] GamepadButton = new string[4] { "A", "B", "X", "Y" };

    private int PlayerNum;          // �v���C���[�ԍ��i�[�p

    private string[] AnimatorFlag = new string[3] {"charge", "up_magic", "down_magic"};         // �A�j���[�^�[��������bool�^�ϐ��̖��O�i�[�p

    // Start is called before the first frame update
    void Start()
    {
        // �X�N���v�g��R���|�[�l���g�̎擾
        animator = GetComponent<Animator>();
        playerDB = GetComponent<PlayerDB>();

        // �v���C���[�ԍ��̐ݒ�(�G�f�B�^���̃L�[�ݒ�ɂ��A�ԍ���1~4�͈̔�)
        PlayerNum = playerDB.MyNumber + 1;
    }

    // Update is called once per frame
    void Update()
    {
        // �A�j���[�V�����J�ڏ����̊Ǘ�����
        MagicAnimationProcessing();

        // �f�o�b�O�p
        DebugProcessing();

    }

    // �A�j���[�V�����̑J�ڏ����̑���
    // �����ŁA���͂��ꂽ�{�^���ɉ����āA�A�j���[�V�����R���g���[���ɂ��Ă���ϐ��ibool�j�𑀍�
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



    // �A�j���[�V�����ϐ��̑���������ȗ������邽�߂̊֐�
    private void SetAnimationStatus(string statusName, bool status)
    {
        if(statusName != null)
        {
            animator.SetBool(statusName, status);
        }
        else
        {
            Debug.Log("�G���[�F�A�j���[�^�[�̃p�����[�^��������ϐ������ݒ�܂��͔z��̗v�f�O");
        }
    }

    // �ǂ̃v���C���[�̃R���g���[�������͂��ꂽ�������֐��Q�i������Ă���A�����ꂽ�A�����ꂽ�j
    private bool PressedButton(string _str)
    {
        return Input.GetButton("Player" + PlayerNum + _str);
    }

    // �f�o�b�O�p�������܂Ƃ߂��֐�
    private void DebugProcessing()
    {
        // �X�y�[�X�������Ȃ���
        if(Input.GetKey(KeyCode.Space))
        {
            // Z�L�[�Œʏ햂�@�̃A�j���[�V����
            if(Input.GetKey(KeyCode.Z))
            {
                animator.SetBool(AnimatorFlag[0], true);
            }
            // X�L�[�ŏ㖂�@�̃A�j���[�V����
            if (Input.GetKey(KeyCode.X))
            {
                animator.SetBool(AnimatorFlag[1], true);
            }
            // C�L�[�ŉ����@�̃A�j���[�V����
            if(Input.GetKey(KeyCode.C))
            {
                animator.SetBool(AnimatorFlag[2], true);
            }
        }
    }

}
