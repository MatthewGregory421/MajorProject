using UnityEngine;
using System.Collections;

public class test : MonoBehaviour
{

  public FMODUnity.StudioEventEmitter emitter1;
  public FMODUnity.StudioEventEmitter emitter2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      emitter1.Play();
      StartCoroutine(PlayAfterDelay());
    }

    

  IEnumerator PlayAfterDelay()
  {
    yield return new WaitForSeconds(3f);

    SwitchEmitter();
  }


    void SwitchEmitter()
  {
    emitter1.Stop();
    Debug.Log("sound 1 stopped");
    emitter2.Play();
    Debug.Log("Sound 2 started");
  }
}
