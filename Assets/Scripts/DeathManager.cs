using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    public MusicManager musicManager;
    public UISFXManager uiSFXManager;

    public GameObject deathCanvas;

    private void Start()
    {
        deathCanvas.SetActive(false);
    }

    public void PlayerDied()
    {
        Time.timeScale = 0;
        deathCanvas.SetActive(true);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        uiSFXManager.PlayConfirm();
        musicManager.MusDeadFO();
        musicManager.StopMusEmitter();
        SceneManager.LoadScene("MainMenu");
    }

    public void Respawn()
    {
        Time.timeScale = 1;
        uiSFXManager.PlayConfirm();
        musicManager.MusDeadFO();
        musicManager.StopMusEmitter();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
