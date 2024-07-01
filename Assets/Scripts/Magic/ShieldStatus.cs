using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldStatus : MonoBehaviour
{
    [SerializeField] int endurance = 3;             // 耐久値
    [SerializeField] float coolTime = 1.0f;         // クールタイム
    [SerializeField] string magicTag = "Magic";     // 魔術のタグ名

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
