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

    // the direction the player is looking
    public enum LookDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    public LookDirection currentlookDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentlookDirection = LookDirection.Right;
    }

    private void Update()
    {
        HandleMovementInput();
        HandleJump();
        HandleLookDirection();
        CheckGrounded();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocityY);
    }

    void HandleMovementInput()
    {
        moveInput = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput = -1f;
            currentlookDirection = LookDirection.Left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveInput = 1f;
            currentlookDirection = LookDirection.Right;
        }
    }

    void HandleLookDirection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentlookDirection = LookDirection.Down;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentlookDirection = LookDirection.Up;
        }
    }

    void HandleJump()
    {
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
            Debug.Log("Player is grounded - can jump");
        }
        else
        {
            Debug.Log("Player is NOT grounded");
        }
    }
}
