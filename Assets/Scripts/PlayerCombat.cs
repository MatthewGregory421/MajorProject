using UnityEngine;

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
            movement.isGroundSlamming = true;
            movement.rb.linearVelocity = Vector2.zero; // reset momentum
            attackTimer = attackCooldown;
            return;
        }

        if (Input.GetKeyDown(KeyCode.D) && attackTimer <= 0)
        {
            PerformAttack();
            attackTimer = attackCooldown;
        }
    }

    void PerformAttack()
    {
        int hor = movement.lookHorizontal;
        int ver = movement.lookVertical;

        // Check for ground slam
        if (!movement.isGrounded && ver < 0 && movement.canGroundSlam && Input.GetKey(KeyCode.D))
        {
            GroundSlam();
            movement.canGroundSlam = false;
            return;
        }

        string attackDirection = "Neutral";

        // Up/Down has priority
        if (ver > 0)
            attackDirection = "Up";
        else if (ver < 0)
            attackDirection = "Down";
        else if (hor != 0)
        {
            attackDirection = hor < 0 ? "Left" : "Right";
            lastHorizontal = hor; // remember last horizontal for idle attacks
        }
        else
        {
            // No input, fall back to last horizontal
            attackDirection = lastHorizontal < 0 ? "Left" : "Right";
        }

        // Spawn indicator
        Vector2 spawnDirection = Vector2.zero;
        if (attackDirection == "Left") spawnDirection = Vector2.left;
        else if (attackDirection == "Right") spawnDirection = Vector2.right;
        else if (attackDirection == "Up") spawnDirection = Vector2.up;
        else if (attackDirection == "Down") spawnDirection = Vector2.down;

        Vector3 spawnPos = transform.position + (Vector3)(spawnDirection * attackDistance);
        GameObject indicator = Instantiate(attackPrefab, spawnPos, Quaternion.identity);
        Destroy(indicator, 0.5f);

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
}
