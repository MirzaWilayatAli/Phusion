using System;
using UnityEngine;

public class ExitDoorScript : MonoBehaviour
{
    public GameObject winUICanvas;
    private int currentPlayerCount = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name + " entered");
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
            currentPlayerCount--;

            if (currentPlayerCount < 2)
            {
                winUICanvas.SetActive(false);
            }
        }
    }
}