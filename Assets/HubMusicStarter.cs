using UnityEngine;

public class HubMusicStarter : MonoBehaviour
{
    public MusicManager musicManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        musicManager.MusHOHPlay();
        //musicManager.PlayMusEmitter();
    }
}
