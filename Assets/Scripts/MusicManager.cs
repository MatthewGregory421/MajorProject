using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public StudioEventEmitter Mus;
    
    [SerializeField]
    [ParamRef]
    private string musicSelect = null;

    [SerializeField]
    [ParamRef]
    private string FadeoutAlive = null;


    
        public void PlayMusEmitter()
    {
          Mus.Play();
    }

        public void SilenceMusEmitter()
    {
        RuntimeManager.StudioSystem.setParameterByName(musicSelect, 0);
        //Mus.Stop();
    }

    // The below code is how we select which music plays, the tricky thing is setting up the scene loading so that each scene uniquely calls one of these functions
        public void MusAllOHPlay()
    {
        RuntimeManager.StudioSystem.setParameterByName(musicSelect, 1);
    }

     public void MusHOHPlay()
    {
        RuntimeManager.StudioSystem.setParameterByName(musicSelect, 2);
    }

     public void MusAOHPlay()
    {
        RuntimeManager.StudioSystem.setParameterByName(musicSelect, 3);
    }

     public void MusSOHPlay()
    {
        RuntimeManager.StudioSystem.setParameterByName(musicSelect, 4);
    }

     public void MusAOSPlay()
    {
        RuntimeManager.StudioSystem.setParameterByName(musicSelect, 5);
    }

     public void MusHOSPlay()
    {
        RuntimeManager.StudioSystem.setParameterByName(musicSelect, 6);
    }

     public void MusSOSPlay()
    {
        RuntimeManager.StudioSystem.setParameterByName(musicSelect, 7);
    }

     public void MusAOAPlay()
    {
        RuntimeManager.StudioSystem.setParameterByName(musicSelect, 8);
    }

     public void MusHOAPlay()
    {
        RuntimeManager.StudioSystem.setParameterByName(musicSelect, 9);
    }

     public void MusSOAPlay()
    {
        RuntimeManager.StudioSystem.setParameterByName(musicSelect, 10);
    }

    public void MusAliveFO()
    {
        RuntimeManager.StudioSystem.setParameterByName(FadeoutAlive, 1);
    }

    public void MusDeadFO()
    {
        RuntimeManager.StudioSystem.setParameterByName(FadeoutAlive, 0);
    }
}