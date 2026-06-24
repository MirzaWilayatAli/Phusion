using UnityEngine;

public class TimedButtonTrigger : TriggerBase
{
    [Header("Trigger Settings")]
    public string triggerTag = "";
    public LayerMask mask;
    public float delay = 5f;

    [Header("Visuals")]
    public SpriteRenderer sprite;

    private bool triggered;
    private bool timerRanOver;
    private float timer;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!triggered)
            return;

        timer += Time.deltaTime;

        if (timer >= delay)
        {
            triggered = false;
            timerRanOver = true;

            onTriggerOff.Invoke();

            sprite.color = Color.softRed;
        }
    }

    private void FixedUpdate()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f, mask);

        if (hits.Length > 0)
        {
            if (timerRanOver)
                return;

            if (!triggered)
            {
                foreach (Collider2D col in hits)
                {
                    if (!col.TryGetComponent(out TriggerTag ttag))
                        continue;

                    if (ttag.newTag != triggerTag)
                        continue;

                    Debug.Log("entered");

                    triggered = true;

                    timer = 0f;

                    onTriggerOn.Invoke();

                    if (col.TryGetComponent(out Rigidbody2D rb))
                    {
                        rb.linearVelocity = Vector2.zero;
                    }

                    sprite.color = Color.green;
                    break;
                }
            }
        }
        else
        {
            if (timerRanOver)
            {
                timer = 0f;
                timerRanOver = false;
            }
            else if (triggered)
            {
                Debug.Log("left");

                triggered = false;
                timer = 0f;

                onTriggerOff.Invoke();

                sprite.color = Color.softRed;
            }
        }
    }

}