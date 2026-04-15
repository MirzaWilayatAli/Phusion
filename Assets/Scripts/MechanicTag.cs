using System;
using UnityEngine;

public class MechanicTag : MonoBehaviour
{
    public bool charge; // True is positive, etc
    [SerializeField] private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Prep()
    {
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    public void Push(Transform t)
    {
        Vector3 temp = t.position - transform.position;
        Vector2 temp2 = new Vector2(temp.x, temp.y).normalized;
        rb.AddForce(temp2 * rb.mass);
    }
    public void Pull(Transform t)
    {
        Vector3 temp = t.position - transform.position;
        Vector2 temp2 = new Vector2(temp.x, temp.y).normalized;
        rb.AddForce(-temp2 * rb.mass);
    }
}
