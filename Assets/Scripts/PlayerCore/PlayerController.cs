using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    public float abilityRadius = 4f;
    
    public DynamicCamera2D camScript;
    public float padding = 0.5f; // prevents clipping

    [SerializeField] private Transform abilityCenter;    
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    [Tooltip("true = positive")] public bool activatedAbility;
    private bool _ability;
    public float maxMagLevDistance = 1f;
    [SerializeField] private Vector2 last;
    [SerializeField] private LayerMask mask;
    private bool _isGrounded;
    private MechanicTag _controlledObject;
    [SerializeField] private FollowTarget followTarget;
    [SerializeField] private GameObject line;
    
    [Header("Annihilation")]
    public AnnihilationProgressBar  AnnihilationProgressBar;
    public AnnihilationChecker  AnnihilationChecker;
    
    [Header("Psion Form")]
    [SerializeField] private int playerID; // assign 0 to Posi and 1 to Eli
    [SerializeField] private PsionFormManager psionFormManager;
    [SerializeField] private PsionFormInitiator psionFormInitiator;

    public Animator animator;
    // SFX Event Calls
    public UnityEvent jumpTrigger;
    public UnityEvent startMagneticAbilitySFX;
    public UnityEvent stopMagneticAbilitySFX;
    public UnityEvent onMove;
    public UnityEvent playDeathSFX;
    void Awake()
    {
        if (!abilityCenter)
        {
            abilityCenter = transform;
        }
        _rb = GetComponent<Rigidbody2D>();
        followTarget.origin = transform;
        if (camScript == null)
        {
            camScript = FindFirstObjectByType<DynamicCamera2D>();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            onMove?.Invoke();
            _moveInput = context.ReadValue<Vector2>();
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && _isGrounded && !psionFormManager.gameObject.activeInHierarchy)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
            jumpTrigger.Invoke();
            if(animator) animator.SetTrigger("Jump");
        }

        if (context.canceled)
        {
            if(animator) animator.ResetTrigger("Jump");
        }
    }

    public void Ability(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _ability = true;
            
            startMagneticAbilitySFX.Invoke();
            
            if(animator) animator.SetBool("Ability", true);
        }

        if (context.canceled)
        {
            if(animator) animator.SetBool("Ability", false);
            
            _ability = false;
            
            stopMagneticAbilitySFX.Invoke();
            
            if (_controlledObject)
            {
                bool current = _controlledObject.neg || _controlledObject.pos;
                if(current)
                    _controlledObject.rb.linearVelocity = Vector2.zero;
                
                _controlledObject.Release(activatedAbility);
                _controlledObject = null;
            }

            followTarget.target = null;
            line.SetActive(false);
        }
    }

    public void PsionForm(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (AnnihilationProgressBar != null && AnnihilationProgressBar.gameObject.activeInHierarchy)
        {
            AnnihilationProgressBar.ActivatePsionForm();
            return;
        }

        if (psionFormManager != null && psionFormManager.gameObject.activeInHierarchy)
        {
            psionFormInitiator.TerminateQuantumHandshake(playerID);
            return;
        }

        Debug.Log("Please move closer to begin Annihilation");
    }

    void FixedUpdate()
    {
        // Ground check
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        
        // Horizontal movement
        if(_moveInput.sqrMagnitude > 0) last = _moveInput.normalized;

        if (!psionFormManager.gameObject.activeInHierarchy)
        {
            if(animator) animator.SetBool("Psion", false);
            HandleNormalMovement();
            if(animator) animator.SetFloat("Movement", _moveInput.x);
        }
        else
        {
            if(animator) animator.SetBool("Psion", true);
            HandlePsionInput();
        }
        
        if (_ability)
        {
            Vector2 temporary = new Vector2(abilityCenter.position.x + last.x, abilityCenter.position.y + last.y);
            Collider2D x = Physics2D.OverlapCircle(temporary, abilityRadius, mask);
            if (x && !_controlledObject)
            {
                if (x.TryGetComponent(out MechanicTag t))
                {
                    _controlledObject = t;
                    followTarget.target = _controlledObject.transform;
                    _controlledObject.Prep(this, activatedAbility);
                    line.SetActive(true);
                }
                if (x.TryGetComponent(out MagLevPlate plate))
                {
                    if (plate.charge == activatedAbility)
                    {
                        if(Vector3.Distance(transform.position, plate.transform.position) <= maxMagLevDistance)
                            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, plate.force);
                    }
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

        float minX = camPos.x - bounds.x + padding;
        float maxX = camPos.x + bounds.x - padding;
        float minY = camPos.y - bounds.y + padding;
        float maxY = camPos.y + bounds.y - padding;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }

    private void HandleNormalMovement()
    {
        _rb.linearVelocity = new Vector2(_moveInput.x * moveSpeed, _rb.linearVelocity.y);
    }

    private void HandlePsionInput()
    {
        if (_moveInput.x > 0.1f)
        {
            psionFormManager.SetPlayerDirection(playerID, 1);
        }
        else if (_moveInput.x < -0.1f)
        {
            psionFormManager.SetPlayerDirection(playerID, -1);
        }
        else
        {
            psionFormManager.SetPlayerDirection(playerID, 0);
        }
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        Gizmos.color = Color.red;
        Vector2 temporary = new Vector2(transform.position.x + last.x, transform.position.y + last.y);

        if (abilityCenter)
        {
            temporary = new Vector2(abilityCenter.position.x + last.x, abilityCenter.position.y + last.y);
        }
        Gizmos.DrawWireSphere(temporary, abilityRadius);
    }
    
    
}