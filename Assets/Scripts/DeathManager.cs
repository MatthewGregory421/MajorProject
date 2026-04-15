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
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void PlayerDied()
    {
        Time.timeScale = 0;
        playerDead = true;
        musicManager.MusDeadFO();
        musicManager.StopMusEmitter();
        deathCanvas.SetActive(true);
    }

    public void RespawnMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
