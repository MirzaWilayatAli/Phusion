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

    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    [Tooltip("true = positive")] public bool activatedAbility;
    private bool _ability;
    [SerializeField] private Vector2 last;
    [SerializeField] private LayerMask mask;
    private bool _isGrounded;
    private MechanicTag _controlledObject;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (camScript == null)
        {
            camScript = FindFirstObjectByType<DynamicCamera2D>();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && _isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
        }
    }

    public void Ability(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _ability = true;
        }

        if (context.canceled)
        {
            _ability = false;
            _controlledObject = null;
        }
    }

    void FixedUpdate()
    {
        // Ground check
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        // Horizontal movement
        if(_moveInput.sqrMagnitude > 0) last = _moveInput.normalized;
        _rb.linearVelocity = new Vector2(_moveInput.x * moveSpeed, _rb.linearVelocity.y);

        if (_ability)
        {
            Debug.Log("Triggered Ability");
            Vector2 temporary = new Vector2(transform.position.x + last.x, transform.position.y + last.y);
            Collider2D x = Physics2D.OverlapCircle(temporary, 2, mask);
            if (x && !_controlledObject)
            {
                Debug.Log("Object found");
                if (x.TryGetComponent(out MechanicTag t))
                {
                    _controlledObject = t;
                }
            }
            if (_controlledObject)
            {
                if (_controlledObject.charge == activatedAbility)
                {
                    _controlledObject.Pull(transform);
                }
                else
                {
                    _controlledObject.Push(transform);
                }
            }
        }
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
        Gizmos.color = Color.red;
        Vector2 temporary = new Vector2(transform.position.x + last.x, transform.position.y + last.y);
        Gizmos.DrawWireSphere(temporary, 2);
    }
}