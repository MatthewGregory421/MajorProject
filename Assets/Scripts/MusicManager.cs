using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter Over;
    public FMODUnity.StudioEventEmitter Under;

    private void Start()
    {
        Over.Play();
        Under.Play();
    }

    public void StopEmitter1()
    {
        Over.Stop();
        Under.Stop();
    }
}