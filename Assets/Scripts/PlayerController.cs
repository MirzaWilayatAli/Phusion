using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    public DynamicCamera2D camScript;
    public float padding = 0.5f; // prevents clipping

    private Rigidbody2D rb;
    private Vector2 moveInput;
    public bool activateAbility;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    public void Ability(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log($"{name}: hold triggered");
        }

        if (context.canceled)
        {
            Debug.Log($"{name}: hold reset");
        }
    }

    void FixedUpdate()
    {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        // Horizontal movement
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }
    
    void LateUpdate()
    {
        ClampToCamera();
    }

    private void ClampToCamera()
    {
        Vector2 bounds = camScript.GetCameraBounds();

        Vector3 camPos = camScript.transform.position;
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, camPos.x - bounds.x + padding, camPos.x + bounds.x - padding);
        pos.y = Mathf.Clamp(pos.y, camPos.y - bounds.y + padding, camPos.y + bounds.y - padding);

        transform.position = pos;
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}