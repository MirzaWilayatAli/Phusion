using System;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonTrigger : TriggerBase
{
    public string triggerTag = "";
    public LayerMask mask;
    public bool triggered = false;
    [SerializeField] private Transform playerCheckCenter;
    [SerializeField] private Vector2 playerCheckBoxSize = Vector2.one;
    public LayerMask playerCheckMask; 
    private bool _triggerDisable;

    private MechanicTag triggeredObj;
    
    private void FixedUpdate()
    {
        if (triggeredObj)
        {
            if(!triggeredObj.neg && !triggeredObj.pos) triggeredObj.rb.linearVelocity = Vector2.zero;
        }

        Vector2 x = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] _ = Physics2D.OverlapCircleAll(x, 0.5f, mask);
        if (_.Length > 0)
        {
            if (!triggered)
            {
                foreach (Collider2D col in _)
                {
                    if (col.TryGetComponent(out TriggerTag ttag))
                    {
                        if (ttag.newTag == triggerTag)
                        {
                            triggered = true;
                            onTriggerOn.Invoke();
                            
                            if (col.TryGetComponent(out MechanicTag rb))
                            {
                                if(!rb.neg || !rb.pos) rb.rb.linearVelocity = Vector2.zero;
                                triggeredObj = rb;
                            }
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
                triggered = false;
                _triggerDisable = true;
                if (triggeredObj)
                {
                    triggeredObj = null;
                }
            }
        }
    }

    private void Update()
    {
        if (_triggerDisable)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(playerCheckCenter.position, playerCheckBoxSize, 0f, playerCheckMask);
            Debug.Log(colliders.Length);
            foreach (Collider2D col in colliders)
            {
                Debug.Log(col.gameObject.name);
            }
            if (colliders.Length <= 0)
            {
                _triggerDisable = false;
                onTriggerOff.Invoke();
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (playerCheckCenter)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(playerCheckCenter.position, playerCheckBoxSize);
        }
    }
}
