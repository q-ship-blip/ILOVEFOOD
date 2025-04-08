using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSpawnManager : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Invoke(nameof(MovePlayerToSpawn), 0.1f); // Slight delay to ensure all objects are loaded
    }

    void MovePlayerToSpawn()
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject spawnPoint = GameObject.Find("PlayerSpawn");

        if (player != null && spawnPoint != null)
        {
            player.transform.position = spawnPoint.transform.position;
            Debug.Log($"Player moved to spawn point in scene: {spawnPoint.transform.position}");
        }
        else
        {
            Debug.LogWarning("Could not move player — Player or PlayerSpawn not found.");
        }
    }
}
