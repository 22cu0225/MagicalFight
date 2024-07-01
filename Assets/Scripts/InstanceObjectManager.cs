using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceObjectManager : MonoBehaviour
{
    // �v���p�e�B
    // -------------------------------------------------------------------------------------------------------------------

    // �I�u�W�F�N�g�̐����p�^�[����ݒ肷��񋓌^�ϐ�
    public enum InstantiatePattrn
    {
        [InspectorName("�Q�[���I�u�W�F�N�g�Ƃ��ĕW������")]
        Standard = 0,
        [InspectorName("UI�Ƃ��ĕ\�����邽��Canvas��ɐ���")]
        UI
    }

    // ��������I�u�W�F�N�g�Ɛ����̎d����ݒ肷��\����
    [System.Serializable]
    public struct Representative
    {
        [Header("DisplayObj�ɁA�I�u�W�F�N�g�Ƃ��Đ����������v���n�u������")]
        public GameObject DisplayObj;           // ������
        public InstantiatePattrn Pattrn;        // �����p�^�[��

        [Header("���W�n�̐ݒ�(UI��Position�̂ݗL��)")]
        public Vector3 DisplayPosition;         // �������W
        public Vector3 DisplayRotation;         // �����p�x(���Z���g�p)
        public Vector3 DisplayScale;            // �����傫��
    };


    // �����I�u�W�F�N�g�̃C���X�^���X�����s����悤�ɍ\���̔z��ɂ���
    [SerializeField] public Representative[] InstanceManager;

    // Canvas�g�����X�t�H�[���p�ϐ�(�C���X�y�N�^�[���Canvas�����Ďg��)
    [SerializeField] private Transform CanvasTransform;

    // -------------------------------------------------------------------------------------------------------------------
    // ���\�b�h
    // -------------------------------------------------------------------------------------------------------------------

    // �I�u�W�F�N�g���C���X�^���X��(����)���鏈���i�������ɁA�e�I�u�W�F�N�g�� SetActive �� false �ɂ��邽�߁A�\���������Ƃ��́AObjectSetActive()���g���āAActive��Ԃ�True�ɂ���j
    public void InstanceObject()
    {
        for (int i = 0; i < InstanceManager.Length; i++)      // �I�u�W�F�N�g�����p�\���̗̂v�f�����擾���āA�v�f��������������
        {
            switch (InstanceManager[i].Pattrn)      // �ݒ肵���������@�ɉ������I�u�W�F�N�g����
            {
                case InstantiatePattrn.Standard:    // �W������

                    InstanceManager[i].DisplayObj = Instantiate(InstanceManager[i].DisplayObj, InstanceManager[i].DisplayPosition, Quaternion.Euler(InstanceManager[i].DisplayRotation));
                    InstanceManager[i].DisplayObj.transform.localScale = InstanceManager[i].DisplayScale;
                    
                    ObjectSetActive(InstanceManager[i].DisplayObj, false);                    

                    Debug.Log("Standard");

                    break;

                case InstantiatePattrn.UI:      // UI�Ƃ���Canvas��ɐ���

                    InstanceManager[i].DisplayObj = Instantiate(InstanceManager[i].DisplayObj, CanvasTransform, false);
                    InstanceManager[i].DisplayObj.transform.localPosition = InstanceManager[i].DisplayPosition;

                    ObjectSetActive(InstanceManager[i].DisplayObj, false);

                    Debug.Log("UI");

                    break;
            }

            Debug.Log("�I�u�W�F�N�g�𐶐�����");
        }
    }

    // �w�肵���I�u�W�F�N�g��Active��؂�ւ��鏈���i�����F�؂�ւ���I�u�W�F�N�g�Atrue��false���j
    public void ObjectSetActive(GameObject setObj, bool setActive)
    {
        if(setObj.activeSelf != setActive)
        {
            setObj.SetActive(setActive);
            Debug.Log("�w�肵���I�u�W�F�N�g�F" + setObj + "SetActive�F" + setActive);
        }
        else
        {
            Debug.Log("���̃I�u�W�F�N�g�́A���� " + setActive + " �ł�");
        }        
    }

    // �w�肵���I�u�W�F�N�g�̍��W��ύX���鏈��
    public void ObjectPos(GameObject setObj, Vector3 pos)
    {
        setObj.transform.position = pos;
    }

    // �C���X�^���X�������I�u�W�F�N�g�𑼃X�N���v�g��ŕϐ��Ƃ��Ď擾�����鏈��(�����F�擾�����������I�u�W�F�N�g�̗v�f�ԍ�)
    public GameObject GetInstansedObject(int InstanceNum)
    {
        return InstanceManager[InstanceNum].DisplayObj;
    }
}

// ����
// �C���X�y�N�^�[��Őݒ肵���I�u�W�F�N�g�̃C���X�^���X�������͊���
// ���ɁA�ʃX�N���v�g�ŁA�������������Ē��ԃ��U���g��\�����鏈�������
//
//
//
// 2024/01/19
// �X�V�FUI�I�u�W�F�N�g�̐����̍ہA�\��������W��ݒ�ł���悤�ɏC��
// �@�@�@UI�I�u�W�F�N�g���������L�q���Ȍ��ɂȂ�悤�X�V
//
// 2024/01/26
// �C���F�R�[�h�̃R�����g���ꕔ�ύX
// 



// UI���������o�b�N�A�b�v
/*
                    GameObject parent = GameObject.Find("Canvas");
                    if (parent != null)
                    {
                        Debug.Log("ObjectFind:Succese");
                        InstanceManager[i].DisplayObj = Instantiate(InstanceManager[i].DisplayObj, parent.transform.position, Quaternion.identity);
                        InstanceManager[i].DisplayObj.transform.SetParent(parent.transform, false);

                        ObjectSetActive(InstanceManager[i].DisplayObj, false);

                    }
                    else
                    {
                        Debug.Log("ObjectFind:Failed");
                    }

                    Debug.Log("UI");

                    break;

*/