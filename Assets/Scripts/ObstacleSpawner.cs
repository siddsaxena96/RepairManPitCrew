using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private bool spawnOn = true;
    [Header("Road Obstacles")]
    [SerializeField] private List<Transform> obstacleSpawnPositions = null;
    [SerializeField] private GameObject[] obstaclePrefabs = null;

    [Header("Nange Insaan")]
    [SerializeField] private Transform[] nangeSpawnPos = null;
    [SerializeField] private Transform[] nangeInsaan = null;
    [SerializeField] private float nangeSpawnFrequency = 1f;

    [Header("Nange Insaan Path Settings")]
    public float MoveSpeed = 5.0f;
    public float frequency = 20.0f;
    public float magnitude = 10f;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip creepyLaugh, death = null;

    private Vector3 axis;
    private Coroutine nangeSpawn = null;
    private Transform blockedRow = null;


    IEnumerator NangeSpawn()
    {
        WaitForSeconds wait = new WaitForSeconds(nangeSpawnFrequency);
        while (spawnOn)
        {
            yield return wait;
            Transform tPosition = nangeSpawnPos[Random.Range(0, nangeSpawnPos.Length)];
            Transform nanga = Instantiate(nangeInsaan[Random.Range(0, nangeInsaan.Length)], tPosition.position, Quaternion.identity, tPosition);
            AudioPlayer.Instance.PlayOneShot(creepyLaugh);
            StartCoroutine(MoveInSineWave(nanga));
        }
    }

    IEnumerator MoveInSineWave(Transform nanga)
    {
        Vector3 pos = nanga.position;
        while (true)
        {
            if (nanga != null && nanga.gameObject.activeInHierarchy)
            {
                pos += transform.right * Time.deltaTime * MoveSpeed;
                nanga.position = pos + axis * Mathf.Sin(Time.time * frequency) * magnitude;
            }
            yield return null;
        }
    }

    public void StartSpawning()
    {
        spawnOn = true;
        StartCoroutine(ObstacleSpawnerCoroutine());
        if (nangeSpawn == null)
        {
            nangeSpawn = StartCoroutine(NangeSpawn());
        }
        axis = Vector3.up;
    }

    IEnumerator ObstacleSpawnerCoroutine()
    {
        while (spawnOn)
        {
            Transform spawnPoint = obstacleSpawnPositions[Random.Range(0, obstacleSpawnPositions.Count)];
            GameObject temp = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(3f);
        }
        yield return null;
    }

    public void BlockRow(Transform rowToBlock)
    {
        blockedRow = rowToBlock;
        if (obstacleSpawnPositions.Contains(rowToBlock))
            obstacleSpawnPositions.Remove(rowToBlock);
    }

    public void UnblockRow()
    {
        if (blockedRow != null)
        {
            obstacleSpawnPositions.Add(blockedRow);
        }
    }
}
