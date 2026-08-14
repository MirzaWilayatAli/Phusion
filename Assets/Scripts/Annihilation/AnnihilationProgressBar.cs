using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class AnnihilationProgressBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider playerSlider;
    [SerializeField] private RectTransform sliderRect;
    [SerializeField] private RectTransform activationZoneVisual;

    [Header("Settings")]
    [SerializeField] private float activationZoneWidth;
    [SerializeField] private float activationZoneMinWidth;
    [SerializeField] private float activationZoneMaxWidth;

    [SerializeField] private float duration = 2f;

    [Header("Events")]
    public UnityEvent onActivationSuccess;
    public UnityEvent onActivationFailed;

    private float activationZoneCenter;
    private float activationZoneMin;
    private float activationZoneMax;

    [SerializeField] private float timer;
    [SerializeField] private bool isTimerRunning;
    [SerializeField] private bool isTimerCompleted;
    
    [SerializeField] private Image sliderFill;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    private bool wasPaused;
    
    private void Update()
    {
        if (!isTimerRunning || isTimerCompleted)
            return;

        // Don't advance the annihilation timer while paused.
        if (PauseMenuManager.Instance != null && PauseMenuManager.Instance.IsPaused)
            return;

        timer += Time.deltaTime;

        playerSlider.value = timer / duration;

        // Calculate tint amount
        float t = Mathf.InverseLerp(0.7f, 1f, playerSlider.value);

        // Smoothly change from white to red
        sliderFill.color = Color.Lerp(startColor, endColor, t);

        if (playerSlider.value >= playerSlider.maxValue)
        {
            Debug.Log($"{gameObject.name} TIMER ENDED");
            isTimerCompleted = true;
            isTimerRunning = false;
            onActivationFailed?.Invoke();
        }
    }
    
    private void OnEnable()
    {
        if (wasPaused)
        {
            wasPaused = false;
            return;
        }

        StartAnnihilationSequence();
    }

    private void OnDisable()
    {
        if (PauseMenuManager.Instance != null && PauseMenuManager.Instance.IsPaused)
        {
            wasPaused = true;
            return;
        }

        ResetAnnihilationSequence();
    }
    
    public void StartAnnihilationSequence()
    {
        timer = 0f;
        isTimerCompleted = false;
        isTimerRunning = true;

        playerSlider.value = 0f;
        sliderFill.color = startColor;

        GenerateRandomActivationZone();
        UpdateZoneVisual();
    }

    public void ResetAnnihilationSequence()
    {
        isTimerRunning = false;
        timer = 0f;
        isTimerCompleted = false;
        playerSlider.value = 0f;
        sliderFill.color = startColor;
    }

    public bool IsWithinActivationZone()
    {
        float currentValue = playerSlider.value;

        return currentValue >= activationZoneMin && currentValue <= activationZoneMax;
    }
    
    /// This method is called when player presses the Psion button.
    public void ActivatePsionForm()
    {
        if (!isTimerRunning || isTimerCompleted)
            return;

        if (IsWithinActivationZone())
        {
            isTimerCompleted = true;
            isTimerRunning = false;

            onActivationSuccess?.Invoke();
            Debug.Log($"SUCCESS - Running:{isTimerRunning} Completed:{isTimerCompleted}");
        }
        else
        {
            onActivationFailed?.Invoke();
        }
    }

    public void GenerateRandomActivationZone()
    {
        float zoneWidth = activationZoneWidth; // like 0.1 would be = 10%

        float minCenter = activationZoneMinWidth + zoneWidth * 0.5f;
        float maxCenter = activationZoneMaxWidth - zoneWidth * 0.5f;

        activationZoneCenter = Random.Range(minCenter, maxCenter);

        activationZoneMin = activationZoneCenter - zoneWidth * 0.5f;
        activationZoneMax = activationZoneCenter + zoneWidth * 0.5f;
    }

    public void UpdateZoneVisual()
    {
        activationZoneVisual.anchorMin = new Vector2(activationZoneMin, activationZoneVisual.anchorMin.y);
        activationZoneVisual.anchorMax = new Vector2(activationZoneMax, activationZoneVisual.anchorMax.y);
    }
}