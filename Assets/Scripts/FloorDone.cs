using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorDone : MonoBehaviour
{
    [Tooltip("Assign all the sprite GameObjects you want to track.")]
    public List<GameObject> trackedSprites = new List<GameObject>();

    [Tooltip("Name of the scene to load when all tracked sprites are gone.")]
    public string sceneToLoad;

    void Update()
    {
        // Remove any null references (destroyed objects) from the list
        trackedSprites.RemoveAll(sprite => sprite == null);

        // If list is empty, all tracked sprites are destroyed
        if (trackedSprites.Count == 0)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
