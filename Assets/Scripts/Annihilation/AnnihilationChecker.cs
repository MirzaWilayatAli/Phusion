using System;
using UnityEngine;
using UnityEngine.UI;

public class AnnihilationChecker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerOne;
    [SerializeField] private Transform playerTwo;
    [SerializeField] private SceneLoader loader;
    [SerializeField] private GameObject annihilationCanvas;

    [Header("Distance Settings")]
    [SerializeField] private float annihilationDistance = 1f;
    [SerializeField] private float shakeStartDistance = 2f;
    [SerializeField] private float annihilationDelay = 1f;

    [Header("Camera Shake")]
    [SerializeField] private float minShakeMagnitude = 0.01f;
    [SerializeField] private float maxShakeMagnitude = 0.08f;

    private CameraShake cameraShake;
    private float annihilationTimer;

    private void Awake()
    {
        annihilationCanvas.SetActive(false);

        if (Camera.main != null)
        {
            cameraShake = Camera.main.GetComponent<CameraShake>();
        }
    }

    private void FixedUpdate()
    {
        if (playerOne.gameObject.activeInHierarchy == false || playerTwo.gameObject.activeInHierarchy == false)
        {
            annihilationCanvas.SetActive(false);
            enabled = false;
            return;
        }

        float distance = GetPlayerDistance();

        
        HandleAnnihilation(distance);
        HandleVisualEffects(distance);
        HandleTimeSlowdown(distance);
    }

    private float GetPlayerDistance()
    {
        return Vector3.Distance(playerOne.position, playerTwo.position);
    }

    private void HandleAnnihilation(float distance)
    {
        if (distance > annihilationDistance)
        {
            annihilationTimer = 0f;
            return;
        }

        annihilationTimer += Time.deltaTime;

        if (annihilationTimer >= annihilationDelay)
        {
            loader.ReloadScene();
        }
    }

    private void HandleVisualEffects(float distance)
    {
        if (distance > shakeStartDistance)
        {
            DisableEffects();
            return;
        }

        annihilationCanvas.SetActive(true);

        UpdateCameraShake(distance);
    }

    private void UpdateCameraShake(float distance)
    {
        if (cameraShake == null) return;

        cameraShake.enabled = true;

        float t = 1f - Mathf.Clamp01((distance - annihilationDistance) / (shakeStartDistance - annihilationDistance));

        float magnitude = Mathf.Lerp(minShakeMagnitude, maxShakeMagnitude, t);

        cameraShake.SetMagnitude(magnitude);
    }

    private void HandleTimeSlowdown(float distance)
    {
        float proximity = 1f - Mathf.Clamp(distance / shakeStartDistance, 0f, 1f);

        Time.timeScale = Mathf.Lerp(1f, 0.1f, proximity);
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    private void DisableEffects()
    {
        annihilationCanvas.SetActive(false);

        if (cameraShake != null)
        {
            cameraShake.enabled = false;
            cameraShake.SetMagnitude(0f);
        }
    }

    private void OnEnable()
    {
        annihilationTimer = 0;
    }

    private void OnDisable()
    {
        DisableEffects();

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}