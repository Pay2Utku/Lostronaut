using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> SpawnPoints;
    [SerializeField] private Transform _poolParent;
    public GameObject[] EnemyPrefabs;
    [SerializeField] private float _enemySpawnRate = 3.0f;
    [SerializeField] private int _enemySpawnGroupSize = 3;
    [SerializeField] private List<GameObject> _enemyPool;

    private float _timer = 0f;


    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _enemySpawnRate)
        {
            _timer = 0f;
            SpawnAtLocation();
        }
    }

    private void SpawnAtLocation()
    {
        Transform selectedSpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Count)];
        Vector3 spawnOffset;

        for (int i = 0; i < _enemySpawnGroupSize; i++)
        {
            GameObject enemy = GetPooledObject();
            enemy.SetActive(true);
            spawnOffset = new Vector3(Random.Range(0, 2f), 0, Random.Range(0, 2f));
            selectedSpawnPoint.position += spawnOffset;
            enemy.transform.position = selectedSpawnPoint.position;
        }
        Debug.Break();
    }

    private GameObject GetPooledObject()
    {
        for (int i = 0; i < _enemyPool.Count; i++)
        {
            if (!_enemyPool[i].activeInHierarchy)
            {
                return _enemyPool[i];
            }
        }
        GameObject newEnemy = Instantiate(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)], _poolParent);
        _enemyPool.Add(newEnemy);
        newEnemy.SetActive(false);
        return newEnemy;
    }
}
