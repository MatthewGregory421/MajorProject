using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    public MusicManager musicManager;

    public GameObject deathCanvas;

    private bool playerDead;

    private void Start()
    {
        deathCanvas.SetActive(false);
        playerDead = false;
    }

    private void Update()
    {
        if (playerDead && Input.GetKeyDown(KeyCode.R))
        {
            //this will need to be redone atm, death re-sets to main menu
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void PlayerDied()
    {
        playerDead = true;
        musicManager.StopMusEmitter();
        deathCanvas.SetActive(true);
    }
}
