using UnityEngine;

public class EnemyAnim : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void AttackActivate()
    {
        Debug.Log("Attack anim");
        animator.SetTrigger("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Xvel", rb.linearVelocityX);
    }
}
