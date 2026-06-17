using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class DeviceAssigner : MonoBehaviour
{
    [Header("Player References")]
    public PlayerInput player1;
    public PlayerInput player2;

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
        AutoAssignDevices();
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
            case InputDeviceChange.Removed:
            case InputDeviceChange.Disconnected:
            case InputDeviceChange.Reconnected:
                AutoAssignDevices();
                break;
        }
    }

    private void AutoAssignDevices()
    {
        int gamepadCount = Gamepad.all.Count;
        bool keyboardExists = Keyboard.current != null;

        Debug.Log($"Keyboard: {keyboardExists}, Gamepads: {gamepadCount}");

        if (keyboardExists && gamepadCount == 0)
        {
            AssignSharedKeyboard();
        }
        else if (keyboardExists && gamepadCount == 1)
        {
            AssignKeyboardAndGamepad();
        }
        else if (gamepadCount >= 2)
        {
            AssignTwoGamepads();
        }
        else
        {
            Debug.LogWarning("No valid control setup found.");
        }
    }

    // ----------------------------
    // Shared Keyboard
    // ----------------------------
    private void AssignSharedKeyboard()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        player1.user.UnpairDevices();
        player2.user.UnpairDevices();

        InputUser.PerformPairingWithDevice(keyboard, player1.user);
        InputUser.PerformPairingWithDevice(keyboard, player2.user);

        player1.SwitchCurrentControlScheme("WASD", keyboard);
        player2.SwitchCurrentControlScheme("Arrows", keyboard);

        Debug.Log("Shared Keyboard Assigned");
    }

    // ----------------------------
    // Keyboard + Gamepad
    // ----------------------------
    private void AssignKeyboardAndGamepad()
    {
        var keyboard = Keyboard.current;
        var gamepad = Gamepad.all[0];

        player1.user.UnpairDevices();
        player2.user.UnpairDevices();

        player1.SwitchCurrentControlScheme("WASD", keyboard);
        player2.SwitchCurrentControlScheme("Gamepad", gamepad);

        Debug.Log("Keyboard + Gamepad Assigned");
    }

    // ----------------------------
    // Two Gamepads
    // ----------------------------
    private void AssignTwoGamepads()
    {
        var gamepads = Gamepad.all;

        if (gamepads.Count < 2)
            return;

        player1.user.UnpairDevices();
        player2.user.UnpairDevices();

        player1.SwitchCurrentControlScheme("Gamepad", gamepads[0]);
        player2.SwitchCurrentControlScheme("Gamepad", gamepads[1]);

        Debug.Log("Two Gamepads Assigned");
    }
}