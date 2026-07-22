using System;
using UnityEngine;

public class EnergyBarrier : MonoBehaviour
{
    public GlobalEventCalls globalEventCalls;

    [SerializeField] private float minImpactVelocity = 2f;
    [SerializeField] private float cooldown = 0.2f;

    private float lastPlayTime;

    private void Awake()
    {
        globalEventCalls = GetComponent<GlobalEventCalls>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (Time.time - lastPlayTime < cooldown)
            return;

        if (collision.relativeVelocity.magnitude >= minImpactVelocity)
        {
            lastPlayTime = Time.time;
            globalEventCalls.energyBarrierImpact.Invoke();
        }
    }
}