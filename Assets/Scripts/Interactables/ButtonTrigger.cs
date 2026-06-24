using System;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonTrigger : TriggerBase
{
    public string triggerTag = "";
    public LayerMask mask;
    public bool triggered = false;
    
    // for visuals so players know they have successfully activated a button
    public SpriteRenderer sprite;
    [SerializeField] private Sprite untriggeredButtonSprite;
    [SerializeField] private Sprite triggeredButtonSprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

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
                    Debug.Log("button triggered");
                    if (col.TryGetComponent(out TriggerTag ttag))
                    {
                        if (ttag.newTag == triggerTag)
                        {
                            triggered = true;
                            onTriggerOn.Invoke();
                            if (col.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                            {
                                rb.linearVelocity = Vector2.zero;
                            }
                            sprite.sprite = triggeredButtonSprite;
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
                Debug.Log("button untriggered");
                triggered = false;
                onTriggerOff.Invoke();
                sprite.sprite = untriggeredButtonSprite;
            }
        }
    }
}
