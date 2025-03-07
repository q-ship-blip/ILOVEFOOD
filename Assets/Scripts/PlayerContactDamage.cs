using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]  // Makes sure the player has health component.
public class PlayerContactDamage : MonoBehaviour
{
    public float damageInterval = 0.4f; // Time between damage ticks.
    public int damageAmount = 1;        // Damage to deal per tick.

    private bool isTouchingEnemy = false;
    private Coroutine damageCoroutine;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!isTouchingEnemy)
            {
                isTouchingEnemy = true;
                damageCoroutine = StartCoroutine(ApplyDamageOverTime());
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            isTouchingEnemy = false;
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    IEnumerator ApplyDamageOverTime()
    {
        while (isTouchingEnemy)
        {
            playerHealth.TakeDamage(damageAmount);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
