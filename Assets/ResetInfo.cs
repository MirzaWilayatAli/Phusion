using UnityEngine;

public class ResetInfo : MonoBehaviour
{
    public Vector3 initialPosition;
    public Quaternion initialRotation;
    public Vector3 initialScale;
    public Rigidbody2D rb;
    
    public float resetY = -30f;
    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
        
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {  
        if(transform.position.y <= resetY)
        {
            Debug.Log("triggered reset?");
            TriggerReset();
        }
    }

    public void TriggerReset()
    {
        rb.linearVelocity = Vector3.zero;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        transform.localScale = initialScale;
        rb.linearVelocity = Vector3.zero;
    }
}
