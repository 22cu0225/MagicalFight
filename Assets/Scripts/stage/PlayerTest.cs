using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//デバッグ用の仮のプレイヤースクリプト
//
public class PlayerTest : MonoBehaviour
{
    public float moveSpeed;

    public int Is_junp;

    // Start is called before the first frame update
    void Start()
    {
        Is_junp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(moveSpeed, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-moveSpeed, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(0.0f, moveSpeed * 1.2f, 0.0f);
            Is_junp = 1;
        }
        else
        {
            Is_junp = 0;
        }
    }
}
