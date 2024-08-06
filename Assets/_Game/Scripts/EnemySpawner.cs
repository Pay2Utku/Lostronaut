using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public GameObject enemyPrefab;
    public bool gameInProgress = false;
    public float enemySpawnRate = 3.0f;
    public float spawnPointCooldown = 1.0f;

    private float nextSpawnTime = 0f;
    private Dictionary<Transform, float> occupiedSpawnPoints = new Dictionary<Transform, float>();

    void Update()
    {
        if (gameInProgress)
        {
            HandleCooldowns();
            if (Time.time >= nextSpawnTime)
            {
                SpawnEnemy();
                nextSpawnTime = Time.time + enemySpawnRate;
            }
        }
    }

    void HandleCooldowns()
    {
        List<Transform> keys = new List<Transform>(occupiedSpawnPoints.Keys);
        foreach (var spawnPoint in keys)
        {
            if (Time.time >= occupiedSpawnPoints[spawnPoint])
            {
                occupiedSpawnPoints.Remove(spawnPoint);
            }
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points available");
            return;
        }

        List<Transform> availableSpawnPoints = spawnPoints.FindAll(spawnPoint => !occupiedSpawnPoints.ContainsKey(spawnPoint));

        if (availableSpawnPoints.Count == 0)
        {
            Debug.LogWarning("No available spawn points");
            return;
        }

        Transform selectedSpawnPoint = availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count)];
        Instantiate(enemyPrefab, selectedSpawnPoint.position, selectedSpawnPoint.rotation);
        occupiedSpawnPoints[selectedSpawnPoint] = Time.time + spawnPointCooldown;
    }
}
