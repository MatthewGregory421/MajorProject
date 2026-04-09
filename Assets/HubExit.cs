using UnityEngine;
using UnityEngine.SceneManagement;

public class HubExit : MonoBehaviour
{
    public SceneLoader sceneLoader;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        sceneLoader.GoToSadnessProto();
    }
}
