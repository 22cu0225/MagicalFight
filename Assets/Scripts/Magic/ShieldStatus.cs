using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldStatus : MonoBehaviour
{
    [SerializeField] int endurance = 3;             // �ϋv�l
    [SerializeField] float coolTime = 1.0f;         // �N�[���^�C��
    [SerializeField] string magicTag = "Magic";     // ���p�̃^�O��

    public float GetCoolTime() { return coolTime; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(magicTag))
        {
            Destroy(collision);
            endurance--;
            if(endurance <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
