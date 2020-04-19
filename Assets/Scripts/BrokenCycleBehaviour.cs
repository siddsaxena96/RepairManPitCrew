using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenCycleBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb = null;
    [SerializeField] private Vector2 stopPoint = Vector2.zero;
    [SerializeField] private BoxCollider2D boxCollider = null;
    [SerializeField] private Animator[] animators = null;
    public float speed;
    private bool reached = false;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    private void Update()
    {
        if (rb.position.x <= 0 && !reached)
        {
            rb.velocity = Vector2.zero;
            reached = true;
        }
    }

    public void RushAhead()
    {
        rb.velocity = transform.right * Mathf.Abs(speed) * 2;
    }

    public void FallDown()
    {
        boxCollider.enabled = false;
        transform.localRotation = Quaternion.Euler(50, 0, 0);
        for (int i = 0; i < animators.Length; i++)
            animators[i].Play("Idle");
        rb.velocity = transform.right * speed;
    }
}
