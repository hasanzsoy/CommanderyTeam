using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;
    private readonly List<EnemyHealth> spawnedEnemies = new();
    public event Action<EnemyHealth> EnemySpawned;
    public IReadOnlyList<EnemyHealth> SpawnedEnemies => spawnedEnemies;
    public void SpawnWave(GameObject enemyPrefab,int enemyCount)
    {
        if (!ValidateReferences(enemyPrefab,enemyCount))
        {
            return;
        }
        spawnedEnemies.Clear();
        int finalSpawnCount = Mathf.Min(enemyCount,spawnPoints.Length);
        for (int i = 0; i < finalSpawnCount; i++)
        {
            SpawnEnemyAt(enemyPrefab,spawnPoints[i]);
        }
    }

    private void SpawnEnemyAt(GameObject enemyPrefab,Transform spawnPoint)
    {
        if (spawnPoint == null)
        {
            return;
        }
        GameObject enemyInstance = Instantiate(enemyPrefab,spawnPoint.position,spawnPoint.rotation);
        if (!enemyInstance.TryGetComponent(out EnemyHealth enemyHealth))
        {
            Debug.LogError($"{enemyPrefab.name} requires an " +$"{nameof(EnemyHealth)} component.",enemyInstance);
            Destroy(enemyInstance);
            return;
        }
        spawnedEnemies.Add(enemyHealth);
        EnemySpawned?.Invoke(enemyHealth);
    }

    private bool ValidateReferences(GameObject enemyPrefab,int enemyCount)
    {
        if (enemyPrefab == null)
        {
            Debug.LogError($"{nameof(EnemySpawner)} received a null enemy prefab.",this);
            return false;
        }

        if (enemyCount <= 0)
        {
            Debug.LogError($"{nameof(EnemySpawner)} received an invalid enemy count.",this);
            return false;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError($"{nameof(EnemySpawner)} requires at least one spawn point.",this);
            return false;
        }
        return true;
    }
}