using UnityEngine;
using UnityEngine.InputSystem;

public class RebindHandler : MonoBehaviour
{
    // first iteration, can only rebind one side and not controller yet. i'm still figuring out what i'm doing from docs but this is jank.
    public InputActionReference rebindAction;
    public string groupName;
    private InputActionRebindingExtensions.RebindingOperation _operation;

    public void TriggerRebind(string group)
    {
        if (rebindAction)
        {
            InputAction action = rebindAction.action;
            action.Disable();

            _operation = action.PerformInteractiveRebinding()
                .WithCancelingThrough("<Keyboard>/escape").OnCancel(
                    (_) =>
                    {
                        _operation.Dispose();
                        action.Enable();
                    }).OnComplete((_) =>
                {
                    _operation.Dispose();
                    action.Enable();
                });
            if (!string.IsNullOrEmpty(group)) _operation.WithBindingGroup(group);
            _operation.Start();
        }
    }
}
