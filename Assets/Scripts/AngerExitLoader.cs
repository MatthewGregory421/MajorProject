using UnityEngine;
using UnityEngine.SceneManagement;

public class AngerExitLoader : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public MusicManager musicManager;

    public GameObject gameOverSplashScreen;

    private void Start()
    {
        gameOverSplashScreen.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Time.timeScale = 0f;
        gameOverSplashScreen.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        musicManager.MusAliveFO();
        musicManager.StopMusEmitter();
        SceneManager.LoadScene("MainMenu");
    }
}