using System;
using UnityEngine;

public class MenuReset : MonoBehaviour
{
    // PauseMenuManager sets simulationMode, and we just want to make sure it's correct every time something loads, so the game loads fine.
    private void Awake()
    {
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
    }

    public void ResetPhysics()
    {
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
    }
}
