using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private Mover[] backgroundMovers = null;
    [SerializeField] private ObstacleSpawner obstacleSpawner = null;

    private void Awake()
    {
        StartGame();
    }

    public void StartGame()
    {        
        for(int i =0;i<backgroundMovers.Length;i++)
            backgroundMovers[i].StartMoving();
        obstacleSpawner.StartSpawning();
        playerController.move = true;
    }
}
