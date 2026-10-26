using System;
using UnityEngine;

public sealed class RoomManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private GameObject roomClearPanel;
    public event Action RoomCleared;
    public bool IsRoomCleared { get; private set; }

    private void Awake()
    {
        if (!ValidateReferences())
        {
            enabled = false;
            return;
        }

        roomClearPanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (waveManager == null)
        {
            return;
        }

        waveManager.WaveCompleted += HandleWaveCompleted;
    }

    private void OnDisable()
    {
        if (waveManager == null)
        {
            return;
        }

        waveManager.WaveCompleted -= HandleWaveCompleted;
    }

    public void HideRoomClearPanel()
    {
        if (roomClearPanel == null)
        {
            return;
        }

        roomClearPanel.SetActive(false);
    }

    private void HandleWaveCompleted()
    {
        if (IsRoomCleared)
        {
            return;
        }
        IsRoomCleared = true;
        roomClearPanel.SetActive(true);
        Debug.Log("Room Cleared");
        RoomCleared?.Invoke();
    }

    private bool ValidateReferences()
    {
        if (waveManager == null)
        {
            Debug.LogError($"{nameof(RoomManager)} requires a " + $"{nameof(WaveManager)} reference.",this);
            return false;
        }

        if (roomClearPanel == null)
        {
            Debug.LogError($"{nameof(RoomManager)} requires a Room Clear Panel.",this);
            return false;
        }

        return true;
    }
}