using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectableAttribute : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        string playerTag = otherCollider.gameObject.tag;

        // Check if the object colliding is a player
        if (playerTag == "Player" || playerTag == "Player2")
        {
            // Load the next level
            SceneManager.LoadScene("Level1");

            // Destroy the collectible object
            Destroy(gameObject);
        }
    }
}
