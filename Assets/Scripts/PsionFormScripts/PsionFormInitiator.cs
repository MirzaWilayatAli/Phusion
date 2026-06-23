using System;
using UnityEngine;

public class PsionFormInitiator : MonoBehaviour
{
    public PsionFormManager psionFormManager;
    private bool player1ReadyForPsionForm;
    private bool player2ReadyForPsionForm;

    private void Start()
    {
        player1ReadyForPsionForm = false;
        player2ReadyForPsionForm = false;
    }

    public void InitiateQuantumHandshake(int playerID)
    {
        if (playerID == 0)
            player1ReadyForPsionForm = true;

        if (playerID == 1)
            player2ReadyForPsionForm = true;
        
        Debug.Log($"Player {playerID} ready for Quantum Handshake");
        
        if (player1ReadyForPsionForm && player2ReadyForPsionForm && !psionFormManager.psionFormInitiated)
        {
            psionFormManager.EnterPsionForm();
        }
    }
    public void TerminateQuantumHandshake(int playerID)
    {
        if (playerID == 0)
            player1ReadyForPsionForm = false;

        if (playerID == 1)
            player2ReadyForPsionForm = false;
        
        Debug.Log($"Player {playerID} has terminated Quantum Handshake");

        if (!player1ReadyForPsionForm && !player2ReadyForPsionForm && psionFormManager.psionFormInitiated)
        {
            psionFormManager.gameObject.SetActive(false);
        }
    }
}
