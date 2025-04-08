using UnityEngine;

public class CanvasPersistence : MonoBehaviour
{
    private static GameObject instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != gameObject)
        {
            Destroy(gameObject); // Avoid duplicates
        }
    }
}
