using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneLoader : MonoBehaviour
{
    public MusicManager musicManager;
    public GameObject menuPanel;
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
                menuPanel.SetActive(true);
                Time.timeScale = 0f;
                isPaused = true;
            }
            else
            {
                // Close menu and resume
                menuPanel.SetActive(false);
                Time.timeScale = 1f;
                isPaused = false;
            }
        }
    }

    public void ResumeGame()
    {
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
