using UnityEngine;

public class RandomBehaviorAttack : EnemyAttack
{
    [Header("Behaviour Settings")]
    public float decisionCooldown = 1.5f;
    private float nextDecisionTime;

    private enum ActionState { Idle, BackingUp, Charging, IdleMove, Attacking }
    private ActionState currentState = ActionState.Idle;

    [Header("Movement")]
    public float backAwaySpeed = 2f;
    public float chargeSpeed = 4f;
    public float maxChargeDistance = 3f;

    [Header("Action Cooldowns")]
    public float chargeCooldown = 3f;
    public float backAwayCooldown = 2f;
    public float normalAttackCooldown = 1f;

    private float lastChargeTime;
    private float lastBackAwayTime;
    private bool hasAttackedDuringCharge;
    private float actionDuration;

    private float backAwayDirection;
    private bool backAwayInitialized = false;

    [Header("Collision / Attack Settings")]
    public int collisionDamage = 1;

    private Vector2 startPosition;
    private Vector2 chargeDirection;

    [Header("Safety")]
    public float verticalIgnoreThreshold = 1.0f;

    // =========================
    // MAIN UPDATE
    // =========================
    public void HandleState()
    {
        if (baseEnemy == null || baseEnemy.player == null)
            return;

        float verticalDiff = Mathf.Abs(baseEnemy.player.position.y - transform.position.y);
        if (verticalDiff > verticalIgnoreThreshold)
        {
            baseEnemy.SetVelocity(Vector2.zero);
            currentState = ActionState.Idle;
            Debug.Log("Vertical diff too high, staying Idle");
            return;
        }

        // Tick down action timer
        if (actionDuration > 0f)
        {
            actionDuration -= Time.deltaTime;
            if (actionDuration <= 0f && currentState != ActionState.Charging)
            {
                currentState = ActionState.Idle;
                backAwayInitialized = false;
            }
        }

        switch (currentState)
        {
            case ActionState.Idle:
                Debug.Log("State: Idle");
                HandleDecision();
                break;
            case ActionState.BackingUp:
                if (!backAwayInitialized)
                {
                    backAwayDirection = Mathf.Sign(transform.position.x - baseEnemy.player.position.x);
                    backAwayInitialized = true;
                }
                BackAway();
                break;
            case ActionState.Charging:
                Debug.Log("State: Charging");
                ChargeForward();
                break;
            case ActionState.IdleMove:
                Debug.Log("State: IdleMove");
                IdleMove();
                break;
            case ActionState.Attacking:
                Debug.Log("State: Attacking");
                baseEnemy.SetVelocity(Vector2.zero);
                break;
        }
    }

    // =========================
    // DECISION MAKING
    // =========================
    void HandleDecision()
    {
        if (Time.time < nextDecisionTime)
            return;

        nextDecisionTime = Time.time + decisionCooldown;

        baseEnemy.SetVelocity(Vector2.zero);

        var possibleActions = new System.Collections.Generic.List<int>();
        if (CanCharge()) possibleActions.Add(1);
        if (CanBackAway()) possibleActions.Add(2);
        if (CanNormalAttack()) possibleActions.Add(3);
        possibleActions.Add(4); // idle move

        int decision = possibleActions[Random.Range(0, possibleActions.Count)];

        switch (decision)
        {
            case 1:
                StartCharge();
                lastChargeTime = Time.time;
                Debug.Log("Decision: StartCharge");
                break;
            case 2:
                currentState = ActionState.BackingUp;
                lastBackAwayTime = Time.time;
                actionDuration = 0.5f;
                backAwayInitialized = false;
                Debug.Log("Decision: BackAway");
                break;
            case 3:
                PerformAttack();
                lastAttackTime = Time.time;
                currentState = ActionState.Attacking;
                actionDuration = 0.5f;
                Debug.Log("Decision: PerformAttack");
                break;
            case 4:
                currentState = ActionState.IdleMove;
                actionDuration = 0.5f;
                Debug.Log("Decision: IdleMove");
                break;
        }
    }

    bool CanCharge() => Time.time >= lastChargeTime + chargeCooldown;
    bool CanBackAway() => Time.time >= lastBackAwayTime + backAwayCooldown;
    bool CanNormalAttack() => Time.time >= lastAttackTime + normalAttackCooldown;

    // =========================
    // BACK AWAY
    // =========================
    void BackAway()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            new Vector2(backAwayDirection, 0f),
            0.5f,
            LayerMask.GetMask("Ground", "Obstacle")
        );

        if (hit)
        {
            baseEnemy.SetVelocity(Vector2.zero);
            actionDuration = 0f;
            currentState = ActionState.Idle;
            backAwayInitialized = false;
            Debug.Log("Backing up stopped due to wall");
            return;
        }

        baseEnemy.SetVelocity(new Vector2(backAwayDirection * backAwaySpeed, baseEnemy.rb.linearVelocity.y));

        if (actionDuration <= 0f)
        {
            currentState = ActionState.Idle;
            backAwayInitialized = false;
            Debug.Log("Backing up finished");
        }
    }

    // =========================
    // CHARGE
    // =========================
    void StartCharge()
    {
        hasAttackedDuringCharge = false;
        startPosition = transform.position;
        float dir = Mathf.Sign(baseEnemy.player.position.x - transform.position.x);
        chargeDirection = new Vector2(dir, 0f);
        currentState = ActionState.Charging;
    }

    void ChargeForward()
    {
        baseEnemy.SetVelocity(new Vector2(chargeDirection.x * chargeSpeed, baseEnemy.rb.linearVelocity.y));

        float distance = Vector2.Distance(startPosition, transform.position);
        float distToPlayer = Vector2.Distance(transform.position, baseEnemy.player.position);

        if ((distance >= maxChargeDistance || distToPlayer < 0.5f) && !hasAttackedDuringCharge)
        {
            PerformChargeAttack();
        }
    }

    void PerformChargeAttack()
    {
        // Deal damage to player
        var playerHealth = baseEnemy.player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            Vector2 knockDir = (baseEnemy.player.position - transform.position).normalized;
            playerHealth.TakeDamage(collisionDamage, knockDir);
            Debug.Log("Player hit by charge, taking damage");
        }

        hasAttackedDuringCharge = true;
        currentState = ActionState.BackingUp;
        backAwayInitialized = false;
        actionDuration = 0.5f;
        lastBackAwayTime = Time.time;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState == ActionState.Charging)
        {
            bool hitPlayer = collision.gameObject.CompareTag("Player");
            bool hitWall = collision.gameObject.layer == LayerMask.NameToLayer("Ground")
                           || collision.gameObject.layer == LayerMask.NameToLayer("Obstacle");

            if (hitPlayer)
            {
                var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    Vector2 knockDir = (collision.transform.position - transform.position).normalized;
                    playerHealth.TakeDamage(collisionDamage, knockDir);
                }
            }

            if (hitPlayer || hitWall)
            {
                currentState = ActionState.BackingUp;
                lastBackAwayTime = Time.time;
                actionDuration = 0.5f;
                backAwayInitialized = false;
                Debug.Log("Charge interrupted due to collision. Switching to BackingUp");
            }
        }
    }

    // =========================
    // BASIC ATTACK
    // =========================
    void PerformAttack()
    {
        var playerHealth = baseEnemy.player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            Vector2 knockDir = (baseEnemy.player.position - transform.position).normalized;
            playerHealth.TakeDamage(collisionDamage, knockDir);
            Debug.Log("Player hit by basic attack");
        }
    }

    // =========================
    // IDLE MOVE
    // =========================
    void IdleMove()
    {
        float dir = Mathf.Sign(baseEnemy.player.position.x - transform.position.x);
        baseEnemy.SetVelocity(new Vector2(dir * 0.5f, baseEnemy.rb.linearVelocity.y));
    }

    public override void TryAttack()
    {
        // no-op
    }
}