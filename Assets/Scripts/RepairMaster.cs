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
    [SerializeField] private ProgressBar progressBar = null;

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
    public event Action<bool> OnRepaired = null;
    private bool completed = false;

    void Awake()
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
            // StartRepairMode(difficultyLevel.Basic, 10);
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
            progress += (float)playerScoreMultiplier * (float)(repairStatus == -1 ? 0.1 : 1);
            progress = ClampedFloat(progress, 0, 100);
            progressBar.BarValue = progress;
            //Color alphaChanel = new Color();
            // alphaChanel = (repairStatus == 1 ? Color.green : Color.red);
            // alphaChanel.a = (repairStatus == 1 ? ((progress + 150f) / 255) : (0.8f - ((progress + 150f) / 255)));
            // backgroundImage.color = alphaChanel;
            yield return wait;
        }
    }

    private float ClampedFloat(float val, float min, float max)
    {
        if (val < min)
        {
            val = min;
        }
        if (val > max)
        {
            val = max;
        }
        return val;
    }

    void Update()
    {

    }

    public void StartRepairMode(difficultyLevel diff, float timer)
    {
        completed = false;
        CheckAndClearCoroutine();
        repairGameobject.SetActive(true);
        StartCoroutine(RepairExpireTimer(diff, timer));
    }

    private IEnumerator RepairExpireTimer(difficultyLevel diff, float timer)
    {
        if (playerCoroutine == null)
            playerCoroutine = StartCoroutine(PlayerCoroutine(diff));
        if (npcCoroutine == null)
            npcCoroutine = StartCoroutine(NPCCoroutine(diff));
        yield return new WaitForSeconds(timer);

        if (playerCoroutine != null)
            StopCoroutine(playerCoroutine);
        if (npcCoroutine != null)
            StopCoroutine(npcCoroutine);
        if (completed == false)
        {
            OnRepaired?.Invoke(false);
        }
        repairGameobject.SetActive(false);
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
            float rate = UnityEngine.Random.Range(npcMinRefreshTime, npcMaxRefreshTime) / 2;
            npcRB.gravityScale = 0;
            yield return new WaitForSeconds(rate);
            npcRB.gravityScale = 1;
            yield return new WaitForSeconds(rate);
            npcRB.gravityScale = 5;
            npcRB.AddForce(new Vector2(0, 1) * npcPower, ForceMode2D.Impulse);
        }
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
        completed = true;
        OnRepaired?.Invoke(true);
        repairGameobject.SetActive(false);

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
        if (progressBar != null)
            progressBar.BarValue = 1;
    }
}
