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
    

    private void Update()
    {
        if (!isTimerRunning || isTimerCompleted)
            return;

        timer += Time.deltaTime;

        playerSlider.value = (timer / duration);

        if (playerSlider.value >= playerSlider.maxValue)
        {
            Debug.Log($"{gameObject.name} TIMER ENDED");
            isTimerCompleted = true;
            isTimerRunning = false;

            // onActivationFailed?.Invoke();
        }
    }
    
    private void OnEnable()
    {
        StartAnnihilationSequence();
    }

    private void OnDisable()
    {
        ResetAnnihilationSequence();
    }
    
    public void StartAnnihilationSequence()
    {
        timer = 0f;
        isTimerCompleted = false;
        isTimerRunning = true;

        playerSlider.value = 0f;

        GenerateRandomActivationZone();
        UpdateZoneVisual();
    }

    public void ResetAnnihilationSequence()
    {
        isTimerRunning = false;
        timer = 0f;
        isTimerCompleted = false;
        playerSlider.value = 0f;
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