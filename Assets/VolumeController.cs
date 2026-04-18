using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class VolumeController : MonoBehaviour
{
    [Header("FMOD Parameters")]
    [SerializeField][ParamRef] private string MasterVolume;
    [SerializeField][ParamRef] private string MusicVolume;
    [SerializeField][ParamRef] private string SFXVolume;

    [Header("UI Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        // Load saved values (default = 1)
        float master = PlayerPrefs.GetFloat("MasterVol", 1f);
        float music = PlayerPrefs.GetFloat("MusicVol", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVol", 1f);

        // Set slider positions
        masterSlider.value = master;
        musicSlider.value = music;
        sfxSlider.value = sfx;

        // Apply to FMOD immediately
        SetMasterVolume(master);
        SetMusicVolume(music);
        SetSFXVolume(sfx);

        // Hook sliders
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float value)
    {
        float scaledValue = value * 10f;

        RuntimeManager.StudioSystem.setParameterByName(MasterVolume, scaledValue);

        Debug.Log($"Master Slider: {value} = FMOD: {scaledValue}");

        PlayerPrefs.SetFloat("MasterVol", value);
    }

    public void SetMusicVolume(float value)
    {
        float scaledValue = value * 10f;

        RuntimeManager.StudioSystem.setParameterByName(MusicVolume, scaledValue);

        Debug.Log($"Music Slider: {value} = FMOD: {scaledValue}");

        PlayerPrefs.SetFloat("MusicVol", value);
    }

    public void SetSFXVolume(float value)
    {
        float scaledValue = value * 10f;

        RuntimeManager.StudioSystem.setParameterByName(SFXVolume, scaledValue);

        Debug.Log($"SFX Slider: {value} = FMOD: {scaledValue}");

        PlayerPrefs.SetFloat("SFXVol", value);
    }
}