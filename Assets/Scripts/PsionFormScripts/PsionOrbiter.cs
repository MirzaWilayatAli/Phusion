using System;
using Unity.VisualScripting;
using UnityEngine;

public class PsionOrbiter : MonoBehaviour
{
    public Rigidbody2D rb;
    public float currentAngle;
    public float orbitRadius = 3f;
    public int direction = 1;
    public PlayerController mainController;

    private void Awake()
    {
        mainController = GetComponent<PlayerController>();
    }
}
