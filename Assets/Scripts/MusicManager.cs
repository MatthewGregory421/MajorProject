using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter emitter1;
    public FMODUnity.StudioEventEmitter emitter2;

    private void Start()
    {
        emitter1.Play();
    }

    public void StopEmitter1()
    {
        emitter1.Stop();
    }
}