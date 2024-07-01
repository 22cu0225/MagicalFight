using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float damageValue;
    //PlayerDB pDB;
    PlayerGenerate PlayerGenerateScript;
    GameManager GameManagerScript;
    //�v���C���[�̌��݂̐l�����i�[
    int currentPlayerCount;

    //[SerializeField] private Slider hpBar;

    [SerializeField] private GameObject mainCamera;     // ���C���J�����I�u�W�F�N�g
    private CameraShake cameraShake;                    // �J������h�炷�X�N���v�g
    [SerializeField] private float duration = 0.3f;     // ����
    [SerializeField] private float magnitude = 0.6f;    // �h��̑傫��
    [SerializeField] private int OverPower;             // �h���l�ɂȂ�傫��

    void Start()
    {
        PlayerGenerateScript = this.gameObject.GetComponent<PlayerGenerate>();
        GameManagerScript = this.gameObject.GetComponent<GameManager>();
        //�v���C���[�l���̏�����
        currentPlayerCount = GameManagerScript.playerCount;

        cameraShake = mainCamera.GetComponent<CameraShake>();
    }

    void Update()
    {
        //debug
        if (Input.GetKeyDown(KeyCode.O)) Hit(PlayerGenerateScript.GetPlayerList()[0], 5);
        //debug
        if (Input.GetKeyDown(KeyCode.P)) Hit(PlayerGenerateScript.GetPlayerList()[1], 5);
    }

    public void SetHpBar(GameObject _hitObj, float _hp)
    {
        _hitObj.GetComponent<PlayerDB>().MyHpBar.GetComponent<Slider>().value = _hp / _hitObj.GetComponent<PlayerDB>().MyMaxHp;
    }

    //�q�b�g���̏���
    public void Hit(GameObject _hitObj, int _power)
    {
        float hp = _hitObj.GetComponent<PlayerDB>().MyHp;
        if (hp > 0)
        {
            hp -= _power;
            if (hp <= 0) hp = 0;
            _hitObj.GetComponent<PlayerDB>().SetHp(hp);
            SetHpBar(_hitObj, hp);
            Debug.Log(_hitObj.GetComponent<PlayerDB>().MyIsAlive);

        }
        // ���S�A�j���[�V���������̂��߂ɁA�R�����g�A�E�g���܂�
        /*
        if (hp <= 0 && _hitObj.GetComponent<PlayerDB>().MyIsAlive != false)
        {
            Debug.Log("�f�X�g���C�I ���Ăяo��");
            PlayerDestroy(_hitObj);
        }
        */

        // �p���[���w�肵���l�ȏゾ������J������h�炷
        if (_power >= OverPower)
        {
            StartCoroutine(cameraShake.Shake(duration, magnitude));
        }
    }

    //�v���C���[���|���ꂽ��
    public void PlayerDestroy(GameObject _destroyObj)
    {
        //�v���C���[�����S��Ԃɂ���
        _destroyObj.GetComponent<PlayerDB>().SetIsAlive(false);
        //�v���C���[���\��
        _destroyObj.SetActive(false);
        //�v���C���[�ɑΉ�����HP�o�[���\���ɂ���
        _destroyObj.GetComponent<PlayerDB>().MyHpBar.SetActive(false);
        //���݂̃v���C���[�̐l�������炷
        currentPlayerCount--;
        Debug.Log("�f�X�g���C�I");
        //if (currentPlayerCount == 1) GameManagerScript.MidResult();
    }
}
