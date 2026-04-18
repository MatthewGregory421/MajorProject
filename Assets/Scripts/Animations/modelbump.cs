using UnityEngine;

public class modelbump : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void verticalstretch( float size)
    {
        gameObject.transform.localScale = new Vector3(0, size, 0);
    }
    public void horizontalstretch(float size)
    {
        gameObject.transform.localScale = new Vector3(size * Mathf.Sign(gameObject.transform.localScale.x), 0.5f, 0);
    }
}
