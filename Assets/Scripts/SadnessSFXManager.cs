using UnityEngine;
using FMODUnity;
using System.Collections.Generic;

public class SadnessSFXManager : MonoBehaviour
{
   

    public StudioEventEmitter sadnessAttack;
    public StudioEventEmitter EnemyHit;
    public StudioEventEmitter EnemyIdle;
    //public StudioEventEmitter footstepEmitter;
    [Header("Cooldown Settings")]
    public float idleCooldown = 5f; // 5 seconds cooldown
    private float lastidleTime = -Mathf.Infinity;
    [Header("Footsteps")]
    

    public float minStepInterval = 0.5f;
    public float maxStepInterval = 0.25f;

    private float stepTimer;

    public class EnemyFootstepData
    {
        public Rigidbody2D rb;
        public float moveSpeed;
        public float stepTimer;
    }
    private List<EnemyFootstepData> enemies = new List<EnemyFootstepData>();

    public void RegisterEnemy(Rigidbody2D rb, float moveSpeed)
    {
        enemies.Add(new EnemyFootstepData { rb = rb, moveSpeed = moveSpeed, stepTimer = 0f });
    }
    private void Update()
    {
        foreach (var e in enemies)
        {
            HandleFootsteps(e);
        }
    }
    public void PlaySadnessAttackEmitter()
    {
        sadnessAttack.Play();
    }
    public void PlayEnemyHitEmitter()
    {
        EnemyHit.Play();
    }
    public void PlayEnemyIdleEmitter()
    {
        if (Time.time - lastidleTime >= idleCooldown)
        {
            EnemyIdle.Play();
                lastidleTime = Time.time;
        }
    }
    public void HandleFootsteps(EnemyFootstepData enemy)
    {
        if (enemy.rb == null) return;

        float currentSpeed = Mathf.Abs(enemy.rb.linearVelocity.x);
        float speedPercent = Mathf.Clamp01(currentSpeed / enemy.moveSpeed);
        float currentStepInterval = Mathf.Lerp(minStepInterval, maxStepInterval, speedPercent);

        if (currentSpeed > 0.1f)
        {
            enemy.stepTimer -= Time.deltaTime;
            if (enemy.stepTimer <= 0f)
            {
                //footstepEmitter.Play();
                enemy.stepTimer = currentStepInterval;
            }
        }
        else
        {
            enemy.stepTimer = 0f;
        }
    }
}

