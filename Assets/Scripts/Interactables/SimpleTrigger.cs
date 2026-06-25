using System;
using UnityEngine;

public class SimpleTrigger : TriggerBase
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        onTriggerOn?.Invoke();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        onTriggerOff?.Invoke();
    }
}
