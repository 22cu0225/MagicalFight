using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMagic : MonoBehaviour
{
    [SerializeField] float rotatoSpeed = 10;
    private float Change;
    // Start is called before the first frame update
    void Start()
    {
        Change = 1.0f;
        // ���p�Ȃ�
        if (gameObject.tag == "Magic")
        {
            // �����ɂ���ĉ�]������ς���
            Change = this.GetComponent<Magic>().GetOrientation() ? -1.0f:1.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotatoSpeed * Time.deltaTime * Change);
    }
}
