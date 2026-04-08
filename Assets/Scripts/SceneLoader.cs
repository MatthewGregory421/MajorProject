using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneLoader : MonoBehaviour
{
    public MusicManager musicManager;
    public GameObject menuPanel;
    public UISFXManager uiSFXManager;
    private bool isPaused = false;
    public void LoadScene()
    {
        musicManager.StopMusEmitter();
        SceneManager.LoadScene("TestScene");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isPaused)
            {
                // Open menu and pause
                uiSFXManager.PlayOpen();
                menuPanel.SetActive(true);
                Time.timeScale = 0f;
                isPaused = true;
            }
            else
            {
                // Close menu and resume
                uiSFXManager.PlayClose();
                menuPanel.SetActive(false);
                Time.timeScale = 1f;
                isPaused = false;
            }
        }
    }

    public void ResumeGame()

    {
        uiSFXManager.PlayClose();
        isPaused = false;
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Main Menu button (can be called from UI Button OnClick)
    public void GoToMainMenu()
    {
        // Make sure timeScale is reset before loading main menu
        Time.timeScale = 1f;

        // Replace "MainMenuScene" with your actual main menu scene name
        SceneManager.LoadScene("MainMenu");
    }
}
