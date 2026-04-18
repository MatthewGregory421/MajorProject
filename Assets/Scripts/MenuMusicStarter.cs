using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusicStarter : MonoBehaviour
{
    public MusicManager musicManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     [SerializeField]
    [ParamRef]
    private string MusicVolume = null;

    [SerializeField]
    [ParamRef]
    private string SFXVolume = null;

    [SerializeField]
    [ParamRef]
    private string MasterVolume = null;

    private void Start()
    {
        RuntimeManager.StudioSystem.setParameterByName(MusicVolume, 10);
        RuntimeManager.StudioSystem.setParameterByName(SFXVolume, 9);
        RuntimeManager.StudioSystem.setParameterByName(MasterVolume, 10);
        musicManager.MusAllOHPlay();
        musicManager.PlayMusEmitter();
    }
}
