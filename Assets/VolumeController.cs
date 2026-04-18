using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
public class VolumeController : MonoBehaviour
{   
    // these fields give us control over our global parameters for volume

    [SerializeField]
    [ParamRef]
    private string MusicVolume = null;

    [SerializeField]
    [ParamRef]
    private string SFXVolume = null;

    [SerializeField]
    [ParamRef]
    private string MasterVolume = null;

    
    //These Public voids are my best guess at making functions to handle those volume changes

    //Music
    public void IncreaseMusVolume()
    {
        RuntimeManager.StudioSystem.setParameterByName(MusicVolume, +1);
        //idk if that plus one actually behaves like I think it should, just adding 1 to whatever the parameter value currently is
    }

    public void DecreaseMusVolume()
    {
        RuntimeManager.StudioSystem.setParameterByName(MusicVolume, -1);
        //idk if that minus one actually behaves like I think it should, just taking one away from whatever the parameter value currently is
    }

    //SFX
    public void IncreaseSFXVolume()
    {
        RuntimeManager.StudioSystem.setParameterByName(SFXVolume, +1);
        //idk if that plus one actually behaves like I think it should, just adding 1 to whatever the parameter value currently is
    }

    public void DecreaseSFXVolume()
    {
        RuntimeManager.StudioSystem.setParameterByName(SFXVolume, -1);
        //idk if that minus one actually behaves like I think it should, just taking one away from whatever the parameter value currently is
    }

    //Master
    public void IncreaseMasterVolume()
    {
        RuntimeManager.StudioSystem.setParameterByName(MasterVolume, +1);
        //idk if that plus one actually behaves like I think it should, just adding 1 to whatever the parameter value currently is
    }

    public void DecreaseMasterVolume()
    {
        RuntimeManager.StudioSystem.setParameterByName(MasterVolume, -1);
        //idk if that minus one actually behaves like I think it should, just taking one away from whatever the parameter value currently is
    }

    //in theory if things work the way I think, it should now just be a matter of hooking these functions up to whichever menus screen they need to be attached too.

}
