using UnityEngine;
using UnityEngine.InputSystem;

public class DeviceAssigner : MonoBehaviour
{
    [Header("Player Prefabs/Components")]
    public PlayerInput player1;
    public PlayerInput player2;

   
    // One uses WASD and one uses Arrow keys
    
    public void AssignSharedKeyboard()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // Player 1: Keyboard using WASD scheme
        player1.SwitchCurrentControlScheme("WASD", keyboard);

        // Player 2: Keyboard using Arrows scheme
        player2.SwitchCurrentControlScheme("Arrows", keyboard);
        
        Debug.Log("Shared Keyboard Assigned.");
    }

    // One uses Keyboard and one uses a Gamepad

    public void AssignKeyboardAndGamepad()
    {
        var keyboard = Keyboard.current;
        var gamepads = Gamepad.all;

        if (keyboard != null)
            player1.SwitchCurrentControlScheme("WASD", keyboard);

        if (gamepads.Count > 0)
            player2.SwitchCurrentControlScheme("Gamepad", gamepads[0]);

        Debug.Log("Keyboard and Gamepad Assigned.");
    }
    
    
    // Both use Gamepads
    
    public void AssignTwoGamepads()
    {
        var gamepads = Gamepad.all;

        if (gamepads.Count >= 2)
        {
            player1.SwitchCurrentControlScheme("Gamepad", gamepads[0]);
            player2.SwitchCurrentControlScheme("Gamepad", gamepads[1]);
            Debug.Log("Two Gamepads Assigned.");
        }
        else
        {
            Debug.LogWarning("Not enough gamepads connected!");
        }
    }
}