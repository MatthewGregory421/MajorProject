using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public BaseEnemy baseEnemy;

    [Header("Attack Settings")]
    public float attackRange = 1.3f;
    public float attackCooldown = 2f;

    public float windupTime = 0.5f;
    public float recoveryTime = 0.4f;

    private float attackTimer;

    [Header("Combo")]
    public bool doCombo = true;

    private void Awake()
    {
        baseEnemy = GetComponent<BaseEnemy>();
    }

    void Update()
    {
        if (baseEnemy.player == null) return;

        attackTimer -= Time.deltaTime;

        float dist = Vector2.Distance(transform.position, baseEnemy.player.position);

        if (dist <= attackRange && attackTimer <= 0f)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        attackTimer = attackCooldown;

        if (doCombo)
        {
            yield return StartCoroutine(SingleHit());
            yield return new WaitForSeconds(0.2f);
            yield return StartCoroutine(SingleHit());
        }
        else
        {
            yield return StartCoroutine(SingleHit());
        }
    }

    IEnumerator SingleHit()
    {
        // windup
        yield return new WaitForSeconds(windupTime);

        // DAMAGE (simple forward hitbox)
        Vector2 pos = transform.position;
        Collider2D hit = Physics2D.OverlapCircle(pos, attackRange);

        if (hit != null && hit.CompareTag("Player"))
        {
            PlayerHealth ph = hit.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage(baseEnemy.damage);
        }

        yield return new WaitForSeconds(recoveryTime);
    }
}
