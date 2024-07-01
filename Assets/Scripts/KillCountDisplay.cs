using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCountDisplay : MonoBehaviour
{
    // �ϐ��錾
    UnityEngine.UI.Text text;       // �e�L�X�g�̕\�����e��ς��邽�߂�Text�R���|�[�l���g���擾����p

    private int KillCount;          // �����҂̌��j�����i�[����p

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g�̎擾
        text = GetComponent<UnityEngine.UI.Text>();

        // �����҂̌��j�����擾
        KillCount = GameObject.Find("SceneManager").GetComponent<SetSingleton>().GetWinnerKillCount();
    }

    // Update is called once per frame
    void Update()
    {
        // �擾������������string�^�Ƃ��Č��j����Text�ɔ��f����
        text.text = "" + KillCount;
    }
}
