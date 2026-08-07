using UnityEngine;
using UnityEngine.UI;

public class FetchControllerVibrationSettings : MonoBehaviour
{
    [SerializeField] private Toggle vibrationToggle;

    private void Awake()
    {
        if (RumbleManager.Instance != null)
        {
            vibrationToggle.SetIsOnWithoutNotify(RumbleManager.Instance.RumbleEnabled);
        }
    }

    public void SetRumble(bool enabled)
    {
        if (RumbleManager.Instance != null)
        {
            RumbleManager.Instance.SetRumbleEnabled(enabled);
        }
    }
}