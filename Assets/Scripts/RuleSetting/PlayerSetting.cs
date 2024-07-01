using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//---------------------------------------------------
//�������e�����F
//  �v���C���[�̐l���̐ݒ�
//  ����������̐ݒ�
//  �ݒ荀�ڂ�z��ŊǗ�
//  ���݂��鍀�ڂ̐F��ς���
//  ���E�̃{�^�����������Ƃ��ɑΉ�����O�p�`����u�F�ς���
//---------------------------------------------------


public class PlayerSetting : MonoBehaviour
{
    //�V�[���J��
    SceneController SceneControllerScript;


    //���l����      �v�f�ԍ��i�O�F�v���C���[�l�������A�P�F�����_����j
    //-----------------------------------------------------------------
    [SerializeField] int[] upperLmits;          //���
    [SerializeField] int[] underLmits;          //����   
    //-----------------------------------------------------------------


    //�ݒ荀�ڂ̔w�i
    [SerializeField] GameObject[] items;



    //�ݒ荀�ڂ̊Ǘ��ԍ�
    enum ItemNumbers {
        e_playerCount,
        e_maxWinPointsCount,
        e_startButton,
        e_ItemCount
    };

    //���݂̑I�����ڂ̊Ǘ��ԍ����i�[
    int curentItem;

    public static int[] playerSettings;       //0 : �v���C���[�̐l�������ݒ�, 1 : �����_�̐����ݒ�

    //���͊֌W
    //-----------------------------------------------------------------
    [SerializeField] float deadZone;
    [SerializeField] float limit;
    float timer;
    bool canInput = false;
    //-----------------------------------------------------------------

    void Start()
    {
        SceneControllerScript = this.gameObject.GetComponent<SceneController>();

        playerSettings = new int[2] { 
            underLmits[(int)ItemNumbers.e_playerCount],
            underLmits[(int)ItemNumbers.e_maxWinPointsCount]
        };

        curentItem = (int)ItemNumbers.e_playerCount;

        HighlightingCurentItem();
    }

    void Update()
    {

        float dx = Input.GetAxis("Horizontal");
        float dy = Input.GetAxis("Vertical");

        if (!canInput)
        {
            timer += Time.deltaTime;
            if (timer >= limit) canInput = true;
        }
        else
        {
            //��
            if (Input.GetKeyDown(KeyCode.UpArrow) || dy > deadZone)
            {
                //�ꍀ�ڏ��
                if (curentItem > 0)
                {
                    curentItem--;
                }
                HighlightingCurentItem();
                canInput = false;
                timer = 0.0f;
            }
            //��
            else if (Input.GetKeyDown(KeyCode.DownArrow) || dy < -deadZone)
            {
                //�ꍀ�ډ���
                if (curentItem < (int)ItemNumbers.e_ItemCount - 1)
                {
                    curentItem++;
                }
                HighlightingCurentItem();
                canInput = false;
                timer = 0.0f;
            }
           
            if (curentItem < (int)ItemNumbers.e_startButton)
            {
                //��
                if (Input.GetKeyDown(KeyCode.LeftArrow) || dx < -deadZone)
                {
                    //�J�E���g���P���炷
                    if (playerSettings[curentItem] > underLmits[curentItem])
                    {
                        playerSettings[curentItem]--;
                    }

                    canInput = false;
                    timer = 0.0f;
                }
                //�E
                else if (Input.GetKeyDown(KeyCode.RightArrow) || dx > deadZone)
                {
                    //�J�E���g��1���₷
                    if (playerSettings[curentItem] < upperLmits[curentItem])
                    {
                        playerSettings[curentItem]++;
                    }
                    canInput = false;
                    timer = 0.0f;
                }
                //�O�p�`�̐F�ς���
                if (Input.GetKey(KeyCode.RightArrow) || dx > deadZone)
                {
                    VerificationGameobject(items[curentItem], Color.red, "Right");
                }
                else
                {
                    VerificationGameobject(items[curentItem], Color.white, "Right");
                }
                if (Input.GetKey(KeyCode.LeftArrow) || dx < -deadZone)
                {
                    VerificationGameobject(items[curentItem], Color.red, "Left");
                }
                else
                {
                    VerificationGameobject(items[curentItem], Color.white, "Left");
                }

            }
        }

        //����L�[
        // �ǂ̃R���g���[���[�ł��{�^�����͂��E����悤�ɕύX�i��{�j
        if (Input.GetKeyDown(KeyCode.M) || AnyControllerPressed("A"))
        {
            //�{�^��������
            if (curentItem == (int)ItemNumbers.e_startButton)
            {
                SceneControllerScript.LoadNextScene();
            }
            //�ꍀ�ډ���
            if (curentItem < (int)ItemNumbers.e_ItemCount - 1)
            {
                curentItem++;
            }
            HighlightingCurentItem();
        }
    }

    // �I�����ڂ̔w�i�̕\����\�����X�V���鏈��
    void HighlightingCurentItem()
    {
        for (int i = 0; i < (int)ItemNumbers.e_ItemCount; ++i)
        {
            if (i == curentItem)
            {
                //���݂̍��ڂɑ΂��čs������

                ChangeColorOfGameObject(items[i], Color.white);
            }
            else
            {
                //���݂̍��ڈȊO�ɑ΂��čs������
                ChangeColorOfGameObject(items[i], Color.gray);
            }
        }
    }

    public int GetCounter(int _index) { return playerSettings[_index]; }


    private void ChangeColorOfGameObject(GameObject _targetObject, Color _color)
    {
        //���͂��ꂽ�I�u�W�F�N�g��Renderer��S�Ď擾���A����ɂ���Renderer�ɐݒ肳��Ă���SMaterial�̐F��ς���
        foreach (Renderer targetRenderer in _targetObject.GetComponents<Renderer>())
        {
            foreach (Material material in targetRenderer.materials)
            {
                material.color = _color;
            }
        }

        //���͂��ꂽ�I�u�W�F�N�g��Text��S�Ď擾���A�F��ς���
        foreach (Text text in _targetObject.GetComponents<Text>())
        {
            text.color = _color;
        }

        //���͂��ꂽ�I�u�W�F�N�g�̎q�ɂ����l�̏������s��
        for (int i = 0; i < _targetObject.transform.childCount; i++)
        {
            ChangeColorOfGameObject(_targetObject.transform.GetChild(i).gameObject, _color);
        }
    }


    void VerificationGameobject(GameObject _targetObject, Color _color, string _obectName)
    {
        if (_targetObject.name == _obectName)
        {
            ChangeColorOfGameObject(_targetObject, _color);
        }

        //���͂��ꂽ�I�u�W�F�N�g�̎q�ɂ����l�̏������s��
        for (int i = 0; i < _targetObject.transform.childCount; i++)
        {
            VerificationGameobject(_targetObject.transform.GetChild(i).gameObject, _color, _obectName);
        }
    }

    // SceneLoadManager����ǂ̃R���g���[���[�̓��͂��E����֐��������Ă����i��{�j
    public bool AnyControllerPressed(string _str)
    {        
        if (Input.GetButtonDown("Player1" + _str)) return true;
        if (Input.GetButtonDown("Player2" + _str)) return true;
        if (Input.GetButtonDown("Player3" + _str)) return true;
        if (Input.GetButtonDown("Player4" + _str)) return true;
        return false;
    }


}
