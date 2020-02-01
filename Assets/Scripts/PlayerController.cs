using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [Space]
    public int rowCount = 5;
    [SerializeField] private Vector2 rowJumpDistance = Vector2.zero;
    [Space]
    public float maxSpeed = 7;
    [SerializeField] private Vector2 velocity = Vector2.zero;
    public bool move = true;
    public float minX, maxX = 0f;
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

    public void FixedUpdate()
    {
        rb.velocity = new Vector2(movex * maxSpeed, rb.velocity.y);
        rb.position = new Vector2(Mathf.Clamp(rb.position.x, minX, maxX), rb.position.y);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Obstacle":
                CameraShake.instance.Shake(0.1f, 0.5f);
                break;
            case "RampUp":
                CameraShake.instance.Shake(0.1f, 0.5f);
                AttemptRowJump(true);
                break;
            case "RampDown":
                CameraShake.instance.Shake(0.1f, 0.5f);
                AttemptRowJump(false);
                break;
        }        
    }
}
