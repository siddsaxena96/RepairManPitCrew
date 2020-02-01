using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb = null;
    public float speed;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }
    
    // Use this for initialization
    void Start()
    {
        rb.velocity = transform.right * speed;
    }
}
