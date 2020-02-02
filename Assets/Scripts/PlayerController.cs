using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameController gameController = null;
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
    [SerializeField] private int wrenchAmmo = 5;
    [SerializeField] private int reloadTime = 5;
    [SerializeField] private TMP_Text ammoText = null;

    [Space]
    [SerializeField] private difficultyLevel difficulty = difficultyLevel.Basic;
    [SerializeField] private ProgressBarCircle dockPB = null;
    [SerializeField] private GameObject eButton = null;
    public float minX, maxX = 0f;
    public AudioClip ohyeah, oh, bicycle, creepyLaugh, kharate, powerUp, negative = null;
    [SerializeField] private TMP_Text scoreText = null;

    private int score = 0;
    private float movex;
    private Coroutine dockingCoroutine, dTimeCoroutine = null;
    private bool canDock = true;
    private float dockingTime = 2f;
    private int currentAmmo = 0;

    private void Awake()
    {
        ammoText.text = wrenchAmmo.ToString();
        currentAmmo = wrenchAmmo;
    }

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

            if (Input.GetMouseButtonDown(0))
            {
                if (currentAmmo > 0)
                {
                    Transform fired = Instantiate(wrenchTransform, wrenchSpawnPos.position, this.transform.rotation);
                    fired.GetComponent<Rigidbody2D>().AddForce(Vector2.right * projectileSpeed, ForceMode2D.Impulse);
                    Destroy(fired.gameObject, 1.5f);
                    currentAmmo--;
                    ammoText.text = currentAmmo.ToString();
                }

                if (currentAmmo == 0)
                {
                    StartCoroutine(ReloadWrenchAmmo());
                }
            }
        }
        else
        {
            //animator.Play("PlayerIdle");
        }

        if (canDock == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // start docking game
                eButton.SetActive(false);
                repairMaster.StartRepairMode(difficulty, 6000);
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
                if (canDock)
                {
                    CameraShake.instance.Shake(0.05f, 0.5f);
                    AudioPlayer.Instance.PlayOneShot(negative);
                    onObstuctionHit.Invoke();
                }
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

    IEnumerator ReloadWrenchAmmo()
    {
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = wrenchAmmo;
        ammoText.text = currentAmmo.ToString();
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
        animator.Play("Repair 1");
        repairMaster.OnRepaired += OnRepairCompleted;
        repairMaster.OnRepaired += gameController.OnRepaired;
        AudioPlayer.Instance.PlayOneShot(bicycle);
    }

    private void OnRepairCompleted(bool status)
    {
        // true - Completed
        // false - failed
        canDock = true;
        if (status)
        {
            score++;
            scoreText.text = score.ToString();
        }
        AudioPlayer.Instance.PlayOneShot((status == true ? ohyeah : negative));
        print("Won : " + status);
        repairMaster.OnRepaired -= OnRepairCompleted;
        animator.Play("Idel");
    }


}
