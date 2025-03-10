using System.Collections;
using UnityEngine;

// You can rename this script to EnemyContactDamage or keep the name as is.
// We remove the [RequireComponent(typeof(PlayerHealth))] because the script is now on the enemy, 
// and we want to access the PLAYER's health, not the enemy's.

public class PlayerContactDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damageInterval = 0.4f; // Time between damage ticks
    public int damageAmount = 1;        // Damage dealt per tick

    private bool isTouchingPlayer = false;
    private Coroutine damageCoroutine;

    // Called when another collider enters this trigger.
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is tagged "Player".
        if (other.CompareTag("Player"))
        {
            // If we just started touching the player, begin damaging them over time.
            if (!isTouchingPlayer)
            {
                isTouchingPlayer = true;
                // Pass the player's collider so we can grab PlayerHealth from it.
                damageCoroutine = StartCoroutine(ApplyDamageOverTime(other));
            }
        }
    }

    // Called when another collider exits this trigger.
    void OnTriggerExit2D(Collider2D other)
    {
        // If the player leaves, stop applying damage.
        if (other.CompareTag("Player"))
        {
            isTouchingPlayer = false;
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    // Continuously deal damage to the player while isTouchingPlayer is true.
    private IEnumerator ApplyDamageOverTime(Collider2D playerCollider)
    {
        // Attempt to get the player's health component.
        PlayerHealth playerHealth = playerCollider.GetComponent<PlayerHealth>();

        while (isTouchingPlayer)
        {
            if (playerHealth != null)
            {
                // Apply damage to the player.
                playerHealth.TakeDamage(damageAmount);
            }

            // Wait the specified interval before applying damage again.
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
