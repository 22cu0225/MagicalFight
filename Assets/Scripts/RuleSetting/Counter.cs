using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    [SerializeField]GameObject obj;
    PlayerSetting PlayerSettingScript;

    [SerializeField] int num;

    //public GameObject counter_object = null;
    void Start()
    {
        PlayerSettingScript = obj.GetComponent<PlayerSetting>();
    }

    void Update()
    {
        // �I�u�W�F�N�g����Text�R���|�[�l���g���擾
        Text counter_text = this.GetComponent<Text>();
        // �e�L�X�g�̕\�������ւ���
        counter_text.text = PlayerSettingScript.GetCounter(num).ToString();
    }
}
