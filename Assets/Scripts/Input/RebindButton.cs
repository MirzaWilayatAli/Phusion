using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RebindButton : MonoBehaviour
{
    [Header("Rebind Settings")]
    [SerializeField] private int playerIndex;
    [SerializeField] private string actionName;
    [SerializeField] private int bindingIndex;

    [Header("UI")]
    [SerializeField] private TMP_Text bindingText;

    [Header("Display Text")]
    [SerializeField] private string waitingText = "PRESS A KEY...";
    
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        RefreshBindingDisplay();
    }

    // Called by the UI Button
    public void StartRebind()
    {
        if (RebindManager.Instance == null)
        {
            Debug.LogError("RebindManager instance not found.");
            return;
        }

        button.interactable = false;

        if (bindingText != null)
            bindingText.text = waitingText;

        RebindManager.Instance.StartRebind(
            playerIndex,
            actionName,
            bindingIndex,
            button,
            OnRebindComplete,
            OnRebindCancelled
        );
    }

    private void OnRebindComplete()
    {
        button.interactable = true;
        
        EventSystem.current.SetSelectedGameObject(button.gameObject);

        RefreshBindingDisplay();
    }

    private void OnRebindCancelled()
    {
        button.interactable = true;
        
        EventSystem.current.SetSelectedGameObject(button.gameObject);

        RefreshBindingDisplay();
    }

    public void RefreshBindingDisplay()
    {
        if (RebindManager.Instance == null)
            return;

        InputActionAsset asset =
            RebindManager.Instance.GetPlayerAssetPublic(playerIndex);

        if (asset == null)
            return;

        InputAction action = asset.FindAction(actionName);

        if (action == null)
            return;

        if (bindingIndex < 0 || bindingIndex >= action.bindings.Count)
            return;

        InputBinding binding = action.bindings[bindingIndex];

        string displayName;

        if (!string.IsNullOrEmpty(binding.overridePath))
        {
            displayName = InputControlPath.ToHumanReadableString(
                binding.overridePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice
            );
        }
        else
        {
            displayName = InputControlPath.ToHumanReadableString(
                binding.path,
                InputControlPath.HumanReadableStringOptions.OmitDevice
            );
        }

        displayName = FormatBindingDisplay(displayName);

        if (bindingText != null)
        {
            bindingText.text = displayName;
        }
    }
    
    private string FormatBindingDisplay(string displayString)
    {
        switch (displayString)
        {
            case "Button South":
                return "A";

            case "Button East":
                return "B";

            case "Button West":
                return "X";

            case "Button North":
                return "Y";

            case "Left Shoulder":
                return "LB";

            case "Right Shoulder":
                return "RB";

            case "Left Trigger":
                return "LT";

            case "Right Trigger":
                return "RT";

            default:
                return displayString;
        }
    }
}