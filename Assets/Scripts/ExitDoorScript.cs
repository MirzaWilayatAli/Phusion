using System;
using UnityEngine;

public class ExitDoorScript : MonoBehaviour
{
    public GameObject winUICanvas;
    private int currentPlayerCount = 0;

    public AnnihilationChecker annihilationChecker;

    private void Start()
    {
        annihilationChecker = FindFirstObjectByType<AnnihilationChecker>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name + " entered");
            
            annihilationChecker.gameObject.SetActive(false);
            
            currentPlayerCount++;

            if (currentPlayerCount == 2)
            {
                winUICanvas.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            annihilationChecker.gameObject.SetActive(true);
            
            currentPlayerCount--;

            if (currentPlayerCount < 2)
            {
                winUICanvas.SetActive(false);
            }
        }
    }
}