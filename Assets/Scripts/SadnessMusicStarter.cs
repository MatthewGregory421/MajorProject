using UnityEngine;

// This script is a crude way of controlling how the music changes between levels. DO NOT ADD IT TO THE PREFAB
public class SadnessMusicStarter : MonoBehaviour
{
    
    public MusicManager musicManager;

    private void Start()
    {
        musicManager.MusSOSPlay();
    }
}
