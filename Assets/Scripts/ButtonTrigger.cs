using System;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonTrigger : TriggerBase
{
    public string triggerTag = "";
    public LayerMask mask;
    public bool triggered = false;
    private void FixedUpdate()
    {
        Vector2 x = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] _ = Physics2D.OverlapCircleAll(x, 0.5f, mask);
        if (_.Length > 0)
        {
            if (!triggered)
            {
                foreach (Collider2D col in _)
                {
                    Debug.Log("entered");
                    if (col.CompareTag(triggerTag))
                    {
                        triggered = true;
                        onTriggerOn.Invoke();
                        if (col.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                        {
                            rb.linearVelocity = Vector2.zero;
                        }
                        break;
                    }
                }                
            }
        }
        else
        {
            if (triggered)
            {
                Debug.Log("left");
                triggered = false;
                onTriggerOff.Invoke();
            }
        }
    }
}
