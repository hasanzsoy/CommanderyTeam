using UnityEngine;

public sealed class MissionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private RoomManager roomManager;
    [SerializeField] private RewardManager rewardManager;
    [Header("Mission")]
    [SerializeField] private RoomDataSO[] rooms;
    public int CurrentRoomIndex { get; private set; } = -1;
    public bool IsMissionCompleted { get; private set; }
    private void Awake()
    {
        if (!ValidateReferences())
        {
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (rewardManager == null)
        {
            return;
        }
        rewardManager.RewardSelected += HandleRewardSelected;
    }

    private void Start()
    {
        if (!enabled)
        {
            return;
        }
        StartNextRoom();
    }

    private void OnDisable()
    {
        if (rewardManager == null)
        {
            return;
        }
        rewardManager.RewardSelected -= HandleRewardSelected;
    }

    private void HandleRewardSelected()
    {
        StartNextRoom();
    }

    private void StartNextRoom()
    {
        CurrentRoomIndex++;
        if (CurrentRoomIndex >= rooms.Length)
        {
            CompleteMission();
            return;
        }
        roomManager.PrepareNextWave();
        RoomDataSO currentRoom = rooms[CurrentRoomIndex];
        Debug.Log($"Starting {currentRoom.DisplayName}");
        waveManager.StartWave(currentRoom);
    }

    private void CompleteMission()
    {
        IsMissionCompleted = true;
        Debug.Log("Mission Complete");
    }

    private bool ValidateReferences()
    {
        if (waveManager == null)
        {
            Debug.LogError($"{nameof(MissionManager)} requires a " +$"{nameof(WaveManager)} reference.",this);
            return false;
        }
        if (roomManager == null)
        {
            Debug.LogError($"{nameof(MissionManager)} requires a " +$"{nameof(RoomManager)} reference.",this);
            return false;
        }
        if (rewardManager == null)
        {
            Debug.LogError($"{nameof(MissionManager)} requires a " +$"{nameof(RewardManager)} reference.",this);
            return false;
        }
        if (rooms == null || rooms.Length == 0)
        {
            Debug.LogError($"{nameof(MissionManager)} requires at least one room.",this);
            return false;
        }
        return true;
    }
}