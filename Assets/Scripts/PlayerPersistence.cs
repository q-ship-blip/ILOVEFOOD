using UnityEngine;

public class PlayerPersistence : MonoBehaviour
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
            Destroy(gameObject); // Prevent duplicates if scene is reloaded
        }
    }
}
