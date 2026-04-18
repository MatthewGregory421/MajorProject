using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public UISFXManager uiSFXManager;
    public MusicManager musicManager; 

    public GameObject pauseScreen;
    private bool pauseMenuActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseScreen.SetActive(false);
        pauseMenuActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (!pauseMenuActive)
            {
                pauseMenuActive = true;
                Time.timeScale = 0f;
                pauseScreen.SetActive(true);
                uiSFXManager.PlayOpen();
            }
            else
            {
                pauseMenuActive = false;
                Time.timeScale = 1f;
                pauseScreen.SetActive(false);
                uiSFXManager.PlayClose();
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        uiSFXManager.PlayConfirm();
        pauseScreen.SetActive(false);
        if (pauseMenuActive == true)
        {
            pauseMenuActive = false; 
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        uiSFXManager.PlayConfirm();
        musicManager.MusAliveFO();
        musicManager.SilenceMusEmitter();
        SceneManager.LoadScene("MainMenu");
    }

    public void Respawn()
    {
        Time.timeScale = 1;
        uiSFXManager.PlayConfirm();
        musicManager.MusAliveFO();
        musicManager.SilenceMusEmitter();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
