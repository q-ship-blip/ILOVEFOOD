using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static bool exists;

    void Awake()
    {
        if (!exists)
        {
            DontDestroyOnLoad(gameObject);
            exists = true;
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }
}
