using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static GameObject[] persistentObjects = new GameObject[3];
    public int objectIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (persistentObjects[objectIndex] == null)
        {
            DontDestroyOnLoad(gameObject);
            
            
        }
        else if (persistentObjects[objectIndex] != null)
        {
            Destroy(gameObject);
        }

        
    }

}
