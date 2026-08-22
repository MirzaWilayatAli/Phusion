using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Detection Zone")]
    [SerializeField] private BoxCollider2D detectionZone;
    [SerializeField] private LayerMask playerLayer;

    [Header("Nudge Settings")]
    [SerializeField] private float pushSpeed = 8f; // speed to shove the player out
    [SerializeField] private bool horizontalDoor = true; // Set True for horizontal doors, false for vertical doors

    [Header("Visuals & Physical Door")]
    [SerializeField] private GameObject doorVisuals;

    private Coroutine enableRoutine;

    public void EnableDoor()
    {
        if (enableRoutine != null) StopCoroutine(enableRoutine);
        enableRoutine = StartCoroutine(NudgePlayerAndEnable());
    }

    private IEnumerator NudgePlayerAndEnable()
    {
        Vector2 center = (Vector2)transform.TransformPoint(detectionZone.offset);
        Vector2 size = Vector2.Scale(detectionZone.size, transform.lossyScale);

        Collider2D hitPlayer = Physics2D.OverlapBox(center, size, transform.eulerAngles.z, playerLayer);

        while (hitPlayer != null)
        {
            Rigidbody2D playerRb = hitPlayer.attachedRigidbody;

            if (playerRb != null)
            {
                // Determine direction away from door center
                Vector2 diff = playerRb.position - center;
                Vector2 pushDir;

                if (horizontalDoor)
                {
                    // Nudge Up or Down for horizontal doors
                    pushDir = (diff.y >= 0 ? Vector2.up : Vector2.down);
                }
                else
                {
                    // Nudge Left or Right depending on which side player is closer to
                    pushDir = (diff.x >= 0 ? Vector2.right : Vector2.left);
                }

                // Smoothly nudge player position out of the area
                playerRb.position += pushDir * pushSpeed * Time.deltaTime;
            }

            yield return null; // Wait 1 frame and just double check if everything is clear

            hitPlayer = Physics2D.OverlapBox(center, size, transform.eulerAngles.z, playerLayer);
        }

        // Area should be guaranteed clear now, safe to activate door
        doorVisuals.SetActive(true);
    }

    public void DisableDoor()
    {
        if (enableRoutine != null) StopCoroutine(enableRoutine);
        doorVisuals.SetActive(false);
    }
    
    private void OnDrawGizmos()
    {
        if (detectionZone == null)
            return;

        Gizmos.color = Color.darkCyan;

        Vector2 center = (Vector2)transform.TransformPoint(detectionZone.offset);
        Vector2 size = Vector2.Scale(detectionZone.size, transform.lossyScale);

        Matrix4x4 rotationMatrix = Matrix4x4.TRS(
            center,
            Quaternion.Euler(0f, 0f, transform.eulerAngles.z),
            Vector3.one
        );

        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, size);

        Gizmos.matrix = Matrix4x4.identity;
    }
}