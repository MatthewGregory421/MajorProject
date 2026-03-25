using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public BaseEnemy baseEnemy;

    [Header("Detection")]
    public float detectionRange = 6f;
    public float preferredDistance = 1.5f;

    [Header("Roaming")]
    public float roamSpeed = 1f;

    [Header("Wall / Edge Checks")]
    public Transform wallCheck;
    public float wallCheckDistance = 0.5f;

    public Transform groundCheck;
    public float groundCheckDistance = 0.3f;
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;

    [Header("Anti-Jitter Vertical Control")]
    public float verticalIgnoreThreshold = 1.0f;

    [Header("Crowd Control")]
    public float separationRadius = 0.6f;
    public float separationStrength = 2f;
    public LayerMask enemyLayer;

    private enum State { Roam, Chase }
    private State state;

    void Awake()
    {
        baseEnemy = GetComponent<BaseEnemy>();

        if (baseEnemy == null)
            Debug.LogError("EnemyAI: BaseEnemy missing!");
    }

    void Update()
    {
        if (baseEnemy == null || baseEnemy.player == null)
            return;

        float distToPlayer = Vector2.Distance(transform.position, baseEnemy.player.position);

        float verticalDiff = baseEnemy.player.position.y - transform.position.y;

        // =========================
        // STATE
        // =========================
        state = (distToPlayer <= detectionRange) ? State.Chase : State.Roam;

        switch (state)
        {
            case State.Roam:
                Roam(verticalDiff);
                break;

            case State.Chase:
                Chase(distToPlayer, verticalDiff);
                break;
        }
    }

    // =========================
    // ROAM (SMART VERSION)
    // =========================
    void Roam(float verticalDiff)
    {
        float dir = transform.localScale.x >= 0 ? 1f : -1f;

        // KEY FIX: if player is above, freeze horizontal AI influence
        if (verticalDiff > verticalIgnoreThreshold)
        {
            baseEnemy.SetVelocity(new Vector2(0f, baseEnemy.rb.linearVelocity.y));
            return;
        }

        Vector2 moveDir = new Vector2(dir, 0);

        RaycastHit2D wallHit = Physics2D.Raycast(
            wallCheck.position,
            moveDir,
            wallCheckDistance,
            obstacleLayer
        );

        RaycastHit2D groundHit = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        if (wallHit || !groundHit)
        {
            FlipDirection();
            dir *= -1f;
        }

        baseEnemy.SetVelocity(new Vector2(dir * roamSpeed, baseEnemy.rb.linearVelocity.y));
    }

    // =========================
    // CHASE
    // =========================
    void Chase(float dist, float verticalDiff)
    {
        float dir = (baseEnemy.player.position.x > transform.position.x) ? 1f : -1f;

        // KEY FIX: prevent jitter when player is above
        if (verticalDiff > verticalIgnoreThreshold)
        {
            baseEnemy.SetVelocity(new Vector2(0f, baseEnemy.rb.linearVelocity.y));
            return;
        }

        Vector2 vel = baseEnemy.rb.linearVelocity;

        if (dist > preferredDistance)
        {
            vel = new Vector2(dir * baseEnemy.moveSpeed, vel.y);
        }
        else
        {
            vel = new Vector2(0f, vel.y);
        }

        baseEnemy.SetVelocity(vel);
    }

    // =========================
    // FLIP
    // =========================
    void FlipDirection()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }
}