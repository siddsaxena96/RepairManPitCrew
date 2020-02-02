using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private Mover[] backgroundMovers = null;
    [SerializeField] private ObstacleSpawner obstacleSpawner = null;
    [SerializeField] private Slider distanceSlider = null;
    [SerializeField] private int secondsTillCycle = 15;
    [SerializeField] private GameObject brokenCycle = null;
    [SerializeField] private Transform brokenCycleSpawnPoint = null;

    private int currentWait = 0;
    private bool pursuitOn = true;

    private void Awake()
    {
        StartGame();
    }

    public void StartGame()
    {
        for (int i = 0; i < backgroundMovers.Length; i++)
            backgroundMovers[i].StartMoving();
        obstacleSpawner.StartSpawning();
        playerController.move = true;
        currentWait = secondsTillCycle;
        StartCoroutine(ReachTillCycleCoroutine());
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
        Instantiate(brokenCycle,brokenCycleSpawnPoint.position,Quaternion.identity);
        yield return null;
    }
}
