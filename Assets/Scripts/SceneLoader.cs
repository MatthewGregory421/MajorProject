using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneLoader : MonoBehaviour
{
    public MusicManager musicManager;
    public void LoadScene()
    {
        musicManager.StopMusEmitter();
        SceneManager.LoadScene("TestScene");
    }
}
