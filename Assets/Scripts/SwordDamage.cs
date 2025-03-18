using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    [Tooltip("Damage dealt by the sword.")]
    public float damage = 10f;

    // To prevent multiple hits on the same enemy in one swipe,
    // you might want to keep track of already damaged enemies.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only deal damage to objects tagged as "Enemy"
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
