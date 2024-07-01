using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerJump : MonoBehaviour
{
    //Jump
    //-----------------------------------
    [SerializeField] float jumpPower = 5.0f;   // ジャンプ力
    private float jumpPowerResult;          // ジャンプ力計算結果
    private bool jumpping;                  // ジャンプしているか
    private uint jumpCount;                 // ジャンプ回数
    private float jumpCoolTime;             // 次のジャンプまでのクールタイム
    private bool coolTimeStart;             // クールタイムが始まるか
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
                //Debug.Log("じゃんぷするよ");
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
