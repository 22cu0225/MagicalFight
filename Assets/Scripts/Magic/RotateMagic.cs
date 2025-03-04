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
        // 魔術なら
        if (gameObject.tag == "Magic")
        {
            // 向きによって回転方向を変える
            Change = this.GetComponent<Magic>().GetOrientation() ? -1.0f:1.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotatoSpeed * Time.deltaTime * Change);
    }
}
