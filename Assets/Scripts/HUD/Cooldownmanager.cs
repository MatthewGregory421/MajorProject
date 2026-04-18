using UnityEngine;

public class Cooldownmanager : MonoBehaviour
{
    [SerializeField] GameObject gScooldown;
    [SerializeField] GameObject dashCooldown;
    [SerializeField] GameObject player;

    PlayerMovement pMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pMovement =  player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pMovement.canGroundSlam)
        {
            gScooldown.SetActive(true);
        }
        else
        {
            gScooldown.SetActive(false);
        }

        dashCooldown.SetActive(pMovement.CheckDashCoolDown());
    }
}
