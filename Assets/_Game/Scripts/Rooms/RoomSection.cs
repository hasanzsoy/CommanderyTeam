using UnityEngine;

public sealed class RoomSection : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private RoomDataSO roomData;
    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;
    [Header("Exit")]
    [SerializeField] private GateController exitGate;
    public RoomDataSO RoomData => roomData;
    public Transform[] SpawnPoints => spawnPoints;

    public void OpenExitGate()
    {
        if (exitGate == null)
        {
            return;
        }
        exitGate.OpenGate();
    }

    public void CloseExitGate()
    {
        if (exitGate == null)
        {
            return;
        }
        exitGate.CloseGate();
    }
}