using UnityEngine;

public class PsionFormMovement : MonoBehaviour
{
    [Header("References")]
    public Transform nucleus;
    public Transform eli;
    public Transform posi;

    private Vector2 velocity;

    public float thrustForce = 1f;
    public float acceleration = 1f;
    public float drag = 1f;

    private void FixedUpdate()
    {
        Vector2 netForce = Vector2.zero;

        // Eli
        Vector2 eliDirection = ((Vector2)eli.position - (Vector2)nucleus.position).normalized;
        netForce += eliDirection * thrustForce;

        // Posi
        Vector2 posiDirection = ((Vector2)posi.position - (Vector2)nucleus.position).normalized;
        netForce += posiDirection * thrustForce;

        // Movement
        velocity += netForce * acceleration * Time.fixedDeltaTime;
        velocity *= 1f / (1f + drag * Time.fixedDeltaTime);
        nucleus.position += (Vector3)(velocity * Time.fixedDeltaTime);
    }
}