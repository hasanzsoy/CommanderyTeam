using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject enemyPrefab;
    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;
    [Header("Settings")]
    [SerializeField, Min(1)]
    private int spawnCount = 5;
    private readonly List<EnemyHealth> spawnedEnemies = new();
    public event Action<EnemyHealth> EnemySpawned;
    public IReadOnlyList<EnemyHealth> SpawnedEnemies => spawnedEnemies;

    private void Start()
    {
        if (!ValidateReferences())
        {
            enabled = false;
            return;
        }
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        int finalSpawnCount = Mathf.Min(spawnCount,spawnPoints.Length);
        for (int i = 0; i < finalSpawnCount; i++)
        {
            SpawnEnemyAt(spawnPoints[i]);
        }
    }

    private void SpawnEnemyAt(Transform spawnPoint)
    {
        if (spawnPoint == null)
        {
            return;
        }
        GameObject enemyInstance = Instantiate(enemyPrefab,spawnPoint.position,spawnPoint.rotation);
        if (!enemyInstance.TryGetComponent(out EnemyHealth enemyHealth))
        {
            Debug.LogError($"{enemyPrefab.name} requires an " + $"{nameof(EnemyHealth)} component.",enemyInstance);
            Destroy(enemyInstance);
            return;
        }
        spawnedEnemies.Add(enemyHealth);
        EnemySpawned?.Invoke(enemyHealth);
    }

    private bool ValidateReferences()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError($"{nameof(EnemySpawner)} requires an enemy prefab.",this);
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