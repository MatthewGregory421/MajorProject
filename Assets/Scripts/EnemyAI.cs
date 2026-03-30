using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private BaseEnemy baseEnemy;
    private EnemyAttack attack;

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

    [Header("Vertical Control")]
    public float verticalIgnoreThreshold = 1.0f;

    private enum State { Roam, Chase, Attack }
    private State state;

    [SerializeField] private string debugState;

    void Awake()
    {
        baseEnemy = GetComponent<BaseEnemy>();
        attack = GetComponent<EnemyAttack>();

        if (baseEnemy == null)
            Debug.LogError("EnemyAI: BaseEnemy missing!");

        if (attack == null)
            Debug.LogWarning("EnemyAI: No EnemyAttack attached");
    }

    void Update()
    {
        debugState = state.ToString();

        if (baseEnemy == null || baseEnemy.player == null)
            return;

        float dist = Vector2.Distance(transform.position, baseEnemy.player.position);
        float verticalDiff = baseEnemy.player.position.y - transform.position.y;

        // =========================
        // STATE LOGIC
        // =========================
        float attackRange = attack != null ? attack.GetAttackRange() : preferredDistance;

        if (dist > detectionRange)
            state = State.Roam;
        else if (dist > attackRange)
            state = State.Chase;
        else
            state = State.Attack;

        // =========================
        // EXECUTE STATE
        // =========================
        switch (state)
        {
            case State.Roam:
                Roam(verticalDiff);
                break;

            case State.Chase:
                Chase(verticalDiff);
                break;

            case State.Attack:
                Attack(verticalDiff);
                break;
        }
    }

    // =========================
    // ROAM
    // =========================
    void Roam(float verticalDiff)
    {
        float dir = transform.localScale.x >= 0 ? 1f : -1f;

        if (verticalDiff > verticalIgnoreThreshold)
        {
            baseEnemy.StopHorizontal();
            return;
        }

        Vector2 moveDir = new Vector2(dir, 0);

        RaycastHit2D wallHit = Physics2D.Raycast(
            wallCheck.position,
            moveDir,
            wallCheckDistance,
            obstacleLayer | groundLayer
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
    void Chase(float verticalDiff)
    {
        float dir = (baseEnemy.player.position.x > transform.position.x) ? 1f : -1f;

        FacePlayer(dir);

        if (verticalDiff > verticalIgnoreThreshold)
        {
            baseEnemy.StopHorizontal();
            return;
        }

        // =========================
        // Base movement towards player
        // =========================
        Vector2 velocity = new Vector2(dir * baseEnemy.moveSpeed, baseEnemy.rb.linearVelocity.y);

        // =========================
        // Separation from other enemies
        // =========================
        Collider2D[] nearby = Physics2D.OverlapCircleAll(
            transform.position,
            0.6f, // separation radius, can adjust
            LayerMask.GetMask("Enemy")
        );

        Vector2 separation = Vector2.zero;

        foreach (var other in nearby)
        {
            if (other.gameObject == gameObject) continue;

            Vector2 diff = (Vector2)(transform.position - other.transform.position);
            separation += diff.normalized / diff.magnitude;
        }

        // Apply separation strength (tweak multiplier as needed)
        velocity += separation * 2f;

        // =========================
        // Set final velocity
        // =========================
        baseEnemy.SetVelocity(velocity);
    }

    // =========================
    // ATTACK
    // =========================
    void Attack(float verticalDiff)
    {
        float dir = (baseEnemy.player.position.x > transform.position.x) ? 1f : -1f;

        FacePlayer(dir);

        if (verticalDiff > verticalIgnoreThreshold)
        {
            baseEnemy.StopHorizontal();
            return;
        }

        baseEnemy.StopHorizontal();

        if (attack != null)
            attack.TryAttack();
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

    void FacePlayer(float dir)
    {
        Vector3 scale = transform.localScale;

        if ((dir > 0 && scale.x < 0) || (dir < 0 && scale.x > 0))
        {
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }
}