using UnityEngine;
using UnityEngine.UI;

public class FetchControllerVibrationSettings : MonoBehaviour
{
    [SerializeField] private Toggle vibrationToggle;

    private void OnEnable()
    {
        if (RumbleManager.Instance != null)
        {
            // This will get the current settings from RumbleManager
            vibrationToggle.SetIsOnWithoutNotify(RumbleManager.Instance.RumbleEnabled);

            // Adding Listener through code so that I don't have to do it from inspector in every God dem scene
            vibrationToggle.onValueChanged.AddListener(SetRumble);
        }
        else
        {
            Debug.LogWarning("FetchControllerVibrationSettings: RumbleManager instance not found.");
        }
    }

    private void OnDisable()
    {
        vibrationToggle.onValueChanged.RemoveListener(SetRumble);
    }

    private void SetRumble(bool enabled)
    {
        if (RumbleManager.Instance != null)
        {
            RumbleManager.Instance.SetRumbleEnabled(enabled);
        }
    }
}