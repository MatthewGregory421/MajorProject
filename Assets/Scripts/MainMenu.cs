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

    [Header("Audio")]
    public Slider volumeSlider;
   // private VCA masterVCA;

    void Start()
    {
        menuPanel.SetActive(true);
        optionsPanel.SetActive(false);

    //    masterVCA = RuntimeManager.GetVCA("vca:/Master");

    //    float savedVolume = PlayerPrefs.GetFloat("volume", 1f);
     //   volumeSlider.value = savedVolume;

    //    SetVolume(savedVolume);

    //    volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void StartGame()
    {
        // hopefully this doesn't break shit
        musicManager.MusAliveFO();
        musicManager.MusHOHPlay();
        SceneManager.LoadScene("Hub Proto");
    }

    public void Options()
    {
        menuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

   // public void SetVolume(float value)
  //  {
   //     masterVCA.setVolume(value);
   //
    //    PlayerPrefs.SetFloat("volume", value);
    //    PlayerPrefs.Save();
//
    //    Debug.Log("FMOD VCA volume: " + value);
   // }

    public void BackButton()
    {
        optionsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
