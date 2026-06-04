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
    public UnityEvent onAnnihilation;

    private float activationZoneCenter;
    private float activationZoneMin;
    private float activationZoneMax;

    private float timer;
    private bool isRunning;
    private bool completed;

    private void Start()
    {
        onActivationSuccess.AddListener(OnActivationSuccess);
        onActivationFailed.AddListener(OnActivationFailed);
    }

    private void Update()
    {
        if (!isRunning || completed)
            return;

        timer += Time.deltaTime;

        playerSlider.value = (timer / duration);

        if (playerSlider.value >= playerSlider.maxValue)
        {
            completed = true;
            isRunning = false;

            onAnnihilation?.Invoke();
        }
    }

    public void StartAnnihilationSequence()
    {
        timer = 0f;
        completed = false;
        isRunning = true;

        playerSlider.value = 0f;

        GenerateRandomActivationZone();
        UpdateZoneVisual();
    }

    public void ResetAnnihilationSequence()
    {
        isRunning = false;
        timer = 0f;
        completed = false;
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
        if (!isRunning || completed)
            return;

        if (IsWithinActivationZone())
        {
            completed = true;
            isRunning = false;

            onActivationSuccess?.Invoke();
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

    void OnActivationSuccess()
    {
        Debug.Log("Psion Form successfully Activated");
    }

    void OnActivationFailed()
    {
        Debug.Log("Psion Form failed to Activate");
    }

    private void OnEnable()
    {
        StartAnnihilationSequence();
    }

    private void OnDisable()
    {
        ResetAnnihilationSequence();
    }
}