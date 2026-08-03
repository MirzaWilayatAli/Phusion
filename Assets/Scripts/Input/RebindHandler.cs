using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class RebindHandler : MonoBehaviour
{
    public InputActionReference rebindAction;
    [FormerlySerializedAs("groupName")] public string actionMapName;
    private InputActionRebindingExtensions.RebindingOperation _operation;
    public string playerPrefKey = "Default";

    public UnityEvent OnSuccess;
    public UnityEvent OnCancel;
    public void TriggerRebind()
    {
        string group = actionMapName;
        Debug.Log(group);
        if (rebindAction)
        {
            InputAction action = rebindAction.action;
            
            action.Disable();

            _operation = action.PerformInteractiveRebinding()
                .WithCancelingThrough("<Keyboard>/escape").OnCancel(
                    (_) =>
                    {
                        Debug.Log("Fail");
                        _operation.Dispose();
                        OnCancel.Invoke();
                        action.Enable();
                    }).OnComplete((_) =>
                {
                    Debug.Log("Success");
                    _operation.Dispose();
                    OnSuccess.Invoke();
                    action.Enable();
                    Save();
                });
            if (!string.IsNullOrEmpty(group)) _operation.WithBindingGroup(group);
            _operation.Start();
        }
    }

    public void ResetAllBindingOverrides()
    {
        ClearKey();
        rebindAction.asset.RemoveAllBindingOverrides();
    }

    public void ClearIndividualBinding()
    {
        rebindAction.action.RemoveAllBindingOverrides();
        Save();
    }
    public void ClearKey()
    {
        PlayerPrefs.DeleteKey($"BindingOverrides_{playerPrefKey}");
    }
    public void Save()
    {
        string customBindings = rebindAction.action.actionMap.SaveBindingOverridesAsJson();
        
        PlayerPrefs.SetString($"BindingOverrides_{playerPrefKey}", customBindings);
        PlayerPrefs.Save();
    }
}
