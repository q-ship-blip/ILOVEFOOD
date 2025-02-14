using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 100f; // Default health
    public GameObject player;
    public float speed = 2f;

    private float distance;
    private bool isDead = false; // Flag to prevent movement after death

    void Update()
    {
        if (player == null || isDead) return; // Prevent errors if player is missing or enemy is dead

        distance = Vector2.Distance(transform.position, player.transform.position);

        // Move towards the player
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + " took damage: " + damage);

        health -= damage;
        Debug.Log(gameObject.name + " health: " + health);

        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " died!");
        isDead = true; // Stop movement in Update()
        Destroy(gameObject);
    }
}
