using UnityEngine;

public class MenuMusicStarter : MonoBehaviour
{
    public MusicManager musicManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        musicManager.MusAllOHPlay();
        musicManager.PlayMusEmitter();
    }
}
