using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 7;
    public int rowCount = 5;
    [SerializeField] private Vector2 rowJumpDistance = Vector2.zero;
    public bool move = true;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private Rigidbody2D rb = null;
    [SerializeField] private Vector2 velocity = Vector2.zero;
    private int currentRow = 3;
    private float movex;

    public void Update()
    {
        if (move)
        {
            movex = Input.GetAxis("Horizontal");
            bool flipSprite = (spriteRenderer.flipX ? (movex > 0.01f) : (movex < 0.01f));
            if (flipSprite)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                AttemptRowJump(true);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                AttemptRowJump(false);
            }
        }
        else
        {
            //animator.Play("PlayerIdle");
        }
    }

    public void AttemptRowJump(bool up)
    {
        if (up && currentRow == 5)
            return;
        if (!up && currentRow == 1)
            return;
        rb.position += up ? rowJumpDistance : -rowJumpDistance;
        currentRow += up ? 1 : -1;
    }

    public void FixedUpdate()
    {
        rb.velocity = new Vector2(movex * maxSpeed, rb.velocity.y);
    }
}
