using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] obstacleSpawnPositions = null;
    [SerializeField] private GameObject[] obstaclePrefabs = null;
    [SerializeField] private bool spawnOn = true;

    private void Awake()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        spawnOn = true;
        StartCoroutine(ObstacleSpawnerCoroutine());
    }

    IEnumerator ObstacleSpawnerCoroutine()
    {
        while (spawnOn)
        {
            Transform spawnPoint = obstacleSpawnPositions[Random.Range(0, obstacleSpawnPositions.Length)];
            GameObject temp = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(1.5f);
        }
        yield return null;
    }
}
