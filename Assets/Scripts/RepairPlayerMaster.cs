using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairPlayerMaster : MonoBehaviour
{
    [Header("Player Config")]
    [SerializeField] private int power = 10;
    private Rigidbody2D playerRB = null;

    // Start is called before the first frame update
    void Start()
    {
        if(this.transform.GetComponent<Rigidbody2D>())
        {
            playerRB = this.transform.GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerRB.gravityScale = 1;
            playerRB.mass = 1;
            playerRB.AddForce(new Vector2(0, 1) * power, ForceMode2D.Impulse);
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            playerRB.mass = 10;
            playerRB.gravityScale = 10;
        }
        
    }
}
