using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum difficultyLevel
{
    Basic,
    Moderate,
    Hard,
    RAGEEEE
}

public class RepairMaster : MonoBehaviour
{
    [Header("Game Pieces")]
    [SerializeField] private GameObject repairGameobject = null;
    [SerializeField] private Image backgroundImage = null;
    [SerializeField] private GameObject player = null;
    [SerializeField] private GameObject npc = null;

    [Header("Game Settings")]
    [SerializeField] private difficultyLevel difficulty = difficultyLevel.Basic;
    [Range(0.1f, 1f)]
    [SerializeField] private float scoringSensitivity = 0.5f;
    [SerializeField] private int WinValue = 100;

    [Header("Collision Controller")]
    [SerializeField] private RepairCollisionMaster colMaster = null;

    private Rigidbody2D playerRB, npcRB = null;
    private Coroutine playerCoroutine, npcCoroutine, timerCoroutine = null;
    private float progress = 0;
    private float npcMinRefreshTime, npcMaxRefreshTime = -1f;
    private int playerScoreMultiplier = -1;
    [SerializeField] private float npcPower = 10;
    private int repairStatus = -1;


    void Start()
    {
        if (repairGameobject.activeInHierarchy)
        {
            repairGameobject.SetActive(false);
        }
        if (player.transform.GetComponent<Rigidbody2D>())
        {
            playerRB = player.transform.GetComponent<Rigidbody2D>();
        }
        if (npc.transform.GetComponent<Rigidbody2D>())
        {
            npcRB = npc.transform.GetComponent<Rigidbody2D>();
        }

        if (colMaster != null)
        {
            colMaster.InProgress += RepairProgress;
            StartRepairMode(difficultyLevel.Basic);
        }


    }

    void OnDestroy()
    {
        colMaster.InProgress -= RepairProgress;
    }

    private void RepairProgress(bool status)
    {
        if (status)
        {
            repairStatus = 1;
        }
        else
        {
            repairStatus = -1;
        }
    }

    IEnumerator TimerCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(scoringSensitivity);
        while (true)
        {
            progress += (float)playerScoreMultiplier * repairStatus;
            Color alphaChanel = new Color();
            alphaChanel = (repairStatus == 1 ? Color.green : Color.red);
            alphaChanel.a = (repairStatus == 1 ? ((progress + 150f) / 255) : (0.8f - ((progress + 150f) / 255)));
            backgroundImage.color = alphaChanel;
            yield return wait;
        }
    }

    void Update()
    {

    }

    public void StartRepairMode(difficultyLevel diff)
    {
        repairGameobject.SetActive(true);
        CheckAndClearCoroutine();
        playerCoroutine = StartCoroutine(PlayerCoroutine(diff));
        npcCoroutine = StartCoroutine(NPCCoroutine(diff));
    }

    private IEnumerator PlayerCoroutine(difficultyLevel diff)
    {
        if (timerCoroutine == null)
        {
            timerCoroutine = StartCoroutine(TimerCoroutine());
        }
        yield return null;
    }

    private IEnumerator NPCCoroutine(difficultyLevel diff)
    {
        SetDifficulty(diff);
        while (progress != WinValue)
        {
            npcRB.gravityScale = 10;
            yield return new WaitForSeconds(UnityEngine.Random.Range(npcMinRefreshTime, npcMaxRefreshTime));
            npcRB.gravityScale = 1;
            npcRB.AddForce(new Vector2(0, 1) * npcPower, ForceMode2D.Impulse);
        }
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

    }

    private void SetDifficulty(difficultyLevel diff)
    {
        switch (diff)
        {
            case difficultyLevel.Basic:
                npcMinRefreshTime = 3;
                npcMaxRefreshTime = 4;
                playerScoreMultiplier = 10;
                break;
            case difficultyLevel.Moderate:
                npcMinRefreshTime = 2;
                npcMaxRefreshTime = 3;
                playerScoreMultiplier = 7;
                break;
            case difficultyLevel.Hard:
                npcMinRefreshTime = 1;
                npcMaxRefreshTime = 2;
                playerScoreMultiplier = 4;
                break;
            case difficultyLevel.RAGEEEE:
                npcMinRefreshTime = 0;
                npcMaxRefreshTime = 1;
                playerScoreMultiplier = 1;
                break;
        }
    }

    private void CheckAndClearCoroutine()
    {
        if (playerCoroutine != null)
            playerCoroutine = null;
        if (npcCoroutine != null)
            npcCoroutine = null;
        progress = -1;
    }
}
