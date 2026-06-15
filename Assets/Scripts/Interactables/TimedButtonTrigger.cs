using System;
using Unity.VisualScripting;
using UnityEngine;

public class TimedButtonTrigger : TriggerBase
{
    public string triggerTag = "";
    public LayerMask mask;
    public bool triggered = false;
    public bool timerRanOver = false;
    public float delay = 5f;
    [SerializeField] private float timer;
    // for visuals so players know they have successfully activated a button
    public SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (triggered)
        {
            timer += Time.deltaTime;
            if (timer >= delay)
            {
                triggered = false;
                timerRanOver = true;
                onTriggerOff.Invoke();
                sprite.color = Color.softRed;
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 x = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] _ = Physics2D.OverlapCircleAll(x, 0.5f, mask);
        if (_.Length > 0)
        {
            if (timerRanOver) return;
            if (!triggered)
            {
                foreach (Collider2D col in _)
                {
                    Debug.Log("entered");
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
                            sprite.color = Color.green;
                        }
                        break;
                    }
                }                
            }
        }
        else
        {
            if (timerRanOver)
            {
                timer = 0f;
                timerRanOver = false;
            } else if (triggered)
            {
                Debug.Log("left");
                triggered = false;
                onTriggerOff.Invoke();
                sprite.color = Color.softRed;
            }
        }
    }
}