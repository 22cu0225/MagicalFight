using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �R���g���[���[�̓��͂��󂯓n������
public class ControllerInput : MonoBehaviour
{
    //--------------------------------------------
    [Header("�L�[�R���t�B�O")]
    [SerializeField] string[] GamepadButton = new string[6] { "A", "B", "X", "Y", "LB", "RB" };
    private bool[] holdButton = new bool[6];
    //--------------------------------------------

    // �X�e�B�b�N����
    public Vector2 Stick(int _playerNumber) { return new Vector2(Input.GetAxis("Player" + _playerNumber + "Horizontal"), -Input.GetAxis("Player" + _playerNumber + "Vertical")); }

    /// <summary>
    /// �{�^���������ꂽ��
    /// </summary>
    /// <param name="_str">�{�^����</param>
    /// <param name="_playerNumber">�v���C���[�ԍ�</param>
    /// <returns></returns>
    public bool Pressd(string _str, int _playerNumber) { return Input.GetButtonDown("Player" + _playerNumber + _str); }

    /// <summary>
    /// �{�^���������ꂽ��
    /// </summary>
    /// <param name="_str">�{�^����</param>
    /// <param name="_playerNumber">�v���C���[�ԍ�</param>
    /// <returns></returns>
    public bool Release(string _str, int _playerNumber) { return Input.GetButtonUp("Player" + _playerNumber + _str); }

    public bool Pushing(string _str, int _playerNumber)
    {
        

        for (int i = 0; i < holdButton.Length; i++)
        {
            if (Pressd(_str, _playerNumber))
            {
                if (!holdButton[i])
                {
                    holdButton[i] = true;
                }
            }

            if (Release(_str, _playerNumber))
            {
                if (holdButton[i])
                {
                    holdButton[i] = false;
                }
            }
        }
        return holdButton[0];
    }
}
