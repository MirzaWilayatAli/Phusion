using System;
using UnityEngine;

public class MechanicTag : MonoBehaviour
{
    public bool charge; // True is positive, etc
    [SerializeField] public Rigidbody2D rb;
    public bool pos = false;
    public bool neg = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Release(bool charge)
    {
        if (charge)
        {
            pos = false;
        }
        if (charge == false)
        {
            neg = false;
        }
    }
    public void Prep(PlayerController sender, bool charge)
    {
        bool current = pos || neg;
        if (rb.linearVelocity.magnitude > 0.1f && !current)
        {
            rb.linearVelocity = Vector2.zero;
        }
        if (charge)
        {
            pos = true;
        }
        if (charge == false)
        {
            neg = true;
        }
    }
    public void Pull(Transform t)
    {
        Vector3 temp = t.position - transform.position;
        Vector2 temp2 = new Vector2(temp.x, temp.y).normalized;
        float dist = 3.75f * Vector3.Distance(t.position, transform.position);
        rb.AddForce(temp2 * (rb.mass * dist), ForceMode2D.Force);
    }
    public void Push(Transform t)
    {
        Vector3 temp = t.position - transform.position;
        Vector2 temp2 = new Vector2(temp.x, temp.y).normalized;
        float dist = 3.75f / Vector3.Distance(t.position, transform.position);
        rb.AddForce(-temp2 * (rb.mass * dist), ForceMode2D.Force);
    }
}
