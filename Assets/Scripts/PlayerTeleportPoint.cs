using UnityEngine;

public class PlayerTeleportPoint : MonoBehaviour
{
    [Tooltip("The exact position to teleport the player to")]
    public Vector2 targetPosition = new Vector2(0, 0);

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            player.transform.position = targetPosition;
            Debug.Log($"✅ Player teleported to {targetPosition}");
        }
        else
        {
            Debug.LogWarning("❌ Player not found in scene.");
        }
    }
}
