using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // �����_���
    public int maxWinPoints;
    public int GetMaxWinPoints() { return maxWinPoints; }

    // Player�̐l�����i�[����ϐ�
    public int playerCount;
    public int GetPlayerCount() { return playerCount; }

    // �eplayer�̏��������i�[����z��
    List<int> winsCounterList = new List<int>();
    // �eplayer�̏��������i�[����z��
    List<bool> isAliveCheckList = new List<bool>();

    PlayerGenerate PlayerGenerateScript;

    // �X�e�[�W��Prefab���i�[����z��
    [SerializeField] private�@List<GameObject> stageList = new List<GameObject>();

    private void Awake()
    {
        playerCount = PlayerSetting.playerSettings[0];
        maxWinPoints = PlayerSetting.playerSettings[1];
    }

    void Start()
    {
        // Player �̐l�����擾
        Debug.Log("Count of players :" + playerCount);

        PlayerGenerateScript = this.gameObject.GetComponent<PlayerGenerate>();

        // �v���C���[���Ƃɏ�������������
        //for (int i = 0; i < playerCount;i++) winsCounterList.Add(0);
        //foreach (GameObject player in PGScript.PlayerList)
        //{
        //    isAliveCheckList.Add(player.GetComponent<PlayerDB>().MyIsAlive);
        //    winsCounterList.Add(player.GetComponent<PlayerDB>().MyWinPoints);
        //}
        Instantiate(stageList[0], Vector3.zero, transform.rotation);
    }

    void Update()
    {
        //if(�v���C���[�̐l������l�ɂȂ�����) 
        //{
        //    ���������J�E���g����i�e�X�̏����_�����Z�j
        //    �Q�[�����ăX�^�[�g������
        //}

        //// �i�f�o�b�O�p�j�����_���ǉ����ꂽ���̏���
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    winsCounterList[0]++;
        //    Debug.Log("Count of 1P's winPoins :" + winsCounterList[0]);
        //    Debug.Log("Count of 2P's winPoins :" + winsCounterList[1]);
        //    MidResult();
        //}
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    winsCounterList[1]++;
        //    Debug.Log("Count of 1P's winPoins :" + winsCounterList[0]);
        //    Debug.Log("Count of 2P's winPoins :" + winsCounterList[1]);
        //    MidResult();
        //}


    }

    /*
    public void MidResult()
    {
        // ���݂̃X�e�[�W���ړ�����
        // ���ʂ�\������
        // ���̃X�e�[�W�����[�h����
        Debug.Log("Display the MidResults screen. ");


        int playerNum = -1;        // ���ԃ`�����s�����̗v�f�ԍ����i�[����ϐ�

        // ���ԃ`�����s�����̗v�f�ԍ����i�[����B
        foreach (GameObject player in PlayerGenerateScript.GetPlayerList())
        {
            playerNum++;
            //�������Ă���v���C���[���������ꍇ�Ƀ��[�v�𔲂���
            if (player.GetComponent<PlayerDB>().MyIsAlive != false) break;
        }
        Debug.Log(playerNum);

        // playerNum�Ƀ`�����s�����̗v�f�ԍ����i�[���ꂽ�ꍇ�̏���
        if (playerNum >= 0 && playerNum < playerCount)
        {
            //���݂̏����_������Z
            int winPoints = PlayerGenerateScript.GetPlayerList()[playerNum].GetComponent<PlayerDB>().MyWinPoints + 1;
            //�����_�𔽉f
            PlayerGenerateScript.GetPlayerList()[playerNum].GetComponent<PlayerDB>().SetWinPoints(winPoints);
            Debug.Log(PlayerGenerateScript.GetPlayerList()[playerNum] + "�̏����_�F " + winPoints);
        }
        //�����_������ɒB�����Ƃ��̏���
        if (PlayerGenerateScript.GetPlayerList()[playerNum].GetComponent<PlayerDB>().MyWinPoints == maxWinPoints)
        {
            FinalResult();        // ���U���g�\�����I��������Ă�

        }
    }
    */

    void FinalResult()
    {
        //int playerNum;        // �`�����s�����̗v�f�ԍ����i�[����ϐ�

        //// �`�����s�����̗v�f�ԍ����i�[����B�`�����s�����o���O��-1��Ԃ�
        //playerNum = winsCounterList.IndexOf(MaxWinPoints);

        //// playerNum�Ƀ`�����s�����̗v�f�ԍ����i�[���ꂽ�ꍇ�̏���
        //if(playerNum != -1)
        //{
        //    // ���U���g��ʂɑJ�ڂ���
        //    SceneManager.LoadScene("FinalResult");
        //}

        Debug.Log("Display the FinalResults screen. ");

    }
}
