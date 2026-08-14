using UnityEngine;

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

    [Header("Camera Shake")]
    [SerializeField] private float minShakeMagnitude = 0.01f;
    [SerializeField] private float maxShakeMagnitude = 0.08f;

    private CameraShake cameraShake;
    private PauseMenuManager pauseMenu;
    private bool effectsEnabled;

    private void Awake()
    {
        if (annihilationCanvas != null)
            annihilationCanvas.SetActive(false);

        if (Camera.main != null)
            cameraShake = Camera.main.GetComponent<CameraShake>();

        pauseMenu = PauseMenuManager.Instance;
    }

    private void Update()
    {
        if (pauseMenu != null && PauseMenuManager.Instance.IsPaused)
        {
            DisableEffects();
            return;
        }

        if (!playerOne.gameObject.activeInHierarchy || !playerTwo.gameObject.activeInHierarchy)
        {
            DisableEffects();
            enabled = false;
            return;
        }

        float distance = Vector2.Distance(playerOne.position, playerTwo.position);

        HandleEffects(distance);
    }

    private void HandleEffects(float distance)
    {
        bool insideWarningRange = distance <= shakeStartDistance;

        if (insideWarningRange)
        {
            if (!effectsEnabled)
                EnableEffects();

            UpdateCameraShake(distance);
        }
        else if (effectsEnabled)
        {
            DisableEffects();
        }
    }

    private void EnableEffects()
    {
        effectsEnabled = true;

        if (annihilationCanvas != null)
            annihilationCanvas.SetActive(true);

        if (cameraShake != null)
            cameraShake.enabled = true;

        RumbleManager.Instance.StartRumble(0, 0.1f, 0.3f); // Posi
        RumbleManager.Instance.StartRumble(1, 0.1f, 0.3f); // Eli
    }

    private void DisableEffects()
    {
        if (!effectsEnabled)
            return;

        effectsEnabled = false;

        if (annihilationCanvas != null)
            annihilationCanvas.SetActive(false);

        if (cameraShake != null)
        {
            cameraShake.SetMagnitude(0f);
            cameraShake.enabled = false;
        }

        RumbleManager.Instance.StopRumble(0);
        RumbleManager.Instance.StopRumble(1);
    }

    private void UpdateCameraShake(float distance)
    {
        if (cameraShake == null)
            return;

        float t = 1f - Mathf.Clamp01((distance - annihilationDistance) / (shakeStartDistance - annihilationDistance));

        float magnitude = Mathf.Lerp(minShakeMagnitude, maxShakeMagnitude, t);

        cameraShake.SetMagnitude(magnitude);
    }

    private void OnEnable()
    {
        effectsEnabled = false;
    }

    private void OnDisable()
    {
        DisableEffects();
    }
}