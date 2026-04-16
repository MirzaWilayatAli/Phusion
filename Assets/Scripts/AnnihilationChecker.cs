using System;
using UnityEngine;
using UnityEngine.UI;

public class AnnihilationChecker : MonoBehaviour
{
    [Header("Player References")]
    public Transform playerOne;
    public Transform playerTwo;
    
    [Header("Shake Settings")]
    [SerializeField] private float minShakeMagnitude = 0.01f;
    [SerializeField] private float maxShakeMagnitude = 0.08f;
    [SerializeField] private float shakeStartDistance = 2f;
    [SerializeField] private float annihilationDistance = 1f;
    
    public Image image;

    public CameraShake cameraShakeComponent;
    
    private void Start()
    {
        cameraShakeComponent = Camera.main.GetComponent<CameraShake>();
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(playerOne.position, playerTwo.position);
        
        if (distance <= 1f)
        {
            Debug.Log("Annihilated!");
        }
        else
        {
            float alpha = 0;
            
            if (distance <= shakeStartDistance)
            {
                if (cameraShakeComponent != null)
                {
                    cameraShakeComponent.enabled = true;
                    
                    float t = 1f - Mathf.Clamp01((distance - annihilationDistance) / (shakeStartDistance - annihilationDistance));
                    float currentMagnitude = Mathf.Lerp(minShakeMagnitude, maxShakeMagnitude, t);

                    cameraShakeComponent.SetMagnitude(currentMagnitude);
                }

                alpha = 0.15f / distance;
            }
            else
            {
                if (cameraShakeComponent != null)
                {
                    cameraShakeComponent.enabled = false;
                }
            }
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        }
        
        float prox = 1f - Mathf.Clamp(distance / 2f, 0, 1);

        Time.timeScale = Mathf.Lerp(1, 0.1f, prox);
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}
