using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneTest : MonoBehaviour
{
    public SceneLoader sceneLoader;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        sceneLoader.LoadScene();
    }
}
