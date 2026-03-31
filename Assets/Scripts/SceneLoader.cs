using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneLoader : MonoBehaviour
{
    public MusicManager musicManager;
    public void LoadScene()
    {
        musicManager.StopEmitter1();
        SceneManager.LoadScene("TestScene");
    }
}
