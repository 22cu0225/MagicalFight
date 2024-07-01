using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaneAlpha : MonoBehaviour
{
    // �ϐ��錾
    [SerializeField] private float ChangeValue;     // �ω���
    [SerializeField] private float ChangeTime;      // �ύX����܂ł̎���

    private float Alpha;                            // �A���t�@�l
    private float Timer;                            // �o�ߎ���

    private bool Changesing;                        // ������ς���t���O
    private float sing;                             // ������ς���ׂ̕ϐ�

    [SerializeField] private float MAX_Limits;      // �A���t�@�l�̍ő�l
    [SerializeField] private float min_Limits;      // �A���t�@�l�̍ŏ��l

    // �R���|�[�l���g�擾�p
    private SpriteRenderer SR;

    private void Start()
    {
        // ������
        Alpha = 0.0f;

        // �R���|�[�l���g�擾
        SR = this.GetComponent<SpriteRenderer>();
        SR.color = new Color(1.0f, 1.0f, 1.0f, Alpha);
    }

    private void Update()
    {
        // �o�ߎ��Ԃ��擾
        Timer += Time.deltaTime;

        // �A���t�@�l�Ńt���O��؂�ւ�
        if      (Alpha >= MAX_Limits) Changesing = false;
        else if (Alpha <= min_Limits) Changesing = true;

        // �t���O�ŕ�����؂�ւ�
        sing = Changesing ? 1.0f : -1.0f;

        // �w�肵�����Ԃ��o�߂�����
        if (Timer >= ChangeTime)
        {
            // �w�肵���������A���t�@�l���X�V
            Alpha += ChangeValue  * sing;
            SR.color = new Color(1.0f, 1.0f, 1.0f, Alpha);

            // Timer ���Z�b�g
            Timer = 0.0f;
        }
    }

    private void FixedUpdate()
    {
        if (Alpha >= MAX_Limits) Alpha = MAX_Limits;
        if (Alpha <= min_Limits) Alpha = min_Limits;
    }
}
