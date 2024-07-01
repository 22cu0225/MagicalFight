using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSingleton : MonoBehaviour
{
    private static SetSingleton instance;
    public static int WinnerNum;            // �����҂̔ԍ�
    public static int WinnerKillCount;      // �����҂̌��j��

    // Start is called before the first frame update
    private void Awake()
    {
        CheckInstance();
    }

    private void CheckInstance()
    {
        // ���g���C���X�^���X���A�V�[�����܂����ł��j������Ȃ��悤�ɂ���iDontDestroyOnLoad�j
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        // ���Ɏ��g�����݂���ꍇ�A���g��j������
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetWinnerNum(int Num)
    {
        WinnerNum = Num;
    }

    public int GetWinnerNum()
    {
        return WinnerNum;
    }

    public void SetWinnerKillCount(int Num)
    {
        WinnerKillCount = Num;
    }

    public int GetWinnerKillCount()
    {
        return WinnerKillCount;
    }
}

// 2024/01/15 ����̃V�[���ɑJ�ڂ����ۂɃV�[���J�ڗp�I�u�W�F�N�g�����B���Ă��܂����ۂ̏C��
//          �@�ϐ� inctance �� static ��ǉ�
// 2024/01/29
// �X�V�FIngame�I�����A���������v���C���[�̗v�f�ԍ���static�ϐ��Ɋi�[���鏈���A�߂�l�Ƃ��ĕԂ�������ǉ�
// �@�@�@���������v���C���[�̔ԍ����g���ꍇ�͂��̃X�N���v�g��ϐ��Ŏ擾���AGetWinnerNum()���g��