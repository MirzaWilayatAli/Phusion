using UnityEngine;
using UnityEngine.InputSystem;

public class CreditScroller : MonoBehaviour
{
    [Header("Scrolling")]
    [SerializeField] private float autoScrollSpeed = 60f;

    private void Update()
    {
        if (IsPauseHeld())
            return;

        transform.position += Vector3.down * autoScrollSpeed * Time.deltaTime;
    }

    private bool IsPauseHeld()
    {
        // Keyboard Space
        if (Keyboard.current != null && Keyboard.current.spaceKey.isPressed)
            return true;

        // Any connected gamepad X button
        foreach (Gamepad gamepad in Gamepad.all)
        {
            if (gamepad.buttonWest.isPressed)
                return true;
        }

        return false;
    }
}