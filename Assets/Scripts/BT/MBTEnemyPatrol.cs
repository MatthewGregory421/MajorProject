using UnityEngine;
using MBT;

[MBTNode("Custom/MBT Enemy Patrol 2D")]
[AddComponentMenu("")]
public class MBTEnemyPatrol : Leaf
{
    public Rigidbody2D enemyRb;
    public float speed = 2f;
    public float wallCheckDistance = 0.5f;
    public float groundCheckDistance = 1f;
    public LayerMask groundLayer;

    private int direction = 1;

    public override NodeResult Execute()
    {
        Rigidbody2D rb = enemyRb;
        if (rb == null)
            return NodeResult.failure;

        Vector2 pos = rb.position;

        // Wall detection
        RaycastHit2D wallHit = Physics2D.Raycast(
            pos,
            Vector2.right * direction,
            wallCheckDistance,
            groundLayer
        );

        // Ground detection
        Vector2 groundCheckPos = pos + new Vector2(direction * 0.5f, 0);
        RaycastHit2D groundHit = Physics2D.Raycast(
            groundCheckPos,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        // Flip if wall or edge
        if (wallHit.collider != null || groundHit.collider == null)
        {
            direction *= -1;
            Vector3 scale = rb.transform.localScale;
            scale.x *= -1;
            rb.transform.localScale = scale;
        }

        // Move using linearVelocity (2D physics friendly)
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

        return NodeResult.running;
    }
}