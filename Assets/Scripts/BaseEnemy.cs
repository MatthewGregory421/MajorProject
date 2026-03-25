using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 3;
    public int damage = 1;
    public float moveSpeed = 2f;

    [Header("Runtime")]
    public int currentHealth;

    [Header("References")]
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Transform player;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
        else
            Debug.LogWarning("BaseEnemy: Player not found (check tag)");
    }

    public void SetVelocity(Vector2 vel)
    {
        if (rb == null) return;
        rb.linearVelocity = vel;
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
            Destroy(gameObject);
    }
}