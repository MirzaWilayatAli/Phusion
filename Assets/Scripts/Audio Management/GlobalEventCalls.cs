using UnityEngine;
using UnityEngine.Events;

public class GlobalEventCalls : MonoBehaviour
{
    public UnityEvent onPlayerDeath;
    public UnityEvent onPsionDeath;
    public UnityEvent onObjectDestroyed;
    public UnityEvent energyBarrierImpact;

    public void PlayerDeath() => onPlayerDeath?.Invoke();
    public void PsionDeath() => onPsionDeath?.Invoke();
    public void ObjectDestroyed() => onObjectDestroyed?.Invoke();
    public void EnergyBarrierImpact() => energyBarrierImpact?.Invoke();
}