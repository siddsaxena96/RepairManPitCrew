using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private RepairMaster repairMaster = null;
    [SerializeField] private Mover[] backgroundMovers = null;
    [SerializeField] private ObstacleSpawner obstacleSpawner = null;
    [SerializeField] private Slider distanceSlider = null;
    [SerializeField] private int secondsTillCycle = 15;
    [SerializeField] private GameObject brokenCycle = null;
    [SerializeField] private Transform brokenCycleSpawnPoint = null;
    [SerializeField] private GameObject[] healthHearts = null;

    [Header("Weather Particle Systems")]
    [SerializeField] private ParticleSystem snowParticleSystem = null;
    [SerializeField] private ParticleSystem rainParticleSystem = null;

    [Header("Game Narrative")]
    [SerializeField] private int gameDuration = 180;
    [SerializeField] private TMP_Text timerText = null;
    [SerializeField] private GameObject narrativePanel = null;
    [SerializeField] private string[] introSequence = null;
    [SerializeField] private string gameOver = null;
    [SerializeField] private string gameWon = null;
    [SerializeField] private TMP_Text mainText = null;
    [SerializeField] private Button nextButton = null;
    [SerializeField] private Button closeButton = null;

    private int currentWait = 0;
    private bool pursuitOn = true;
    private int currentHealth = 0;
    private int narrativeCount = 0;
    private BrokenCycleBehaviour brokenCycleBehaviour = null;

    public void StartGame()
    {
        currentHealth = healthHearts.Length;
        for (int i = 0; i < backgroundMovers.Length; i++)
            backgroundMovers[i].StartMoving();
        obstacleSpawner.StartSpawning();
        playerController.move = true;
        currentWait = secondsTillCycle;
        StartCoroutine(ReachTillCycleCoroutine());
        StartCoroutine(StartGameTimer());
    }

    private IEnumerator StartGameTimer()
    {
        int time = gameDuration;
        while (time > 0)
        {
            timerText.text = (int)time / 60 + " : " + (int)time % 60;
            yield return new WaitForSeconds(1);
            time--;
        }
        EndGame(true);
    }

    IEnumerator ReachTillCycleCoroutine()
    {
        distanceSlider.maxValue = currentWait;
        distanceSlider.value = 1;
        while (pursuitOn)
        {
            yield return new WaitForSeconds(1);
            distanceSlider.value++;

            if (distanceSlider.value >= currentWait)
            {
                pursuitOn = false;
            }
        }
        Debug.Log("Caught up to cycle");
        obstacleSpawner.BlockRow(brokenCycleSpawnPoint);
        brokenCycleBehaviour = Instantiate(brokenCycle, brokenCycleSpawnPoint.position, Quaternion.identity).GetComponent<BrokenCycleBehaviour>();
        yield return null;
    }

    public void ReduceHealth()
    {
        currentHealth--;
        if (currentHealth >= 0)
            healthHearts[currentHealth].SetActive(false);
        if (currentHealth == 0)
        {            
            for (int i = 0; i < backgroundMovers.Length; i++)
                backgroundMovers[i].StopMoving();            
            playerController.move = false;
            EndGame(false);
        }
    }

    public void OnRepaired(bool obj)
    {
        if (obj && brokenCycleBehaviour != null)
        {
            brokenCycleBehaviour.RushAhead();
        }
        else if (!obj && brokenCycleBehaviour != null)
        {
            brokenCycleBehaviour.FallDown();
        }
        distanceSlider.value = 1;
        brokenCycleBehaviour = null;
        pursuitOn = true;
        repairMaster.OnRepaired -= OnRepaired;
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            snowParticleSystem.Stop();
            rainParticleSystem.Play();
        }
        else
        {
            rainParticleSystem.Stop();
            snowParticleSystem.Play();
        }
        StartCoroutine(ReachTillCycleCoroutine());
    }

    public void OnNextClicked()
    {
        if (narrativeCount == introSequence.Length - 1)
        {
            OnIntroSequenceOver();
            return;
        }
        narrativeCount++;
        mainText.text = introSequence[narrativeCount];
    }

    public void OnIntroSequenceOver()
    {
        nextButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(true);
        narrativePanel.SetActive(false);
        StartGame();
    }

    public void EndGame(bool status)
    {
        mainText.text = status ? gameWon : gameOver;
        narrativePanel.SetActive(true);
    }
}
