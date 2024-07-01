using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    //Move
    //------------------------------------------------
    [SerializeField] float moveSpeed = 3.0f;                   // �ړ����x
    private float moveSpeedResult;                          // �ړ����x�v�Z����
    [SerializeField] float fallSpeed�@= 2.5f;                       // �������x
    //------------------------------------------------

    //Size
    //-----------------------------------
    private CapsuleCollider2D playerCollder;
    private float MaxYSize;
    //-----------------------------------

    // ���蔲���鏰
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
            // �_�b�V��
            if (moveDirection.x > 0.9 || moveDirection.x < -0.9)
            {
                moveSpeedResult = moveSpeed * 1.5f;
            }
            else
            {
                moveSpeedResult = moveSpeed;
            }

            // X�ړ�
            transform.Translate(moveDirection.x * moveSpeedResult * Time.deltaTime, 0.0f, 0.0f);
            // �A�j���[�V���������@�J�n
            playerAnimator.SetBool("walk", true);
        }
        else
        {
            // �A�j���[�V���������@��~
            playerAnimator.SetBool("walk", false);
        }
        // crouch
        if (moveDirection.y < -0.1)
        {
            // �W�����v��
            if (fly)
            {
                //�@�������x�㏸
                playerRb.velocity = Vector2.down * fallSpeed;
            }
            // �n��
            else
            {
                playerAnimator.SetFloat("speed", 2f);
                // ���Ⴊ�݃A�j���[�V���� �J�n
                playerAnimator.SetBool("crouch", true);

                // �ő唼���̍����܂�
                if (MaxYSize / 2 <= playerCollder.size.y)
                {
                    // �R���W������Y������������
                    float ColliderY = playerCollder.size.y;
                    ColliderY -= 0.1f;
                    playerCollder.size = new Vector2(playerCollder.size.x, ColliderY);
                }

                // ��������
                scaffold.SetThrough(true);

                Debug.Log("���Ⴊ��ł��");
            }
        }
        else
        {
            if (playerAnimator.GetBool("crouch"))
            {
                // ���Ⴊ�݃A�j���[�V�����@�t�Đ�
                playerAnimator.SetFloat("speed", -3f);
                // �ő�T�C�Y��菬����������
                if (playerCollder.size.y < MaxYSize)
                {
                    // �R���W������Y��傫������
                    float ColliderY = playerCollder.size.y;
                    ColliderY += 0.1f;
                    playerCollder.size = new Vector2(playerCollder.size.x, ColliderY);
                }
                else
                {
                    // ���Ⴊ�݃A�j���[�V���� ��~
                    playerAnimator.SetBool("crouch", false);
                }
            }
        }
    }
}
