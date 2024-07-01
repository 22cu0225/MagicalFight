using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{

    [SerializeField] float rotationSpeed;
    float width;
    void Start()
    {
        width = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        //transform.Translate(-scrollScroll * Time.deltaTime, 0.0f, 0.0f);
        transform.Rotate(0.0f, 0.0f, rotationSpeed * Time.deltaTime);
    }
}
