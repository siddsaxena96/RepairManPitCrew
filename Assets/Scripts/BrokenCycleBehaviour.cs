using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenCycleBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb = null;
    [SerializeField] private Vector2 stopPoint = Vector2.zero;
    public float speed;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    private void Update()
    {
        if (rb.position.x <= 0)
            rb.velocity = Vector2.zero;
    }

    public void RushAhead()
    {
        rb.velocity = transform.right * Mathf.Abs(speed) * 2;
    }
}
