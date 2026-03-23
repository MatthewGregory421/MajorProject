using UnityEngine;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour
{
    private PlayerMovement movement;

    [Header("Attack Settings")]
    public float attackCooldown = 0.3f;
    private float attackTimer;

    [Header("Debug Attack Prefab")]
    public GameObject attackPrefab; // assign AttackIndicator prefab in inspector
    public float attackDistance = 1f; // how far in front of player it spawns

    [Header("Ground Slam Settings")]
    public int groundSlamCount = 4;         // number of indicators per side
    public float groundSlamSpacing = 0.5f;     // reset when grounded

    [Header("Knockback Settings")]
    public float normalHorizontalForce = 6f;
    public float normalVerticalForce = 2f;

    public float slamHorizontalForce = 10f;
    public float slamVerticalForce = 6f;

    private HashSet<BaseEnemy> enemiesHit = new HashSet<BaseEnemy>();


    // Tracks last horizontal direction (1 = right, -1 = left)
    private int lastHorizontal = 1;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        UpdateLastHorizontal();
        HandleAttack();
    }

    // Update the last horizontal direction from player input
    void UpdateLastHorizontal()
    {
        if (movement.lookHorizontal != 0)
            lastHorizontal = movement.lookHorizontal;
    }

    void HandleAttack()
    {
        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;

        int ver = movement.lookVertical;

        if (!movement.isGrounded && ver < 0 && Input.GetKeyDown(KeyCode.D) && !movement.isGroundSlamming)
        {
            enemiesHit.Clear(); // reset hits for slam

            movement.isGroundSlamming = true;
            movement.rb.linearVelocity = Vector2.zero;
            attackTimer = attackCooldown;
            return;
        }

        if (Input.GetKeyDown(KeyCode.D) && attackTimer <= 0)
        {
            enemiesHit.Clear(); // reset hits for this attack

            PerformAttack();
            attackTimer = attackCooldown;
        }
    }

    void PerformAttack()
    {
        int hor = movement.lookHorizontal;
        int ver = movement.lookVertical;

        string attackDirection = "Neutral";

        if (ver > 0)
            attackDirection = "Up";
        else if (ver < 0)
            attackDirection = "Down";
        else if (hor != 0)
        {
            attackDirection = hor < 0 ? "Left" : "Right";
            lastHorizontal = hor;
        }
        else
        {
            attackDirection = lastHorizontal < 0 ? "Left" : "Right";
        }

        // Direction vector
        Vector2 dir = Vector2.zero;
        if (attackDirection == "Left") dir = Vector2.left;
        else if (attackDirection == "Right") dir = Vector2.right;
        else if (attackDirection == "Up") dir = Vector2.up;
        else if (attackDirection == "Down") dir = Vector2.down;

        Vector3 spawnPos = transform.position + (Vector3)(dir * attackDistance);

        // Spawn debug visual
        GameObject indicator = Instantiate(attackPrefab, spawnPos, Quaternion.identity);
        Destroy(indicator, 0.5f);

        // Damage check
        Collider2D[] hits = Physics2D.OverlapCircleAll(spawnPos, 0.5f);

        foreach (Collider2D hit in hits)
        {
            BaseEnemy enemy = hit.GetComponent<BaseEnemy>();
            if (enemy != null && !enemiesHit.Contains(enemy))
            {
                enemiesHit.Add(enemy);

                Vector2 knockDir = ((Vector2)(enemy.transform.position - transform.position)).normalized;

                enemy.TakeDamage(1, knockDir, normalHorizontalForce, normalVerticalForce);
            }
        }

        Debug.Log("Attack Direction: " + attackDirection);
    }
    void GroundSlam()
    {
        Debug.Log("Ground Slam!");

        // Spawn multiple indicators on left and right
        for (int i = 1; i <= groundSlamCount; i++)
        {
            float offset = i * groundSlamSpacing;

            // Left side
            Vector3 leftPos = transform.position + Vector3.left * offset;
            GameObject leftIndicator = Instantiate(attackPrefab, leftPos, Quaternion.identity);
            Destroy(leftIndicator, 0.5f);

            // Right side
            Vector3 rightPos = transform.position + Vector3.right * offset;
            GameObject rightIndicator = Instantiate(attackPrefab, rightPos, Quaternion.identity);
            Destroy(rightIndicator, 0.5f);
        }
    }

    public void SpawnGroundSlamIndicators()
    {
        Debug.Log("Ground Slam Impact!");

        for (int i = 1; i <= groundSlamCount; i++)
        {
            float offset = i * groundSlamSpacing;

            // LEFT SIDE
            Vector3 leftPos = transform.position + Vector3.left * offset;

            GameObject leftIndicator = Instantiate(attackPrefab, leftPos, Quaternion.identity);
            Destroy(leftIndicator, 0.5f);

            // DAMAGE CHECK (LEFT)
            Collider2D[] leftHits = Physics2D.OverlapCircleAll(leftPos, 0.5f);
            foreach (Collider2D hit in leftHits)
            {
                BaseEnemy enemy = hit.GetComponent<BaseEnemy>();
                if (enemy != null && !enemiesHit.Contains(enemy))
                {
                    enemiesHit.Add(enemy);

                    Vector2 dir = ((Vector2)(enemy.transform.position - transform.position)).normalized;

                    enemy.TakeDamage(1, dir, slamHorizontalForce, slamVerticalForce);
                }
            }

            // RIGHT SIDE
            Vector3 rightPos = transform.position + Vector3.right * offset;

            GameObject rightIndicator = Instantiate(attackPrefab, rightPos, Quaternion.identity);
            Destroy(rightIndicator, 0.5f);

            // DAMAGE CHECK (RIGHT)
            Collider2D[] rightHits = Physics2D.OverlapCircleAll(rightPos, 0.5f);
            foreach (Collider2D hit in rightHits)
            {
                BaseEnemy enemy = hit.GetComponent<BaseEnemy>();
                if (enemy != null && !enemiesHit.Contains(enemy))
                {
                    enemiesHit.Add(enemy);

                    Vector2 dir = ((Vector2)(enemy.transform.position - transform.position)).normalized;

                    enemy.TakeDamage(1, dir, slamHorizontalForce, slamVerticalForce);
                }
            }
        }
    }
}
