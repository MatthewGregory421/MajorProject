using UnityEngine;
using FMODUnity;

public class SadnessSFXManager : MonoBehaviour
{
    public StudioEventEmitter sadnessAttack;

    public void PlaySadnessAttackEmitter()
    {
        sadnessAttack.Play();
    }
}
