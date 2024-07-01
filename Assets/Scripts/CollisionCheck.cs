using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    // �ϐ��錾
    private string ObjTag;      // �ڐG�����I�u�W�F�N�g�ɂ��Ă���^�O��������ϐ�

    // �ڐG��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        {
            // �ڐG�����I�u�W�F�N�g�̃^�O���`�F�b�N
            ObjTag = collision.gameObject.tag;

            Debug.Log("ObjTag:" + ObjTag);
        }
    }

    // ���ꂽ��
    private void OnCollisionExit2D(Collision2D collision)
    {
        // �擾�����^�O����j��
        ObjTag = null;

        Debug.Log("ObjTag:Discard");
    }

    // �ڐG���̃I�u�W�F�N�g�̃^�O����String�^�ŕԂ��֐�
    public string GetCollisionObjectTag()
    {
        if(ObjTag != null)
        {
            return ObjTag;
        }
        else
        {
            return null;
        }
    }



}
