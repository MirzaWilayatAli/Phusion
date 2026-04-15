using System;
using UnityEngine;
using UnityEngine.UI;

public class AnnihilationChecker : MonoBehaviour
{
    public Transform playerOne;
    public Transform playerTwo;

    public Image image;    
    private void FixedUpdate()
    {
        float distance = Vector3.Distance(playerOne.position, playerTwo.position);
        
        if (distance <= 1f)
        {
            Debug.Log("Annihilate!");
        }
        else
        {
            float alpha = 0;
            if (distance <= 2f)
            {
                alpha = 0.15f / distance;
            }
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        }
    }
}
