using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MidResultManager : MonoBehaviour
{
    // �ϐ��錾
    // -------------------------------------------------------------------------------------------------------------------

    // �v���C���[�I�u�W�F�N�g�̎擾�p�ϐ�
    [HideInInspector] public List<GameObject> pObj;
    // ���ԃ��U���g�̕\���I�u�W�F�N�g�̎擾�p�ϐ�
    //[HideInInspector] public GameObject midObj;

    // ���ԃ��U���g�Ŋe�v���C���[�����̕\���I�u�W�F�N�g�擾�p�ϐ�
    [HideInInspector] public List<GameObject> winnnerObj;

    // ���ԃ��U���g�ŕ\��������������p�I�u�W�F�N�g�ϐ�
    [HideInInspector] public GameObject DrowObj;

    // �e�X�N���v�g�擾�p
    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public InstanceObjectManager instanceObjectManager;
    [HideInInspector] public PlayerDefeaterManager playerDefeaterManager;

    // �����擾�������
    [HideInInspector] public List<PlayerDB> playerDB;
    [HideInInspector] public List<SpriteRenderer> spriteRenderer;
    [HideInInspector] public List<Animator> animator;
    [HideInInspector] public List<PlayerDeadProcessing> playerDeadProcessing;
    [HideInInspector] public List<Scaffold_p> scaffold_p;

    

    // ���Q�[���̎Q���l�����C���Q�[���J�n���ɓ���Ă����ϐ�
    private int PlayerMemberNum;

    // �v���C���[�̐������y�ѐ������Ă���v���C���[�̔ԍ��Ɛ����m�F�p�ϐ�
    private int PlayerAliveCounter;
    private int PlayerAliveNum;
    [HideInInspector] public List<bool> PlayerAliveChack;

    // �e�v���C���[�̊J�n���̍��W�����Ă����ϐ�(�C���Q�[�����g���C�������Ƀv���C���[���X�^�[�g�ʒu�ɔz�u���邽��)
    [HideInInspector] public List<Vector3> PlayerStartPos;     

    // ���ԃ��U���g�J�n�t���O
    private bool MidResultFlag = false;

    // �v���C���[�f�[�^�擾�m�F�p�t���O
    private bool GetPlayerDBChack = false;

    // ���ԃ��U���g�������d�����čs��Ȃ��悤�ɂ��邽�߂̊m�F�p�t���O
    private bool MidResultEndFlag = false;

    // ���ԃ��U���g�̕\������
    [SerializeField] private float MidResultTime;


    // (��)
    //[SerializeField,Header("�ő叟�����̉��u��")] private int WinPoint;

    // -------------------------------------------------------------------------------------------------------------------
    // �֐�
    // -------------------------------------------------------------------------------------------------------------------

    // �C���Q�[���̃��g���C����
    void InGameRetry()
    {
        // �v���C���[�����ɍs�����g���C����
        for(int i = 0; i < PlayerMemberNum; i++)
        {
            pObj[i].SetActive(true);                            // ���S���A��\���ɂ��ꂽ�v���C���[�I�u�W�F�N�g��\������iSetActive��true�ɐݒ�j

            // ���S�A�j���[�V��������̃t���O�ݒ� ----------------------
            
            animator[i].SetBool("dead", false);                 // �A�j���[�V�����t���O�F���S�A�j���[�V������؂�

            playerDeadProcessing[i].PlayerDestroy = false;

            playerDeadProcessing[i].Alpha = 1.0f;               // �v���C���[�I�u�W�F�N�g�̕s�����x��߂��i��������Ȃ����j

            spriteRenderer[i].color = new Color(spriteRenderer[i].color.r, spriteRenderer[i].color.g, spriteRenderer[i].color.b, 1.0f);

            // -------------------------------------------------------

            playerDB[i].SetIsAlive(true);                       // PlayerDB���̐��������true�ɖ߂�

            playerDB[i].SetHp(playerDB[i].MyMaxHp);             // �v���C���[��HP�������l�ɖ߂�

            pObj[i].transform.position = PlayerStartPos[i];     // �v���C���[�̈ʒu�������n�_�ɖ߂�

            scaffold_p[i].SetActive_Sca(false);                 // ���蔲�����̏����Ɏg���Ă���ϐ��̏�����

            scaffold_p[i].SetOn_Sca(false);                     // ���蔲�����̏����Ɏg���Ă���ϐ��̏�����

            PlayerAliveChack[i] = true;                         // ��������t���O��true�ɖ߂�

            playerDB[i].MyHpBar.SetActive(true);                // �e�v���C���[�ɑΉ�����HP�o�[��SetActive��true�ɂ���i�\�������悤�ɂ���j

            playerDB[i].MyHpBar.GetComponent<Slider>().value = 1; //hp�o�[�̕\�����X�V

        }

        // �V�[���S�̂̃��g���C����
        // ���ԃ��U���g�̕\��������
        if(PlayerAliveNum != -1)
		{
            instanceObjectManager.ObjectSetActive(winnnerObj[PlayerAliveNum], false);
        }
		else    // ���������̏ꍇ
		{
            instanceObjectManager.ObjectSetActive(DrowObj, false);
		}

        // �e��t���O�ƒl��������
        PlayerAliveCounter = PlayerMemberNum;
        MidResultFlag = false;
        MidResultEndFlag = false;
        PlayerAliveNum = 0;

        // ���g���C�������J��Ԃ��Ăяo����Ȃ��悤�ɂ��邽��Invoke���L�����Z��
        CancelInvoke();

        // ����̏����񐔂𖞂������v���C���[�����݂���ꍇ                
        for(int i = 0; i < PlayerMemberNum; i++)
        {
            // �e�v���C���[�̏����񐔂����O�ɏo��
            Debug.Log("Player" + i + ":winpoint = " + playerDB[i].MyWinPoints);

            // GameManager���̍ő叟�����̒l�ȏ�ɁA�����ꂩ�̃v���C���[�̏����������B������A���̃v���C���[�̗v�f�ԍ���SetSingleton���̕ϐ��Ɋi�[���A�V�[����J�ڂ�����
            if (playerDB[i].MyWinPoints >= gameManager.GetMaxWinPoints())
            {
                // �����v���C���[�̌��j���𐔂���(�ʃX�N���v�g�̊֐����N��)
                playerDefeaterManager.KillCountProcessing(pObj[i].name);

                GameObject.Find("SceneManager").GetComponent<SetSingleton>().SetWinnerNum(i);
                GameObject.Find("SceneManager").GetComponent<SceneLoadManager>().WinnerFlag = true;

                break;
            }
        }
    }
    
    // ���ԃ��U���g����
    void MidResult()
    {
        // ���ԃ��U���g�������s���Ă��Ȃ��ꍇ
        if (MidResultEndFlag == false)
        {            
            Debug.Log("MidResult");

            // �������Ă���v���C���[�̗v�f�ԍ����擾����
            PlayerAliveNum = GetPlayerAliveNum();

            if(PlayerAliveNum != -1)
			{
                // �������Ă���v���C���[�ɏ����_���P�t�^����
                playerDB[PlayerAliveNum].SetWinPoints(playerDB[PlayerAliveNum].MyWinPoints + 1);

                // ���ԃ��U���g�̕\��
                instanceObjectManager.ObjectSetActive(winnnerObj[PlayerAliveNum], true);
            }
			else
			{
                // ���������̕\��������
                instanceObjectManager.ObjectSetActive(DrowObj, true);
			}



            // �Q�[���}�l�[�W���[���̒��ԃ��g���C�������s��
            //gameManager.MidResult();
            
            // ���ԃ��U���g���������t���O��true�ɐݒ�
            MidResultEndFlag = true;
        }

        // Invoke���g�p���A4�b��ɃC���Q�[�����g���C�������s��
        Invoke("InGameRetry", MidResultTime);

    }

    // �e�v���C���[�����m�F����
    void PlayerChack()
    {
        for(int i = 0; i < PlayerMemberNum; i++)    // �v���C���[�Q���������ݐ������Ă��邩���m�F����
        {
            if(playerDB[i].MyHp <= 0 && PlayerAliveChack[i] == true)    // ���ݐ������Ă��肩�AHP���O�ɂȂ����v���C���[�̏ꍇ
            {
                // �������J�E���g�ϐ����f�N�������g���A���̗v�f�ԍ��̃v���C���[�I�u�W�F�N�g�̐����t���O��false�ɂ���
                PlayerAliveCounter--;
                PlayerAliveChack[i] = false;

                Debug.Log("Player" + i + "�FisDead");
            }
        }

        if(PlayerAliveCounter <= 1)       // �������Ă���v���C���[�̐���1�l�ȉ��ɂȂ����ꍇ
        {
            MidResultFlag = true;       // ���ԃ��U���g�����J�n�t���O���グ��
        }
    }

    // �����c�����v���C���[�̗v�f�ԍ���߂�l�Ƃ��ĕԂ�����
    int GetPlayerAliveNum()
    {
        // �v���C���[���̕������J��Ԃ��A�������Ă���v���C���[��������߂�l�Ƃ��ėv�f�ԍ���Ԃ�
        for (int i = 0; i < PlayerMemberNum; i++)
        {
            if(PlayerAliveChack[i] == true)
            {
                return i;
            }
        }
        // ���ɐ������Ă���v���C���[�����Ȃ������ꍇ��-1��Ԃ�
        return -1;
    }

    // �v���C���[�I�u�W�F�N�g�Ɗe�v���C���[�̏����Ǘ�����X�N���v�g�ƕK�v�ȏ����擾
    void GetPlayersComponent()
    {
        // ---------------------------------------------------------------------------------------
        // �v���C���[�I�u�W�F�N�g�̖��̕ύX�ɔ����A�v���C���[�I�u�W�F�N�g�̎擾�^�C�~���O�Ə�����ύX
        pObj.Add(GameObject.Find("TaikiAka_0(Clone)"));
        pObj.Add(GameObject.Find("TaikiAo_0(Clone)"));
        pObj.Add(GameObject.Find("TaikiKiro_0(Clone)"));
        pObj.Add(GameObject.Find("TaikiMidori_0(Clone)"));
        // ---------------------------------------------------------------------------------------

        // �v���C���[�̎Q���l���ɉ�����
        for (int i = 0; i < PlayerMemberNum; i++)
        {
            Debug.Log("forChack�F" + i);

            //pObj.Add(GameObject.Find("Player(Clone)"));             // Player(Clone)�Ƃ������O�̃I�u�W�F�N�g��T���A�擾����i�v���C���[�I�u�W�F�N�g�̎擾�j

            pObj[i].name = "Player" + i;                            // ���O�Ńv���C���[�I�u�W�F�N�g�̎擾������ۂɏd�����Ȃ��悤�Ƀv���C���[�I�u�W�F�N�g�̖��O��ύX����i�v�f�ԍ���U��j

            if(pObj[i] != null)
            {
                Debug.Log("pObj[i]_NULLCheck = true");

                // �e�v���C���[�ɃA�^�b�`���ꂽ�X�N���v�g���擾����
                playerDB.Add(pObj[i].GetComponent<PlayerDB>());                         // i�Ԗڂ̃v���C���[�I�u�W�F�N�g�� PlayerDB �X�N���v�g���擾����
                spriteRenderer.Add(pObj[i].GetComponent<SpriteRenderer>());             // �X�v���C�g�����_���[���擾
                animator.Add(pObj[i].GetComponent<Animator>());                         // �A�j���[�^�[���擾
                playerDeadProcessing.Add(pObj[i].GetComponent<PlayerDeadProcessing>()); // ���S���A�j���[�V�����������擾
                scaffold_p.Add(pObj[i].GetComponent<Scaffold_p>());                     // ���蔲���������擾

                PlayerStartPos.Add(pObj[i].transform.position);         // i�Ԗڂ̃v���C���[�̃X�^�[�g���_�̍��W���擾���� �� ���̍s�ŃG���[���������iNullReferenceException: Object reference not set to an instance of an object�j
                                                                        //                                               �� �����FPlayerStartPos�̃A�N�Z�X�w��q��public�ɕύX

                PlayerAliveChack.Add(true);                             // i�Ԗڂ̃v���C���[�̐�������t���O��true�ɐݒ肷��

                Debug.Log("SetPlayer" + i + "PlayerDB");
            }            
        }

        // �v���C���[�����J�E���^�[�ϐ��̒l�ɁA�Q���l����������
        PlayerAliveCounter = PlayerMemberNum;

        // �v���C���[���ݒ�m�F�t���O��true�ɂ���
        GetPlayerDBChack = true;
    }

    // -------------------------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    // �J�n������
    void Start()
    {
        // �Q�[���}�l�[�W���[�X�N���v�g���擾
        gameManager = GetComponent<GameManager>();
        // �I�u�W�F�N�g�����X�N���v�g���擾
        instanceObjectManager = GetComponent<InstanceObjectManager>();
        // ���j�҂ƌ��j���̊Ǘ��X�N���v�g�擾
        playerDefeaterManager = GetComponent<PlayerDefeaterManager>();

        // �v���C���[�������m�F�p�ϐ��̒l�ɁA�Q���l�����̒l����
        PlayerMemberNum = gameManager.playerCount;

        Debug.Log("PlayerCount = " + gameManager.playerCount);
        Debug.Log("PlayerMemberNum = " + PlayerMemberNum);

        // ���ԕ\���̂��߂̃I�u�W�F�N�g�𐶐����Ă���
        instanceObjectManager.InstanceObject();

        // ���ԃ��U���g�ŕ\������I�u�W�F�N�g��Active��Ԃ̑�����ȒP�ɂ��邽�߁A���������I�u�W�F�N�g���擾
        //midObj = instanceObjectManager.GetInstansedObject(0);

        // ���ԃ��U���g�ŕ\������I�u�W�F�N�g��Active��Ԃ̑�����ȒP�ɂ��邽�߁A���������e�I�u�W�F�N�g���擾
        for(int i = 0; i < PlayerMemberNum; i++)
        {
            winnnerObj.Add(instanceObjectManager.GetInstansedObject(i));
        }

        // ���ԃ��U���g�ŕ\����������������̃I�u�W�F�N�g���擾
        DrowObj = instanceObjectManager.GetInstansedObject(4);

        // ���ԃ��U���g���Ԃ����ݒ�̏ꍇ�A�����l�Ƃ��ĂS�b������
        if(MidResultTime <= 0)
        {
            MidResultTime = 4.0f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // �v���C���[�Q�����Ɗe�v���C���[���̐ݒ�i���񏈗��j
        if(GetPlayerDBChack == false)
        {
            GetPlayersComponent();
        }

        // �v���C���[�̐����󋵂̊Ď�
        PlayerChack();

        // ���ԃ��U���g�J�n�t���O���������璆�ԃ��U���g����
        if(MidResultFlag == true)
        {
            MidResult();
        }

        // --------------------------------------------------------------------------------------------------

        // �f�o�b�O�p(���V�t�g�������Ȃ���Ή�����L�[����͂ŏ������s)
        if(Input.GetKey(KeyCode.LeftShift))
        {
            // �v���C���[�P��HP���O�ɂ���
            if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.Log("Input_K");
                pObj[0].GetComponent<PlayerDB>().SetHp(0);
                Debug.Log("Player0_HP:" + pObj[0].GetComponent<PlayerDB>().MyHp);
            }
            // �v���C���[�Q��HP���O�ɂ���
            if(Input.GetKeyDown(KeyCode.O))
            {
                Debug.Log("Input_O");
                pObj[1].GetComponent<PlayerDB>().SetHp(0);
                Debug.Log("Player1_HP:" + pObj[1].GetComponent<PlayerDB>().MyHp);
            }
            // �v���C���[�R��HP���O�ɂ���
            if (Input.GetKeyDown(KeyCode.J))
            {
                Debug.Log("Input_J");
                pObj[2].GetComponent<PlayerDB>().SetHp(0);
                Debug.Log("Player2_HP:" + pObj[2].GetComponent<PlayerDB>().MyHp);
            }
            // �v���C���[�S��HP���O�ɂ���
            if (Input.GetKeyDown(KeyCode.I))
            {
                Debug.Log("Input_I");
                pObj[3].GetComponent<PlayerDB>().SetHp(0);
                Debug.Log("Player3_HP:" + pObj[3].GetComponent<PlayerDB>().MyHp);
            }
            // �v���C���[�Q�����ƃv���C���[�����������O�ɏo��
            if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log("PlayerAliveCounter:" + PlayerAliveCounter);
                Debug.Log("PlayerMemberNum:" + PlayerMemberNum);
            }
            // �v���C���[�P�̍��W�����O�ɏo��
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("Player0_Pos:" + pObj[0].transform.position);
            }
        }
        
    }
}

// ����
// ���ԃ��U���g�����F�����i2023/12/25�j
// 
// �����_�ł̌��O�_
// �Q���v���C���[�����������Ƃ��ɉ�������G���[���������邩������Ȃ�
// �i�Q���v���C���[���Q�l�ł����e�X�g�ł��Ă��Ȃ����߁j
// 
// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//
// 2024/01/12
// �X�V�F�v���C���[�I�u�W�F�N�g�̖��̕ύX�ɔ����A�v���C���[�I�u�W�F�N�g�̎擾�^�C�~���O�y�я�����ύX
// 
// 2024/01/19
// �X�V�F���������v���C���[�ɉ����āA���ԃ��U���g�ŕ\��������e���ς�����悤�ɕύX
// �������e�X�g�̓s����AGameManager,PlayerManager�ł̒��ԃ��U���g�������R�����g�A�E�g���܂����iGameManager -> /* MidResurt() */�@PlayerManager -> //if (currentPlayerCount == 1) GMScript.MidResult();�j 
// �@Git��ł� GameManager,PlayerManager �̕ύX���R�~�b�g���Ȃ��̂ŁA���̃X�N���v�g�𐳏�ɓ��삳����ꍇ�́A��L�̏����̃R�����g�A�E�g�����肢���܂�
// 
// 2024/01/22
// �X�V�F���ԃ��U���g���������s����������A�v���C���[���c��1�l�̎�����v���C���[���c��P�l�ȉ��̎��ɕύX
// 
// 2024/01/26
// �X�V�F�ő叟�����̒l���A�Q�[���}�l�[�W���[�X�N���v�g����擾����悤�ɕύX�iGetMaxWinPoints()���g�p�j
// �C���F�R�[�h�̃R�����g���ꕔ�ύX
// 
// 2024/01/29
// �X�V�F���������v���C���[�̗v�f�ԍ���SetSingleton�X�N���v�g����WinnerNum�ϐ��ɓn�������̒ǉ�
// �@�@�@�f�o�b�O�p�����ɁA�v���C���[�R�C�v���C���[�S��HP���O�ɂ��鏈����ǉ�
// 
// 2024/02/08
// �X�V�FScaffold_p���擾���A���蔲�����̏����Ɏg���Ă���ϐ��̏�����������ǉ�
// 