using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadProcessing : MonoBehaviour
{
    // �e�X�N���v�g��R���|�[�l���g�̎擾�p�ϐ�
    [HideInInspector] public PlayerDB playerDB;
    [HideInInspector] public PlayerManager playerManager;
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    // �I�u�W�F�N�g���i�[����ϐ�
    public GameObject DeadObj;
    public GameObject PlayerObj;

    // �g�����X�t�H�[�����i�[����ϐ�
    Transform PlayerTransform;

    // �v���C���[���S���ɍs������
    public bool PlayerDestroy;

    // �A���t�@�l�̑���̂��߂̕ϐ�
    [HideInInspector] public float Alpha;
    public float AlphaSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // �X�N���v�g�A�R���|�[�l���g�̎擾
        playerDB = GetComponent<PlayerDB>();
        playerManager = GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // �G�t�F�N�g�����ʒu�̂��߂Ƀv���C���[�̍��W�擾
        PlayerTransform = transform;

        // �s�����x����̂��߂Ɍ��݂̕s�����x�̎擾
        Alpha = spriteRenderer.color.a;

        // �v���C���[�j�󏈗��̂P��̂ݍs�����߂̃t���O
        PlayerDestroy = false;
    }

    // Update is called once per frame
    void Update()
    {
        // �v���C���[��HP���O�ȉ��ɂȂ����ꍇ
        if(playerDB.MyHp <= 0)
        {
            // ���S���̃A�j���[�V������G�t�F�N�g�̐����A�s�����x���쏈���Ȃ�
            ProcPlayerDeathAnim();
        }        

        // �s�����x���O�ȉ��ɂȂ�A�������肪false�ɂȂ�����
        if (Alpha <= 0.0f && PlayerDestroy == false && playerDB.MyIsAlive == false)
        {
            // �v���C���[���S���̏�����A�A�j���[�V�������Đ���̌㏈��
            ProcPlayerDestroy();
        }
    }

    void ProcPlayerDeathAnim()
    {
        // �g�����X�t�H�[�����قȂ镨�ɂȂ��Ă��܂����ꍇ�ɍĐݒ�
        if (PlayerTransform != transform)
        {
            PlayerTransform = transform;
        }

        // �G�t�F�N�g�����ƃA�j���[�V�����ݒ�
        // ���S����̍X�V�����̃^�C�~���O�ōs��
        if (playerDB.MyIsAlive == true)
        {
            Instantiate(DeadObj, PlayerTransform);
            animator.SetBool("dead", true);

            // �v���C���[�����𔻒肷��ϐ���false�ɐݒ�i���S����̍X�V�j
            playerDB.SetIsAlive(false);

        }
        // HP���O�ȉ��̎����s�����x���O���傫����
        // �s�����x�̑���(0�������Ȃ��l�ɒl�̒���)
        if (playerDB.MyHp <= 0 && Alpha > 0.0f)
        {
            // �s�����x��������i�����鑬�x�̓C���X�y�N�^�[��Œ����\�j
            Alpha -= AlphaSpeed;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Alpha);
        }
    }

    void ProcPlayerDestroy()
    {        
        // �s�����x���O��������Ă����ꍇ�A�O�ɍĐݒ肷��
        if (spriteRenderer.color.a < 0.0f)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.0f);
        }

        // �A�j���[�V�����ݒ�ϐ��i���S�A�j���[�V�����j��false�ɐݒ肵�A���S�G�t�F�N�g��j������i�j�����Ȃ��Ǝc�葱���Ă��܂����߁j
        animator.SetBool("dead", false);

        // �v���C���[���S���̏����iPlayerManager�ōs���Ă��������������ōs���j
        if (PlayerDestroy == false)
        {
            // 1��̂ݍs�����߃t���O�̐ݒ�i���g���C����MidResultManager�ɂ��false�ɖ߂�j
            PlayerDestroy = true;

            // �v���C���[���S���̏��� -------------------------

            // �v���C���[�I�u�W�F�N�g�̔�\��
            PlayerObj.SetActive(false);
            // HP�o�[�̔�\��
            playerDB.MyHpBar.SetActive(false);

            // ----------------------------------------------

            // ����ɓ��삵�Ȃ��������߃R�����g�A�E�g�iNULLReference�̔����j
            //playerManager.PlayerDestroy(PlayerObj);
        }
    }
}
