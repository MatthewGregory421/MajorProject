using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class MainMenu : MonoBehaviour
{
    //hopefull this doesn't break shit

    [Header("Music")]
    public MusicManager musicManager;

    public GameObject menuPanel;
    public GameObject optionsPanel;
   // private VCA masterVCA;

   public UISFXManager uiSFXManager;

    void Start()
    {
        menuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void StartGame()
    {
        // hopefully this doesn't break shit
        uiSFXManager.PlayConfirm();
        musicManager.MusAliveFO();
        musicManager.MusHOHPlay();
        SceneManager.LoadScene("Hub Proto");
    }

    public void Options()
    {
        uiSFXManager.PlayConfirm();
        menuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void BackButton()
    {
        uiSFXManager.PlayBack();
        optionsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        uiSFXManager.PlayConfirm();
        Application.Quit();
    }
}
