using UnityEngine;
using UnityEngine.UI;

public class ResetBindingsButton : MonoBehaviour
{
    private Button button;

    private void OnEnable()
    {
        button = GetComponent<Button>();

        if (RebindManager.Instance != null)
        {
            button.onClick.AddListener(RebindManager.Instance.ResetAllRebinds);
        }
        else
        {
            Debug.LogWarning("ResetBindingsButton: RebindManager instance not found.");
        }
    }

    private void OnDisable()
    {
        if (RebindManager.Instance != null)
        {
            button.onClick.RemoveListener(RebindManager.Instance.ResetAllRebinds);
        }
    }
}