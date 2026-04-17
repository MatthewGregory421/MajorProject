using UnityEngine;
using UnityEngine.SceneManagement;

public class SadnessExitLoader : MonoBehaviour
{
    public SceneLoader sceneLoader;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        sceneLoader.GoToAngerProto();
    }
}
