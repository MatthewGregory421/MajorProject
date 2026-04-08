using FMODUnity;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public StudioEventEmitter Mus;
    
    [SerializeField]
    [ParamRef]
    private string musicSelect = null;


    
    private void Start()
    {
        //The line below is set up only for testing purposes it should be deleted and it's function handled elsewhere
        RuntimeManager.StudioSystem.setParameterByName(musicSelect, 2);
        Mus.Play();
    }

        public void StopMusEmitter()
    {
        RuntimeManager.StudioSystem.setParameterByName(musicSelect, 0);
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
}