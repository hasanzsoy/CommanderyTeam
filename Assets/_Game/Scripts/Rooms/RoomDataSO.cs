using UnityEngine;

[CreateAssetMenu(fileName = "SO_Room_New",menuName = "CommanderyTeam/Data/Rooms/Room Data")]
public sealed class RoomDataSO : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string displayName = "Room";
    [Header("Enemies")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField, Min(1)]
    private int enemyCount = 5;
    public string DisplayName => displayName;
    public GameObject EnemyPrefab => enemyPrefab;
    public int EnemyCount => enemyCount;
}