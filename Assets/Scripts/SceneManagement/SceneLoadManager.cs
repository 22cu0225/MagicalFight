using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    // �v���p�e�B
    // ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    // SetSingleton�X�N���v�g������ϐ�
    SetSingleton setSingleton;

    // �V�[���J�ڏ������Ǘ�����񋓌^
    public enum SceneLoadConditions
    {
        [InspectorName("�������Ő؂�ւ�")]
        WinCount,
        [InspectorName("�{�^�����͂Ő؂�ւ�")]
        Button,
        [InspectorName("���Ԍo�߂Ő؂�ւ�")]
        TimeCount
    }


    // �V�[���̑J�ڂ̏����A�J�ڐ���Ǘ�����\����(SceneIndex�ƒl����v������)
    // ���L�F�V�[���ꗗ
    // Title       0
    // RuleSeting  1
    // Ingame      2
    // MidResult   3
    // FinalResult 4
    [System.Serializable]
    public struct SceneChangeManager
    {
        [Header("���̃V�[���̑J�ڏ����̐ݒ�")]
        [Header("Element�̔ԍ� = �e�V�[����BuildIndex�̔ԍ�")]
        [Header("���݂̃V�[���̑J�ڏ����̐ݒ�")] public SceneLoadConditions LoadCondition;       // ���̗񋓌^�ϐ��ŃV�[���J�ڂ̏�����؂�ւ��Ă���

        [Header("�J�ڂ���V�[���̐ݒ�i�����ݒ�j")] public string[] ChengeScene;     // �J�ڂ���V�[���̐ݒ�i�����ݒ��)
        
        [Header("�o�ߎ��Ԃ̐ݒ�")] public float TimeCount;               // �J�ڏ����F�o�ߎ��Ԃ�����ϐ�
    }

    [SerializeField] private SceneChangeManager[] Manager;      // �e�V�[�����Ƃ̑J�ڏ����ƑJ�ڐ���Ǘ�����\����

    // ���Ԍo�߂ł̑J�ڗp�ϐ�
    private float TimeCounter;      �@// ���Ԍo�߂őJ�ڂ���V�[���Ōo�ߎ��Ԃ�}�邽�߂̕ϐ�
    private bool TimeFlag = false;    // ���Ԃ��v�����Ă��邩�𔻒f���邽�߂̕ϐ�

    // �������ł̑J�ڗp
    [HideInInspector] public bool WinnerFlag = false;   // �����҂����܂�����True�ɂȂ�A�V�[���J�ڏ������s�킹�邽�߂̕ϐ�

    // �{�^�����͂ł̑J�ڗp
    [HideInInspector] public bool SceneLoadReady;       // �{�^�����͂őJ�ڂ��Ă悢���̊m�F�p�t���O

    // �J�ڐ悪��������V�[���̏ꍇ�̑J�ڐ�ݒ�p�ϐ�
    private int SelectSceneNum;

    // Player.cs���A�L�[�R���t�B�O
    [HideInInspector] string[] GamepadButton = new string[4] { "A", "B", "X", "Y"};

    // ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // ���\�b�h
    // ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // �V�[���̑J�ڏ����ɉ������������܂Ƃ߂����\�b�h
    private void LoadManager(SceneLoadConditions condition)
    {
        switch (condition)
        {
            // �J�ڏ������������̎��̏���
            case SceneLoadConditions.WinCount:

                // �����Ҋm��t���O
                if(WinnerFlag)
                {
                    WinnerFlag = false;
                    LoadScene(Manager[SceneIndex()].ChengeScene[0]);
                }
                break;

            // �J�ڏ������{�^���̓��͂̎��̏���
            case SceneLoadConditions.Button:

                // �J�ډ\�̏ꍇ
                if(SceneLoadReady)
                {
                    // ���݂̃V�[�����^�C�g���̏ꍇ�AABXY�̑S�Ẵ{�^���Ŏ��̃V�[���֑J�ڂ���
                    if(SceneManager.GetActiveScene().name == "Title")
                    {
                        if(PressedAllButtonCheck())
                        {
                            LoadScene(Manager[SceneIndex()].ChengeScene[0]);
                        }                        
                    }

                    // �Ή�����{�^���i���݂͂PP��A�{�^���j����͂���ƃV�[���J�ڏ������s��
                    else if (AnyControllerPressed(GamepadButton[0]))
                    {
                        LoadScene(Manager[SceneIndex()].ChengeScene[0]);
                    }
                }                

                // �V�[���J�ډ̏ꍇ(�Ή�����{�^����������Ă��܂��J�ڂ������Ȃ��ꍇ�A���̏������g��)
                //if(SceneLoadReady)

                break;

            // �J�ڏ��������Ԍo�߂̎�
            case SceneLoadConditions.TimeCount:
                // �V�[���J�n������v���J�n
                if (TimeFlag != true)
                {
                    TimeCounter = Time.time;
                    TimeFlag = true;
                }
                // �ݒ肵�����Ԃ��o�߂�����A�V�[���̑J�ڏ������s���i�v���t���O�ƌv�����Ԃ̓��Z�b�g����j
                if (TimeFlag == true && Manager[SceneIndex()].TimeCount <= (Time.time - TimeCounter))
                {
                    LoadScene(Manager[SceneIndex()].ChengeScene[0]);
                    TimeFlag = false;
                    TimeCounter = 0.0f;
                }

                break;
        }
    }


    // �V�[���J�ڏ����i�����Ŏ󂯎�������O�̃V�[���ɑJ�ڂ���j
    private void LoadScene(string SceneName)        // SceneName�ɑJ�ڐ��n���ہA�󋵂ɉ������J�ڐ��ǂݍ��ނ��߂ɁA�Ή������V�[���̖��O�������Ă���z��ԍ��܂Ŏw�肷��
    {
        Debug.Log("LoadScene�F" + SceneName);
        SceneManager.LoadScene(SceneName);
    }

    // SceneIndex��Ԃ��i���݂̃V�[�����ʗp�j
    private int SceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }


    // �V�[���ԍ��ɉ������J�ڏ���
    // �V�[���J�ڏ����i���݂��玟��Index�̃V�[���ɑJ�ڂ���j
    public void LoadNextScene()
    {
        // ���݂̃V�[���̃C���f�b�N�X���擾
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // ���̃V�[���̃C���f�b�N�X���v�Z
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

        // ���̃V�[���ɑJ��
        SceneManager.LoadScene(nextSceneIndex);
    }

    // �V�[���J�ڏ����i���݂���O��Index�̃V�[���ɑJ�ڂ���j
    public void LoadPrevScene()
    {
        // ���݂̃V�[���̃C���f�b�N�X���擾
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // ���̃V�[���̃C���f�b�N�X���v�Z
        int nextSceneIndex = (currentSceneIndex - 1) % SceneManager.sceneCountInBuildSettings;

        // ���̃V�[���ɑJ��
        SceneManager.LoadScene(nextSceneIndex);
    }


    // �{�^�����͂őJ�ڂ���V�[���ŁA�V�[�����ƂɑJ�ډ\�t���O��ݒ肷�鏈��
    private void SetSceneLoadReady(int index)
    {
        // �V�[���ɉ����đJ�ډ\�t���O�̐ݒ�A�܂��͏�����t����
        switch(index)
        {
            // �^�C�g�����
            case 0:

                SceneLoadReady = true;
                break;

            // ���[���ݒ���
            // ���[���ݒ��ʂ̑J�ڏ����́APlayerSetting�X�N���v�g�ɂ܂Ƃ߂��Ă��邽�߁A���̃X�N���v�g�ł͍s��Ȃ�
            case 1:
                // ���[���ݒ��ʂɗ������AStatic�ϐ��F�����҂̌��j�� �̒l������������
                setSingleton.SetWinnerKillCount(0);
                SceneLoadReady = false;
                break;

            // �ŏI���U���g���
            case 3:

                SceneLoadReady = true;
                break;

            // �{�^�����͂őJ�ڂ���V�[���ȊO�̏ꍇ�́A�t���O�������Ă���
            default:

                SceneLoadReady = false;
                break;
        }
        
        // �V�[���ꗗ�i�{�^�����͂őJ�ڂ���V�[���� �� �j
        // Title       0�@��
        // RuleSeting  1�@��
        // Ingame      2
        // MidResult   3
        // FinalResult 4�@��

    }




    // Player.cs���A�{�^�����͏���(���͂�Player1�̂��̂Ɍ���)
    public bool Pressd(string _str) { return Input.GetButtonDown("Player1" + _str); }

    // �ǂ̃R���g���[���[�ł����͂𔽉f�ł���悤�ɂ���֐�
    public bool AnyControllerPressed(string _str)
    {
        // �ĂP(for����if����p������@)
        /*
        bool flag = false;
        for(int i = 1; i <= 4; i++)
        {
            if (Input.GetButtonDown("Player" + i + _str))
            {
                flag = true;
                break;
            }
        }
        return flag;
        */

        // �ĂQ�iif���݂̂�p������@�j        
        if (Input.GetButtonDown("Player1" + _str)) return true;
        if (Input.GetButtonDown("Player2" + _str)) return true;
        if (Input.GetButtonDown("Player3" + _str)) return true;
        if (Input.GetButtonDown("Player4" + _str)) return true;
        return false;
    }
    
    // ABXY�̂����ꂩ�������ꂽ��True��Ԃ��֐�
    private bool PressedAllButtonCheck()
    {
        bool pressed = false;
        
        for(int i = 0; i < 4 && pressed == false; i++)
        {
            if (Input.GetButtonDown("Player1" + GamepadButton[i])) pressed = true;
            if (Input.GetButtonDown("Player2" + GamepadButton[i])) pressed = true;
            if (Input.GetButtonDown("Player3" + GamepadButton[i])) pressed = true;
            if (Input.GetButtonDown("Player4" + GamepadButton[i])) pressed = true;
        }        
        return pressed;
    }


    // �f�o�b�O�p�����܂Ƃ�
    private void DebugProcessing()
    {
        // F1�������Ƃǂ̃V�[������ł��^�C�g���V�[���ɑJ�ڂ���
        if(Input.GetKeyDown(KeyCode.F1))
        {
            LoadScene("Title");
        }


        // �f�o�b�O�p(�E�V�t�g�������Ȃ���Ή�����L�[�����)
        if (Input.GetKey(KeyCode.RightShift))
        {
            // �f�o�b�O�p�F�V�[���̃r���h�C���f�b�N�X�̏o�́iP�L�[�j
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("BuildIndex�F" + SceneManager.GetActiveScene().buildIndex);
            }

            // �f�o�b�O�p�F���Ԍo�߂őJ�ڂ���V�[���̎��ɁA�V�[���J�n����̌o�ߎ��Ԃ��o�́iI�L�[�j
            if (Input.GetKeyDown(KeyCode.I) && TimeFlag == true)
            {
                Debug.Log("TimeCount�F" + (Time.time - TimeCounter));
            }
            // �f�o�b�O�p�F�V�[���̐؂�ւ�
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                LoadNextScene();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                LoadPrevScene();
            }

            // �f�o�b�O�p�F�V�[���J�ډ\�t���O�̑���
            if (Input.GetKeyDown(KeyCode.Y) && SceneLoadReady == false)
            {
                SceneLoadReady = true;
                Debug.Log("SceneLoadReady�F" + SceneLoadReady);
            }
            if (Input.GetKeyDown(KeyCode.N) && SceneLoadReady == true)
            {
                SceneLoadReady = false;
                Debug.Log("SceneLoadReady�F" + SceneLoadReady);
            }
            // �f�o�b�O�p�F�V�[���J�ڃt���O�̏o��
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("SceneLoadReady�F" + SceneLoadReady);
            }

            // �{�^�����͑J�ڂ̃V�[���̎�
            if (Manager[SceneIndex()].LoadCondition == SceneLoadConditions.Button)
            {
                if (SceneLoadReady)
                {
                    // �f�o�b�O�p�F�L�[�{�[�h�̓��͂Ń{�^�����͂̃V�[���J�ڂ��s���ꍇ��B�L�[
                    if (Input.GetKeyDown(KeyCode.B))
                    {
                        LoadScene(Manager[SceneIndex()].ChengeScene[0]);
                    }
                }
            }

            // �i�[���������v���C���[�̗v�f�ԍ����o�͂���
            if(Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("WinnerPlayerNumber�F" + setSingleton.GetWinnerNum());
            }
        }
    }

    // -------------------------------------------------------------------------------------------------------------------

    private void Start()
    {
        setSingleton = GetComponent<SetSingleton>();
    }


    void Update()
    {
        // �V�[���ɉ������J�ڏ���
        LoadManager(Manager[SceneIndex()].LoadCondition);

        // �{�^�����͂őJ�ڂ���V�[�����̑J�ڃt���O�̊m�F�Ɛݒ�
        SetSceneLoadReady(SceneIndex());

        // �f�o�b�O����
        DebugProcessing();        
    }
}

// ����
// 2024/01/19
// �X�V�F�������ɂ���čs����V�[���J�ڏ����̎���
// 
// 2024/01/22
// �X�V�F�{�^���ł̃V�[���J�ڂ��s���������A���u����space�L�[����APlayer1��B�{�^�����͂Ŏ��s�ɕύX
// 
// 2024/01/26
// �X�V�F�{�^���ł̃V�[���J�ڂ��s���ہA�V�[�����ƂɑJ�ډ\�ȏ�����ݒ�ł���悤�ɏ�����ǉ��iSetSceneLoadReady�j
// �@�@�@���[���ݒ��ʂ̏ꍇ�A��ɐi�ރ{�^�����I������Ă���Ƃ��i�I�u�W�F�N�g���\������Ă���ԁjB�{�^�����͂őJ�ڂ���悤�ɂȂ��Ă���
// �C���F�R�[�h�̃R�����g�AHeader�ł̃C���X�y�N�^�[��ł̕\�����e���ꕔ�ύX
// �@�@�@�f�o�b�O�p�������P�̊֐��ɂ܂Ƃ߂�悤�ɕύX
//  


//// ���݂̃V�[���ɉ����đJ�ڏ�����ݒ肷�郁�\�b�h
//private SceneLoadConditions SetCondition(string ActiveSceneName)
//{
//    switch (ActiveSceneName)
//    {
//        case "Title":

//            break;

//        case "RuleSetting":

//            break;

//        case "InGame":

//            break;

//        case "MidResult":

//            break;

//        case "FinalResult":

//            break;
//    }


//    return 0;
//}