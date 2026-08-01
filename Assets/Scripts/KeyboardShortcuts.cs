using UnityEngine;
using UnityEngine.Events;

public class KeyboardShortcuts : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private UnityEvent onRPressed;
    [SerializeField] private UnityEvent onEscapePressed;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            onRPressed?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onEscapePressed?.Invoke();
        }
    }
}