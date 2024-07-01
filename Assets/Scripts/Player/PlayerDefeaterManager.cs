using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefeaterManager : MonoBehaviour
{
    // ���ꂽ�v���C���[�ƌ��j�҂��󂯎��ϐ�
    [HideInInspector] public string DefeaterRecord;       // ���j�҂̖��O
    [HideInInspector] public string VictimRecord;         // ���ꂽ�v���C���[�̖��O

    // ���ꂽ�v���C���[�ƌ��j�҂�R�Â��ĊǗ�����\���̂̐錾
    [HideInInspector] public struct RecordOfDefeats
    {
        public string Deferter;
        public string Victim;
    }
 

    // ���ꂽ�v���C���[�ƌ��j�҂�R�Â��ĊǗ�����\����
    [HideInInspector] public List<RecordOfDefeats> Record;

    // Start is called before the first frame update
    void Start()
    {
        // �ϐ��A�\���̂̏�����
        DefeaterRecord = null;
        VictimRecord = null;

        Record = new List<RecordOfDefeats>();
    }

    // Update is called once per frame
    void Update()
    {
        // �N�������j����A���O��񂪑����Ă�����A���O�����Ǘ��\���̂Ɋi�[����iPlayer����OnTreeger~~���疼�O�������Ă����珈�������s�����j
        if(DefeaterRecord != null && VictimRecord != null)
        {
            RecordAddProcessing();
        }

        DebugProcessing();

    }

    // �N�������j����A���O��񂪑����Ă�����A���O�����Ǘ��\���̂Ɋi�[����֐�
    private void RecordAddProcessing()
    {
        // �f�o�b�O�p�F���̏������J�n���ꂽ�����O���o��
        Debug.Log("ProcStart");

        // ���L���p�\���̂ɖ��O�����i�[
        RecordOfDefeats Temporary = new RecordOfDefeats();
        Temporary.Deferter = DefeaterRecord;
        Temporary.Victim = VictimRecord;

        // �L���p�\���̂ɓ����
        Record.Add(Temporary);

        // �\���̂Ɋi�[������́A�ϐ�������������
        DefeaterRecord = null;
        VictimRecord = null;
    }

    // �����҂̌��j���𐔂��AStatic�ϐ��Ɋi�[����֐�
    public void KillCountProcessing(string WinnerName)
    {
        int WinnerKillCount = 0;

        // �v���C���[���j���Ǘ��\���̂���A�����҂̌��j���𒊏o
        foreach(RecordOfDefeats KillCount in Record)
        {
            if(KillCount.Deferter == WinnerName)
            {
                WinnerKillCount++;
            }
        }

        // �����j���Ə����҂̌��j�����f�o�b�O���O�ɏo��
        Debug.Log("AllKillCount�F" + Record.Count);
        Debug.Log(WinnerName + "�FKillCount = " + WinnerKillCount);

        // SetSingleton����Static�ϐ��ɏ����҂̌��j�����i�[
        GameObject.Find("SceneManager").GetComponent<SetSingleton>().SetWinnerKillCount(WinnerKillCount);
    }


    private void DebugProcessing()
    {
        // List�z��̌��݂̗v�f���Ɗi�[���Ă��閼�O���o�́i���݂̌��j�L�^���f�o�b�O���O�ɏo�́j
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("ListCount�F" + Record.Count);
            foreach (RecordOfDefeats output in Record)
            {
                Debug.Log("defeart " + output.Deferter + " in " + output.Victim);
            }
        }
    }

}
