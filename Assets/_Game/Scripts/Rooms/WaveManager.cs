using System;
using UnityEngine;

public sealed class WaveManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemySpawner enemySpawner;
    public event Action WaveCompleted;
    public int AliveEnemyCount { get; private set; }
    private bool waveCompleted;
    private void Awake()
    {
        if (enemySpawner != null)
        {
            return;
        }
        Debug.LogError($"{nameof(WaveManager)} requires an " +$"{nameof(EnemySpawner)} reference.",this);
        enabled = false;
    }
    private void OnEnable()
    {
        if (enemySpawner == null)
        {
            return;
        }
        enemySpawner.EnemySpawned += HandleEnemySpawned;
    }
    private void OnDisable()
    {
        if (enemySpawner == null)
        {
            return;
        }
        enemySpawner.EnemySpawned -= HandleEnemySpawned;
    }
    public void StartWave(
        RoomDataSO roomData)
    {
        if (roomData == null)
        {
            Debug.LogError($"{nameof(WaveManager)} received null RoomDataSO.",this);
            return;
        }
        AliveEnemyCount = 0;
        waveCompleted = false;
        enemySpawner.SpawnWave(roomData.EnemyPrefab,roomData.EnemyCount);
        Debug.Log($"{roomData.DisplayName} Started");
    }
    private void HandleEnemySpawned(EnemyHealth enemyHealth)
    {
        if (enemyHealth == null)
        {
            return;
        }
        AliveEnemyCount++;
        enemyHealth.Died += HandleEnemyDied;
    }
    private void HandleEnemyDied(EnemyHealth enemyHealth)
    {
        if (enemyHealth == null)
        {
            return;
        }
        enemyHealth.Died -= HandleEnemyDied;
        AliveEnemyCount = Mathf.Max(AliveEnemyCount - 1,0);
        CheckWaveCompleted();
    }
    private void CheckWaveCompleted()
    {
        if (waveCompleted)
        {
            return;
        }
        if (AliveEnemyCount > 0)
        {
            return;
        }
        waveCompleted = true;
        Debug.Log("Wave Complete");
        WaveCompleted?.Invoke();
    }
}