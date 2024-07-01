using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerJump : MonoBehaviour
{
    //Jump
    //-----------------------------------
    [SerializeField] float jumpPower = 5.0f;   // �W�����v��
    private float jumpPowerResult;          // �W�����v�͌v�Z����
    private bool jumpping;                  // �W�����v���Ă��邩
    private uint jumpCount;                 // �W�����v��
    private float jumpCoolTime;             // ���̃W�����v�܂ł̃N�[���^�C��
    private bool coolTimeStart;             // �N�[���^�C�����n�܂邩
    //-----------------------------------

    //--------------------------------------------
    [Header("TagName")]
    [SerializeField] string Floor = "Floor";
    [SerializeField] string Scaffold = "Scaffold";
    //[SerializeField] string someone = "";
    //--------------------------------------------

    // Rigidbody
    private Rigidbody2D playerRb;

    // Start is called before the first frame update
    void Start()
    {
        jumpping = false;
        jumpPowerResult = jumpPower;
        jumpCoolTime = 0.0f;
        coolTimeStart = false;

        playerRb = GetComponent<Rigidbody2D>();
    }

    public void Jump()
    {
        if (!jumpping)
        {
            if (jumpCoolTime <= 0.0f)
            {
                playerRb.velocity = Vector2.up * jumpPowerResult;
                jumpCount++;
                if (jumpCount > 1)
                {
                    jumpping = true;
                }
                jumpCoolTime = 0.3f;
                coolTimeStart = true;
                //Debug.Log("�����Ղ����");
            }
        }
        if (coolTimeStart)
        {
            jumpCoolTime -= Time.deltaTime;
            if (jumpCoolTime < 0.0f)
            {
                coolTimeStart = false;
                jumpCoolTime = 0.0f;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Floor) || collision.gameObject.CompareTag(Scaffold))
        {
            jumpping = false;
            jumpCount = 0;
            jumpPowerResult = jumpPower;
        }
        //Debug.Log(collision.gameObject.tag);
    }
}
