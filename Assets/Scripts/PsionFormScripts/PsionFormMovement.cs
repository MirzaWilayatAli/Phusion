using UnityEngine;

public class PsionFormMovement : MonoBehaviour
{
    [Header("Core")]
    public Transform nucleus;

    [Header("Orbiters")]
    public Transform eli;
    public Transform posi;

    [Header("Movement")]
    public float thrustForce = 5f;
    public float acceleration = 5f;
    public float drag = 1f;

    private Vector3 velocity;

    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        Vector3 netForce = Vector3.zero;

        // Eli 

        Vector3 eliDirection = (eli.position - nucleus.position).normalized;

        netForce += eliDirection * thrustForce;

        
        // Posi Input

        Vector3 posiDirection =
            (posi.position - nucleus.position).normalized;

        netForce += posiDirection * thrustForce;

        
        // Move Core
       
        velocity += netForce * acceleration * Time.deltaTime;

        velocity *= 1f / (1f + drag * Time.deltaTime);

        nucleus.position += velocity * Time.deltaTime;
    }
}
