using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShot : MonoBehaviour
{
    // ïœêîêÈåæ
    public int Power;
    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Speed, 0.0f,0.0f);

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("C" + collision.gameObject.name);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ActiveArea")
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("T" + collision.gameObject.name) ;
        if(collision.gameObject.tag == "MAGIC")
        {
            // if(Power <= 
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("T" + collision.gameObject.name);
        if (collision.gameObject.tag == "ActiveArea")
        {
            Destroy(this.gameObject);
        }
    }
}
