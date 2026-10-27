using UnityEngine;

public sealed class GateController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject gateBlocker;
    public bool IsOpen { get; private set; }
    private void Awake()
    {
        CloseGate();
    }
    public void OpenGate()
    {
        if (IsOpen)
        {
            return;
        }
        IsOpen = true;
        gateBlocker.SetActive(false);
    }

    public void CloseGate()
    {
        IsOpen = false;
        if (gateBlocker != null)
        {
            gateBlocker.SetActive(true);
        }
    }
}