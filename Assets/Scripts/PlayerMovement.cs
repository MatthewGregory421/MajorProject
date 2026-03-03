using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Double Jump Settings")]
    public bool doubleJumpEnabled = true; // toggle in inspector
    private bool canDoubleJump;
    public bool canJump = true;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;

    [Header("Crouch Settings")]
    public bool isCrouching;
    public float crouchSpeedMultiplyer = 0.6f;

    [Header("Dash Settings")]
    public bool dashEnabled = true;
    public float dashForce = 18f;
    [Tooltip("How long the player remains in the dash state.")]
    public float dashDuration;
    public float dashCooldown = 0.8f;

    private bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;
    private bool hasAirDashed;
    private float dashDirection;

    [Header("Combat State")]
    public bool isInvulnerable;

    // the direction the player is looking
    public enum LookDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    public LookDirection currentLookDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentLookDirection = LookDirection.Right;
    }

    private void Update()
    {
        HandleMovementInput();
        HandleJump();
        HandleLookDirection();
        CheckGrounded();
        HandleDash();

        Debug.Log("Crouching: " + isCrouching);
    }

    private void FixedUpdate()
    {
        if (isDashing)
            return;

        float speed = moveSpeed;

        if (isCrouching)
            speed *= crouchSpeedMultiplyer;

        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }

    void HandleMovementInput()
    {
        moveInput = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput = -1f;
            currentLookDirection = LookDirection.Left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveInput = 1f;
            currentLookDirection = LookDirection.Right;
        }
    }

    void HandleLookDirection()
    {
        // Crouch
        isCrouching = Input.GetKey(KeyCode.DownArrow);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            currentLookDirection = LookDirection.Up;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            currentLookDirection = LookDirection.Left;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            currentLookDirection = LookDirection.Right;
    }

    void HandleJump()
    {
        if (isCrouching)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                canDoubleJump = true;
            }
            else if (doubleJumpEnabled && canDoubleJump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                canDoubleJump = false;
            }
        }
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (isGrounded)
        {
            canDoubleJump = true;
            hasAirDashed = false;
            Debug.Log("Player is grounded");
        }
        else
        {
            Debug.Log("Player is NOT grounded");
        }
    }

    void HandleDash()
    {
        if (!dashEnabled || isCrouching)
            return;

        if (dashCooldown > 0)
            dashCooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.A) && dashCooldownTimer <= 0 && !isDashing)
        {
            if (isGrounded)
            {
                StartDash();
            }
            else if (!hasAirDashed)
            {
                hasAirDashed = true;
                StartDash();
            }
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;

            rb.linearVelocity = new Vector2(dashDirection * dashForce, 0f);

            if (dashTimer <= 0)
                EndDash();

        }
    }

    void StartDash()
    {
        isDashing = true;
        isInvulnerable = true;

        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;

        if (currentLookDirection == LookDirection.Left)
            dashDirection = -1f;
        else
            dashDirection = 1f;

        rb.gravityScale = 0f; // optional freeze gravity
    }

    void EndDash()
    {
        isDashing = false;
        isInvulnerable = false;

        rb.gravityScale = 1f; // set back to normal gravity
    }
}
