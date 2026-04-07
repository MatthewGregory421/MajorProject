using UnityEngine;

public class PlayerAnims : MonoBehaviour
{
    [SerializeField] PlayerCombat pCombat;
    [SerializeField] PlayerMovement pMovement;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] int lookdirection = 1;

    [SerializeField] Transform body;

    bool grounded;
    bool crouched;

    //trigger attack
    //trigger jump

    float yVelocity;
    float xVelocity;

    //groundslam
    bool groundslam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Jumpo");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            animator.SetTrigger("Attack");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        crouched = pMovement.isCrouching;
        grounded = pMovement.isGrounded;
        groundslam = pMovement.isGroundSlamming;



        yVelocity = rb.linearVelocityY;
        xVelocity = Mathf.Abs(rb.linearVelocityX);

        //Handling lookdirection
        if (pMovement.lookHorizontal != lookdirection && pMovement.lookHorizontal != 0)
        {
            lookdirection = pMovement.lookHorizontal;
            body.localScale =new Vector3(lookdirection, 1, 1);
        }

       

        animator.SetBool("Crouched", crouched);
        animator.SetBool("Grounded", grounded);
        animator.SetBool("Groundslam", groundslam);

        animator.SetFloat("Yvelocity", yVelocity);
        animator.SetFloat("Xvelocity",xVelocity);
    }
}
