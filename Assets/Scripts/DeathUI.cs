using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    [Tooltip("How many seconds to wait before auto-respawn")]
    public float respawnDelay = 3f;

    [Tooltip("How much to heal before respawn (in half-hearts)")]
    public int healAmount = 10;

    void Start()
    {
        HidePlayer();
        Invoke("RespawnPlayer", respawnDelay);
    }

    void HidePlayer()
    {
        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        if (player != null)
        {
            // Hide player's sprite
            SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = false;

            // Disable movement
            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            if (movement != null) movement.enabled = false;
        }
    }

    void RespawnPlayer()
    {
        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        if (player != null)
        {
            // Heal the player
            player.Heal(healAmount);

            // Restore sprite
            SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = true;

            // Enable movement
            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            if (movement != null) movement.enabled = true;
        }

        // Respawn to the previous scene
        GameManager.Instance.Respawn();
    }
}
