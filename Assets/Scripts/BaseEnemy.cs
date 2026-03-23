using UnityEngine;
using System.Collections;

public class BaseEnemy : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 3;
    private int currentHealth;

    public float moveSpeed = 2f;
    public int damage = 1;

    [Header("Detection")]
    public float detectionRange = 6f;
    public float attackRange = 1.2f;

    [Header("Attack")]
    public float attackCooldown = 1.5f;
    private float attackTimer;
    private bool isAttacking = false;
    private Transform attackTarget;
    public float attackRecoveryTime = 0.4f;

    [Header("Attack Hitbox")]
    public Vector2 attackBoxSize = new Vector2(1.2f, 0.8f);
    public Vector2 attackOffset = new Vector2(0.8f, 0f);
    public LayerMask playerLayer;

    [Header("Edge / Wall Checks")]
    public Transform groundCheck;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;

    public Transform wallCheck;
    public float wallCheckDistance = 0.5f;

    private float flipCooldown = 0.2f;
    private float flipTimer = 0f;

    [Header("Knockback")]
    public float knockbackDuration = 0.2f;
    private float knockbackTimer;

    [Header("References")]
    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Color originalColor;

    private int moveDirection = 1; // 1 right, -1 left
    private bool isAggro = false;

    [Header("Attack Telegraph")]
    public float attackWindUpTime = 0.4f;
    private bool attackTelegraphActive = false;
    public Color windUpColor = Color.yellow;
    public Color attackColor = Color.red;

    [Header("Return To Patrol")]
    public float returnToPatrolDelay = 1.2f;
    private float returnTimer;
    private bool waitingToReturn = false;

    private enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Stunned
    }

    private EnemyState state;

    void Start()
    {
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            originalColor = sr.color;

        state = EnemyState.Patrol;
    }

    void Update()
    {
        if (isAttacking)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // LOCK POSITION

            if (attackTimer > 0)
                attackTimer -= Time.deltaTime;

            return;
        }

        if (player == null) return;

        if (flipTimer > 0)
            flipTimer -= Time.deltaTime;

        float distance = Vector2.Distance(transform.position, player.position);

        // ================= STUN OVERRIDE =================
        if (state == EnemyState.Stunned)
        {
            if (knockbackTimer > 0)
            {
                knockbackTimer -= Time.deltaTime;
                RunState(state, distance);
                return;
            }

            state = EnemyState.Patrol;
        }

        // ================= DETECTION =================
        if (distance <= detectionRange)
        {
            state = EnemyState.Chase;
            waitingToReturn = false;
        }
        else
        {
            if (state != EnemyState.Patrol && !waitingToReturn)
            {
                waitingToReturn = true;
                returnTimer = returnToPatrolDelay;
            }
        }

        // ================= ATTACK TRANSITION =================
        if (state == EnemyState.Chase && distance <= attackRange)
        {
            state = EnemyState.Attack;
        }

        // ================= RETURN DELAY TIMER =================
        if (waitingToReturn)
        {
            returnTimer -= Time.deltaTime;

            if (returnTimer <= 0)
            {
                waitingToReturn = false;
                state = EnemyState.Patrol;
            }
        }

        // ================= RUN CURRENT STATE =================
        UpdateFacing();
        RunState(state, distance);

        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;

        UpdateChecks();
    }

    void RunState(EnemyState state, float distance)
    {
        switch (state)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Chase:
                ChasePlayer();
                break;

            case EnemyState.Attack:
                TryAttack(distance);
                break;

            case EnemyState.Stunned:
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                break;

            case EnemyState.Idle:
                rb.linearVelocity = Vector2.zero;
                break;
        }

        CheckEdge();
        CheckWall();
    }

    // ================= MOVEMENT =================

    void UpdateFacing()
    {
        if (state == EnemyState.Chase)
        {
            moveDirection = player.position.x > transform.position.x ? 1 : -1;
        }
    }

    void ChasePlayer()
    {
        rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);
    }

    void Patrol()
    {
        rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);
    }

    // ================= ATTACK =================

    void TryAttack(float distance)
    {
        if (isAttacking) return;
        if (attackTimer > 0) return;

        if (distance <= attackRange)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        attackTimer = attackCooldown;
        attackTarget = player;

        if (sr != null)
            sr.color = windUpColor;

        // WIND-UP
        yield return new WaitForSeconds(attackWindUpTime);

        if (sr != null)
            sr.color = attackColor;

        float facingDir = moveDirection;

        Vector2 boxPos = (Vector2)transform.position + new Vector2(attackOffset.x * facingDir, attackOffset.y);

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            boxPos,
            attackBoxSize,
            0f,
            playerLayer
        );

        foreach (Collider2D hit in hits)
        {
            Debug.Log("Enemy hit: " + hit.name);

            PlayerHealth ph = hit.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
            }
        }

        yield return new WaitForSeconds(attackRecoveryTime);

        if (sr != null)
            sr.color = originalColor;

        isAttacking = false;
    }

    // ================= EDGE / WALL =================

    void CheckEdge()
    {
        if (groundCheck == null) return;

        Vector2 origin = (Vector2)groundCheck.position;

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        if (!hit)
        {
            FlipDirection();
        }
    }

    void CheckWall()
    {
        if (wallCheck == null) return;

        Vector2 direction = moveDirection == 1 ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(
            wallCheck.position,
            direction,
            wallCheckDistance,
            groundLayer
        );

        if (hit)
        {
            FlipDirection();
        }
    }

    void UpdateChecks()
    {
        if (groundCheck != null)
        {
            Vector3 pos = groundCheck.localPosition;
            pos.x = Mathf.Abs(pos.x) * moveDirection;
            groundCheck.localPosition = pos;
        }

        if (wallCheck != null)
        {
            Vector3 pos = wallCheck.localPosition;
            pos.x = Mathf.Abs(pos.x) * moveDirection;
            wallCheck.localPosition = pos;
        }
    }

    void FlipDirection()
    {
        if (flipTimer > 0) return;

        moveDirection *= -1;
        flipTimer = flipCooldown;
    }

    // ================= DAMAGE =================

    public void TakeDamage(int damage, Vector2 knockbackDir, float horForce, float vertForce)
    {
        currentHealth -= damage;

        StartCoroutine(FlashWhite());

        isAggro = true;
        FacePlayer();

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(knockbackDir.x * horForce, vertForce), ForceMode2D.Impulse);

        knockbackTimer = knockbackDuration;

        if (currentHealth <= 0)
            Die();
    }

    void FacePlayer()
    {
        if (player == null) return;

        moveDirection = player.position.x > transform.position.x ? 1 : -1;
    }

    // ================= VISUAL =================

    System.Collections.IEnumerator FlashWhite()
    {
        if (sr == null) yield break;

        sr.color = Color.white;
        yield return new WaitForSeconds(0.08f);
        sr.color = originalColor;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        float facingDir = moveDirection;

        Vector2 boxPos = (Vector2)transform.position + new Vector2(attackOffset.x * facingDir, attackOffset.y);

        Gizmos.DrawWireCube(boxPos, attackBoxSize);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}