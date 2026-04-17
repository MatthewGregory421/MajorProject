using UnityEngine;

public class DontDestroyMusic : MonoBehaviour
{
    
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("BGM");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);    
    }
}
