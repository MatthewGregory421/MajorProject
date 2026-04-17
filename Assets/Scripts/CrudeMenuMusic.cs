using FMODUnity;
using UnityEngine;

public class CrudeMenuMusic : MonoBehaviour
{
    public StudioEventEmitter Mus;
    
    [SerializeField]
    [ParamRef]
    private string musicSelect = null;


    
    private void Start()
    {
        //The line below is set up only for testing purposes it should be deleted and it's function handled elsewhere
        RuntimeManager.StudioSystem.setParameterByName(musicSelect, 1);
        Mus.Play();
    }

    public void StopMenuMusic()
    {
        RuntimeManager.StudioSystem.setParameterByName(musicSelect, 0);
    }
}
