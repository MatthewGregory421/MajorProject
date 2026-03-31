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
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void PlayerDied()
    {
        playerDead = true;
        musicManager.StopEmitter1();
        deathCanvas.SetActive(true);
    }
}
