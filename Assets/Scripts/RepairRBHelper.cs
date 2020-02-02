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
        if (this.transform.localPosition.y < -141)
        {
            this.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            this.transform.localPosition = new Vector3(0, -140, 0);


        }
        if (this.transform.localPosition.y > 141)
        {
            this.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            this.transform.localPosition = new Vector3(0, 140, 0);
        }

    }
}
