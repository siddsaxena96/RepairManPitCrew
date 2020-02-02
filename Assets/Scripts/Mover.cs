using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb = null;
    public float speed;
    public bool moveOnAwake = true;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (moveOnAwake)
            rb.velocity = transform.right * speed;
    }

    // Use this for initialization
    public void StartMoving()
    {
        rb.velocity = transform.right * speed;
    }

    public void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }
}
