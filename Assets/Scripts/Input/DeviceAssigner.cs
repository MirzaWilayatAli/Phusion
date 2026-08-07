using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public enum ControlType
{
    Unknown,
    SharedKeyboard,
    TwoGamepads,
    OneGamepadOneKeyboard
}

public class DeviceAssigner : MonoBehaviour
{
    [Header("Player References")]
    public PlayerInput player1;
    public PlayerInput player2;

    [Header("Misc")]
    public ControlType controlType = ControlType.Unknown;
    public Action onTypeChange;

    private void OnEnable()
    {
        // Completely disable mouse input
        if (Mouse.current != null)
            InputSystem.DisableDevice(Mouse.current);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        InputSystem.onDeviceChange += OnDeviceChange;
        AutoAssignDevices();
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;

        if (Mouse.current != null)
            InputSystem.EnableDevice(Mouse.current);
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

        if (keyboardExists && gamepadCount == 0)
        {
            controlType = ControlType.SharedKeyboard;
            AssignSharedKeyboard();
        }
        else if (keyboardExists && gamepadCount == 1)
        {
            controlType = ControlType.OneGamepadOneKeyboard;
            AssignKeyboardAndGamepad();
        }
        else if (gamepadCount >= 2)
        {
            controlType = ControlType.TwoGamepads;
            AssignTwoGamepads();
        }
        else
        {
            controlType = ControlType.Unknown;
        }

        onTypeChange?.Invoke();
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
        
        RumbleManager.Instance.RegisterPlayerPad(0, null);
        RumbleManager.Instance.RegisterPlayerPad(1, null);
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
        
        RumbleManager.Instance.RegisterPlayerPad(0, null);
        RumbleManager.Instance.RegisterPlayerPad(1, gamepad);
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
        
        RumbleManager.Instance.RegisterPlayerPad(0, gamepads[0]);
        RumbleManager.Instance.RegisterPlayerPad(1, gamepads[1]);
    }
}