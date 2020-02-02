using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb = null;
    [SerializeField] private Animator animator = null;
    [Space]

    public int rowCount = 5;
    [SerializeField] private int currentRow = 3;
    [SerializeField] private Vector2 rowJumpDistance = Vector2.zero;
    [SerializeField] private Vector3 scaleChange = Vector2.zero;
    public UnityEvent onObstuctionHit = new UnityEvent();
    
    [Space]

    public float maxSpeed = 7;
    [SerializeField] private Vector2 velocity = Vector2.zero;
    public bool move = true;
    [Space]

    public bool fireEnable = true;
    [SerializeField] private Transform wrenchTransform = null;
    [SerializeField] private Transform wrenchSpawnPos = null;
    [SerializeField] private int projectileSpeed = 10;
    [SerializeField] private RepairMaster repairMaster = null;

    [Space]
    [SerializeField] private difficultyLevel difficulty = difficultyLevel.Basic;
    [SerializeField] private ProgressBarCircle dockPB = null;
    [SerializeField] private GameObject eButton = null;
    public float minX, maxX = 0f;

    public AudioClip ohyeah, oh, bicycle, creepyLaugh, kharate, powerUp, negative = null;
    private float movex;
    private Coroutine dockingCoroutine, dTimeCoroutine = null;
    private bool canDock = true;
    private float dockingTime = 2f;


    public void Update()
    {
        if (move && canDock)
        {
            movex = Input.GetAxis("Horizontal");
            //bool flipSprite = (spriteRenderer.flipX ? (movex > 0.05f) : (movex < 0.05f));

            // if (flipSprite)
            // {
            //     spriteRenderer.flipX = !spriteRenderer.flipX;
            // }

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

        if (fireEnable)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Transform fired = Instantiate(wrenchTransform, wrenchSpawnPos.position, this.transform.rotation);
                fired.GetComponent<Rigidbody2D>().AddForce(Vector2.right * projectileSpeed, ForceMode2D.Impulse);
                Destroy(fired.gameObject, 1.5f);
            }
        }

        if (canDock == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // start docking game
                eButton.SetActive(false);
                repairMaster.StartRepairMode(difficulty, 10);
            }
        }
    }

    public void FixedUpdate()
    {
        rb.velocity = new Vector2(movex * maxSpeed, rb.velocity.y);
        rb.position = new Vector2(Mathf.Clamp(rb.position.x, minX, maxX), rb.position.y);
    }

    public void AttemptRowJump(bool up)
    {
        if (up && currentRow == rowCount)
            return;
        if (!up && currentRow == 1)
            return;
        rb.position += up ? rowJumpDistance : -rowJumpDistance;
        transform.localScale += up ? -scaleChange : scaleChange;
        currentRow += up ? 1 : -1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Obstacle":
                CameraShake.instance.Shake(0.05f, 0.5f);               
                AudioPlayer.Instance.PlayOneShot(negative);
                onObstuctionHit.Invoke();
                //transform.position = new Vector2(-8, transform.position.y);
                break;
            case "CYCLIST":
                if (dockingCoroutine == null)
                    dockingCoroutine = StartCoroutine(DockingCoroutine());
                AudioPlayer.Instance.PlayOneShot(ohyeah);

                break;
            case "POWERUP":
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        switch (col.tag)
        {
            case "CYCLIST":
                if (dockingCoroutine != null)
                {
                    canDock = true;
                    if (dTimeCoroutine != null)
                        StopCoroutine(dTimeCoroutine);
                    StopCoroutine(dockingCoroutine);
                    dockingCoroutine = null;
                    dockPB.gameObject.SetActive(false);
                    eButton.SetActive(false);
                }
                break;
        }
    }

    IEnumerator DockingCoroutine()
    {
        if (dTimeCoroutine == null)
            StartCoroutine(dockLoader(dockingTime));
        yield return new WaitForSeconds(dockingTime);
        //this.transform.GetComponent<BoxCollider2D>().isTrigger = true;  // switch off when docking completed!
        canDock = false;
        REPAIR();
    }

    IEnumerator dockLoader(float dockingTime)
    {
        dockPB.gameObject.SetActive(true);
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            dockPB.BarValue = normalizedTime * 100;
            normalizedTime += Time.deltaTime / dockingTime;
            yield return null;
        }
        dockPB.gameObject.SetActive(false);
        eButton.SetActive(true);
    }

    void REPAIR()
    {
        repairMaster.OnRepaired += OnRepairCompleted;
        AudioPlayer.Instance.PlayOneShot(bicycle);
    }

    private void OnRepairCompleted(bool status)
    {
        // true - Completed
        // false - failed
        print("Won : " + status);
    }
}
