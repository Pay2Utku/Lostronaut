using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> SpawnPoints;
    public GameObject EnemyPrefab;
    public bool GameInProgress = false;
    public float EnemySpawnRate = 3.0f;
    public float SpawnPointCooldown = 1.0f;

    private float _nextSpawnTime = 0f;
    private Dictionary<Transform, float> _occupiedSpawnPoints = new Dictionary<Transform, float>();

    void Update()
    {
        if (GameInProgress)
        {
            HandleCooldowns();
            if (Time.time >= _nextSpawnTime)
            {
                SpawnEnemy();
                _nextSpawnTime = Time.time + EnemySpawnRate;
            }
        }
    }

    void HandleCooldowns()
    {
        List<Transform> keys = new List<Transform>(_occupiedSpawnPoints.Keys);
        foreach (var spawnPoint in keys)
        {
            if (Time.time >= _occupiedSpawnPoints[spawnPoint])
            {
                _occupiedSpawnPoints.Remove(spawnPoint);
            }
        }
    }

    void SpawnEnemy()
    {
        if (SpawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points available");
            return;
        }

        List<Transform> availableSpawnPoints = SpawnPoints.FindAll(spawnPoint => !_occupiedSpawnPoints.ContainsKey(spawnPoint));

        if (availableSpawnPoints.Count == 0)
        {
            Debug.LogWarning("No available spawn points");
            return;
        }

        Transform selectedSpawnPoint = availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count)];
        Instantiate(EnemyPrefab, selectedSpawnPoint.position, selectedSpawnPoint.rotation);
        _occupiedSpawnPoints[selectedSpawnPoint] = Time.time + SpawnPointCooldown;
    }
}
