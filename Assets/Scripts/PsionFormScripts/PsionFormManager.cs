using System;
using UnityEngine;
using System.Collections;

public class PsionFormManager : MonoBehaviour
{
    public Vector3 activationLocation; 
    [Header("Core")]
    public Transform nucleus;

    [Header("Psion Orbiter References")]
    public PsionOrbiter player1;
    public PsionOrbiter player2;

    [Header("Orbit Settings")]
    public float angularSpeed = 120f;

    [Header("Transition")]
    public float enterDuration = 0.5f;
    
    public GameObject AnnihilationChecker;
    public bool psionFormInitiated = false;
    

    private void Awake()
    {
        AnnihilationChecker =  GameObject.Find("AnnihilationChecker");
    }

    private void OnEnable()
    {
        transform.position = activationLocation;
    }

    private void OnDisable()
    {
        ExitPsionForm();
    }

    private void Update()
    {
        if (!psionFormInitiated)
        {
            return;
        }
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
        psionFormInitiated = true;
        AnnihilationChecker.SetActive(false);
        
        StartCoroutine(EnterRoutine());
    }

    IEnumerator EnterRoutine()
    {
        player1.direction = 0;
        player2.direction = 0;

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
        psionFormInitiated = false;
        AnnihilationChecker.SetActive(true);
        
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