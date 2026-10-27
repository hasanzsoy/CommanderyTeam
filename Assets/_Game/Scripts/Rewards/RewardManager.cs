using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class RewardManager : MonoBehaviour
{
    [Header("Core References")]
    [SerializeField] private RoomManager roomManager;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private SquadManager squadManager;
    [SerializeField] private SquadCombatStats squadCombatStats;
    [Header("Reward UI")]
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private Button riflemanButton;
    [SerializeField] private Button healButton;
    [SerializeField] private Button damageButton;
    [Header("Reward Data")]
    [SerializeField] private RewardDataSO riflemanReward;
    [SerializeField] private RewardDataSO healReward;
    [SerializeField] private RewardDataSO damageReward;
    [Header("Squad Data")]
    [SerializeField] private SquadMemberDataSO riflemanData;
    [Header("Settings")]
    [SerializeField, Min(0f)]
    private float rewardOpenDelay = 1f;
    private bool rewardSelected;
    public event Action RewardSelected;

    private void Awake()
    {
        if (!ValidateReferences())
        {
            enabled = false;
            return;
        }

        rewardPanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (roomManager != null)
        {
            roomManager.RoomCleared += HandleRoomCleared;
        }

        AddButtonListeners();
    }

    private void OnDisable()
    {
        if (roomManager != null)
        {
            roomManager.RoomCleared -= HandleRoomCleared;
        }

        RemoveButtonListeners();
    }

    private void HandleRoomCleared()
    {
        StartCoroutine(OpenRewardsAfterDelay());
    }

    private IEnumerator OpenRewardsAfterDelay()
    {
        yield return new WaitForSeconds(rewardOpenDelay);
        roomManager.HideRoomClearPanel();
        rewardSelected = false;
        rewardPanel.SetActive(true);
    }

    private void SelectRiflemanReward()
    {
        ApplyReward(riflemanReward);
    }

    private void SelectHealReward()
    {
        Debug.Log("Heal Reward Button Clicked");

        ApplyReward(healReward);
    }

    private void SelectDamageReward()
    {
        ApplyReward(damageReward);
    }

    private void ApplyReward(RewardDataSO rewardData)
    {
        if (rewardSelected || rewardData == null)
        {
            return;
        }
        rewardSelected = true;
        switch (rewardData.RewardType)
        {
            case RewardType.AddRifleman: ApplyRiflemanReward();
                break;

            case RewardType.Heal:ApplyHealReward(rewardData.Amount);
                break;

            case RewardType.DamageUpgrade: ApplyDamageReward(rewardData.Amount);
                break;
        }
        rewardPanel.SetActive(false);
        RewardSelected?.Invoke();
    }
    
    private void ApplyRiflemanReward()
    {
        squadManager.TryAddMember(riflemanData);
    }
    private void ApplyHealReward(float healAmount)
    {
        Debug.Log($"Heal applied. Amount: {healAmount}");

        playerHealth.Heal(healAmount);
    }
    private void ApplyDamageReward(float damagePercent)
    {
        squadCombatStats.AddDamagePercent(damagePercent);
    }

    private void AddButtonListeners()
    {
        if (riflemanButton != null)
        {
            riflemanButton.onClick.AddListener(SelectRiflemanReward);
        }
        if (healButton != null)
        {
            healButton.onClick.AddListener(SelectHealReward);
        }
        if (damageButton != null)
        {
            damageButton.onClick.AddListener(SelectDamageReward);
        }
    }

    private void RemoveButtonListeners()
    {
        if (riflemanButton != null)
        {
            riflemanButton.onClick.RemoveListener(SelectRiflemanReward);
        }
        if (healButton != null)
        {
            healButton.onClick.RemoveListener(SelectHealReward);
        }
        if (damageButton != null)
        {
            damageButton.onClick.RemoveListener(SelectDamageReward);
        }
    }

    private bool ValidateReferences()
    {
        if (roomManager == null || playerHealth == null || squadManager == null || squadCombatStats == null)
        {
            Debug.LogError($"{nameof(RewardManager)} has missing core references.",this);
            return false;
        }
        if (rewardPanel == null || riflemanButton == null || healButton == null || damageButton == null)
        {
            Debug.LogError($"{nameof(RewardManager)} has missing UI references.",this);
            return false;
        }
        if (riflemanReward == null || healReward == null || damageReward == null || riflemanData == null)
        {
            Debug.LogError($"{nameof(RewardManager)} has missing reward data.",this);
            return false;
        }
        return true;
    }
}