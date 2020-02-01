using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairRBHelper : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (this.transform.localPosition.y < -200)
        {
            this.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;


        }
        if (this.transform.localPosition.y > 200)
        {
            this.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
       
    }
}
