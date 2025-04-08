using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSpawnManager : MonoBehaviour
{
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        // If the scene is already loaded (e.g. entering first scene), position player right away
        PositionPlayer();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PositionPlayer();
    }

    void PositionPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject spawnPoint = GameObject.Find("PlayerSpawn");

        if (player != null && spawnPoint != null)
        {
            player.transform.position = spawnPoint.transform.position;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
