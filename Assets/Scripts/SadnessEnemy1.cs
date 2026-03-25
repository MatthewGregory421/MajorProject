using UnityEngine;

public class SadnessEnemy1 : BaseEnemy
{
    [Header("Sadness Identity")]
    public bool isSadnessType = true;

    protected override void Awake()
    {
        base.Awake();

        // =====================
        // LEVEL 1 SADNESS STATS
        // =====================

        maxHealth = 4;
        moveSpeed = 1.2f;     // slow, heavy movement
        damage = 1;           // basic damage

        currentHealth = maxHealth;
    }
}