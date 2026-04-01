using UnityEngine;

public class MusicManager : MonoBehaviour
{
    //For Josh, this script can be used as a TEMPLATE, for SFX management and triggering DO NOT ADD OR SUBTRACT THINGS FROM THIS SCRIPT, COPY IT'S CONTENTS AND MAKE YOU'RE OWN
    // Okay, so it might make the most sense to have each sound effect cluster (i.e. player, emotion specific enemies, etc) handled by a seperate script so just keep that in mind
    // Step one though is copy this script to a new script, name that script whatever makes the most sence
    
    //Step two, these are your emitter controllers, copy one of the lines below and change that light blue lettering (i.e. "MusOver") to whatever makes the most sense for the event you want to call
    // now over in unity, you can drag that event emitter from whatever object it's on into the correctly labelled field and Bob's your mums brother. Now this script can control that emitter with specific controls. I reccomend doing this step last because it'll save you going back and forth a bunch.
    public FMODUnity.StudioEventEmitter MusOver;
    public FMODUnity.StudioEventEmitter MusUnder;

    // For Josh, ignore this section, there shouldn't be any sound effects you're running on start I don't think.
    private void Start()
    {
        MusOver.Play();
        MusUnder.Play();
    }

    //For Matt, I've changed "StopEmitter1" to "StopMusEmitter" so that it's more clear whats going on. That way it'll make scritping easier when we start doing more stuff with SFX etc
    //I also changed the references to "StopEmitter1" in DeathManager and SceneLoader, Please don't kill me.
    
    // For Josh, this is the section you need.
    //Basically these voids are how other scripts are going to call this one, in order to play sound effects.
    // Copy the script below and change "StopMusEmitter" to a sound effect specific Play order (i.e. "PlayPlayerWalkEmitter")
    // Delete one of the stop orders in the below script and change the other one to "TheNameOfTheEmitterYouWantToPlay.Play();" (e.g. "PlayerWalk.Play();")
    // Since there's no stop order, you'll need to set up event stoppages in FMod so that we don't end up with 5 billion silent sound effect instances
    // Next, give Matt a call, get him to check the script and ask how to connect it up to the neccessary scripts and object to make sure it's called properly
    // NOTE, these voids will do everything you put in them, so you'll want to have seperate ones for each sound effect and just have the external script calling multiple voids.
    
    public void StopMusEmitter()
    {
        MusOver.Stop();
        MusUnder.Stop();
    }
}