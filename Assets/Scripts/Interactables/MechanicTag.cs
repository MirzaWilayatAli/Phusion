using System;
using UnityEngine;

public class MechanicTag : MonoBehaviour
{
    public bool charge; // True is positive, etc
    [SerializeField] public Rigidbody2D rb;
    private Collider2D col;
    public bool pos = false;
    public bool neg = false;
    public PhysicsMaterial2D physMat;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        if (rb && col)
        {
            if (!physMat) physMat = rb.sharedMaterial;
            rb.sharedMaterial = null;
            col.sharedMaterial = null;
        }
    }

    public void Release(bool charge)
    {
        rb.sharedMaterial = null;
        col.sharedMaterial = null;

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
        rb.sharedMaterial = physMat;
        col.sharedMaterial = physMat;
        bool current = pos || neg;
        if (rb.linearVelocity.magnitude > 0.1f && !current)
        {
            rb.linearVelocity /= 2;
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
