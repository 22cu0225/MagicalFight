using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    //Move
    //------------------------------------------------
    [SerializeField] float moveSpeed = 3.0f;                   // 移動速度
    private float moveSpeedResult;                          // 移動速度計算結果
    [SerializeField] float fallSpeed　= 2.5f;                       // 落下速度
    //------------------------------------------------

    //Size
    //-----------------------------------
    private CapsuleCollider2D playerCollder;
    private float MaxYSize;
    //-----------------------------------

    // すり抜ける床
    private Scaffold_p scaffold;

    // Animation
    private Animator playerAnimator;

    // Rigidbody
    private Rigidbody2D playerRb;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeedResult = moveSpeed;
        playerAnimator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
        scaffold = GetComponent<Scaffold_p>();
    }

    public void Move(Vector2 moveDirection,bool fly)
    {
        if (moveDirection.x > 0.2 || moveDirection.x < -0.2)
        {
            // ダッシュ
            if (moveDirection.x > 0.9 || moveDirection.x < -0.9)
            {
                moveSpeedResult = moveSpeed * 1.5f;
            }
            else
            {
                moveSpeedResult = moveSpeed;
            }

            // X移動
            transform.Translate(moveDirection.x * moveSpeedResult * Time.deltaTime, 0.0f, 0.0f);
            // アニメーション歩き　開始
            playerAnimator.SetBool("walk", true);
        }
        else
        {
            // アニメーション歩き　停止
            playerAnimator.SetBool("walk", false);
        }
        // crouch
        if (moveDirection.y < -0.1)
        {
            // ジャンプ時
            if (fly)
            {
                //　落下速度上昇
                playerRb.velocity = Vector2.down * fallSpeed;
            }
            // 地上
            else
            {
                playerAnimator.SetFloat("speed", 2f);
                // しゃがみアニメーション 開始
                playerAnimator.SetBool("crouch", true);

                // 最大半分の高さまで
                if (MaxYSize / 2 <= playerCollder.size.y)
                {
                    // コリジョンのYを小さくする
                    float ColliderY = playerCollder.size.y;
                    ColliderY -= 0.1f;
                    playerCollder.size = new Vector2(playerCollder.size.x, ColliderY);
                }

                // 床抜ける
                scaffold.SetThrough(true);

                Debug.Log("しゃがんでるよ");
            }
        }
        else
        {
            if (playerAnimator.GetBool("crouch"))
            {
                // しゃがみアニメーション　逆再生
                playerAnimator.SetFloat("speed", -3f);
                // 最大サイズより小さかったら
                if (playerCollder.size.y < MaxYSize)
                {
                    // コリジョンのYを大きくする
                    float ColliderY = playerCollder.size.y;
                    ColliderY += 0.1f;
                    playerCollder.size = new Vector2(playerCollder.size.x, ColliderY);
                }
                else
                {
                    // しゃがみアニメーション 停止
                    playerAnimator.SetBool("crouch", false);
                }
            }
        }
    }
}
