using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FakeLoad : MonoBehaviour
{
    IEnumerator Start()
    {
        // Wait for a fraction of a second to ensure the engine is fully ready
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("MainMenu");
    }
}
