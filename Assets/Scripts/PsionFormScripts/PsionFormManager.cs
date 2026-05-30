using System;
using UnityEngine;
using System.Collections;

public class PsionFormManager : MonoBehaviour
{
    [Header("Core")]
    public Transform nucleus;

    [Header("Players")]
    public PsionOrbiter player1;
    public PsionOrbiter player2;

    [Header("Orbit Settings")]
    public float angularSpeed = 120f;

    [Header("Transition")]
    public float enterDuration = 0.5f;

    private bool psionFormActivated;
    
    [SerializeField] private PlayerController player1Controller;
    [SerializeField] private PlayerController player2Controller;

    [SerializeField] private PsionFormMovement psionFormMovementComponent;

    private void Awake()
    {
        psionFormMovementComponent = GetComponent<PsionFormMovement>();
        psionFormMovementComponent.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!psionFormActivated)
            {
                EnterPsionForm();
                psionFormMovementComponent.enabled = true;
            }
            else
            {
                ExitPsionForm();
                psionFormMovementComponent.enabled = false;
            }
        }
        
        if (!psionFormActivated) return;

        UpdateOrbit(player1);
        UpdateOrbit(player2);
    }

    void UpdateOrbit(PsionOrbiter psionOrbiter)
    {
        psionOrbiter.currentAngle += angularSpeed * psionOrbiter.direction * Time.deltaTime;

        float radians = psionOrbiter.currentAngle * Mathf.Deg2Rad;

        Vector2 offset = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * psionOrbiter.orbitRadius;

        Vector2 targetPosition = (Vector2)nucleus.position + offset;

        psionOrbiter.rb.MovePosition(targetPosition);
    }
    
    // Entering Psion Form

    public void EnterPsionForm()
    {
        if (psionFormActivated) return;

        StartCoroutine(EnterRoutine());
    }

    IEnumerator EnterRoutine()
    {
        player1Controller.inPsionForm = true;
        player2Controller.inPsionForm = true;
        player1.direction = 0;
        player2.direction = 0;
        psionFormActivated = false;

        PreparePlayer(player1);
        PreparePlayer(player2);

        Vector2 p1Target = (Vector2)nucleus.position + Vector2.left * player1.orbitRadius;

        Vector2 p2Target = (Vector2)nucleus.position + Vector2.right * player2.orbitRadius;

        Vector2 p1Start = player1.rb.position;
        Vector2 p2Start = player2.rb.position;

        float timer = 0f;

        while (timer < enterDuration)
        {
            timer += Time.deltaTime;

            float t = timer / enterDuration;

            player1.rb.MovePosition(Vector2.Lerp(p1Start, p1Target, t));

            player2.rb.MovePosition(Vector2.Lerp(p2Start, p2Target, t));

            yield return null;
        }

        player1.currentAngle = 180f;
        player2.currentAngle = 0f;

        psionFormActivated = true;
    }

    void PreparePlayer(PsionOrbiter member)
    {
        member.rb.linearVelocity = Vector2.zero;
        member.rb.angularVelocity = 0f;

        member.rb.bodyType = RigidbodyType2D.Kinematic;
    }
    
    // Exiting Psion Form

    public void ExitPsionForm()
    {
        player1.direction = 0;
        player2.direction = 0;
        player1Controller.inPsionForm = false;
        player2Controller.inPsionForm = false;
        psionFormActivated = false;

        RestorePlayer(player1);
        RestorePlayer(player2);
    }

    void RestorePlayer(PsionOrbiter member)
    { 
        member.rb.bodyType = RigidbodyType2D.Dynamic;
    }
    
    // Direction Control

    public void SetPlayerDirection(int playerIndex, int direction)
    {
        if (direction > 0) direction = 1;
        
        else if (direction < 0) direction = -1;
        
        else direction = 0;

        if (playerIndex == 0) player1.direction = direction;

        if (playerIndex == 1) player2.direction = direction;
    }
}