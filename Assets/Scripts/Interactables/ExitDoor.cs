using System;
using UnityEngine;
using UnityEngine.Events;

public class ExitDoor : MonoBehaviour
{
    private int currentPlayerCount = 0;

    public AnnihilationChecker annihilationChecker;
    public UnityEvent winEvent;
    
    private void Start()
    {
        annihilationChecker = FindFirstObjectByType<AnnihilationChecker>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name + " entered");

            if (annihilationChecker)
            {
                annihilationChecker.gameObject.SetActive(false);
            }
            
            currentPlayerCount++;

            if (currentPlayerCount == 2)
            {
                winEvent.Invoke();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (annihilationChecker)
            {
                annihilationChecker.gameObject.SetActive(true);
            }
            
            currentPlayerCount--;
        }
    }
}