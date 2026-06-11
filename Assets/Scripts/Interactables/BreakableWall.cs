using UnityEngine;
using Random = UnityEngine.Random;

public class BreakableWall : MonoBehaviour
{
    public GameObject unbrokenWall;
    public GameObject brokenWall;
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        unbrokenWall.SetActive(false);
        brokenWall.SetActive(true);
        
        Vector2 hitDirection = collision.contacts[0].normal;
        
        Rigidbody2D[] pieces = brokenWall.GetComponentsInChildren<Rigidbody2D>();

        foreach (Rigidbody2D rb in pieces)
        {
            Vector2 dir = hitDirection + Random.insideUnitCircle * 0.3f;
            dir.Normalize();

            rb.AddForce(dir * Random.Range(2f, 5f), ForceMode2D.Impulse);

            rb.AddTorque(Random.Range(-2f, 2f), ForceMode2D.Impulse);
        }
    }
}
