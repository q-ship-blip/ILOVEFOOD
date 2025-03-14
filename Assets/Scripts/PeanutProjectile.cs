using UnityEngine;

public class PeanutProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [Tooltip("Speed of the projectile (units per second).")]
    public float projectileSpeed = 10f;
    
    [Tooltip("Lifetime of the projectile in seconds.")]
    public float lifetime = 4f;
    
    [Tooltip("Damage dealt to the player on impact.")]
    public int damage = 1;

    private float lifeTimer;
    private bool hasDealtDamage = false; // Prevent multiple damage applications

    void Awake()
    {
        // Ensure there is a Rigidbody2D attached for trigger detection.
        if (GetComponent<Rigidbody2D>() == null)
        {
            Debug.LogWarning("PeanutProjectile: Rigidbody2D component missing. Please add one and set its Body Type to Kinematic.");
        }
    }

    void Start()
    {
        lifeTimer = lifetime;
    }

    void Update()
    {
        // Move the projectile in its local right direction.
        transform.Translate(Vector2.right * projectileSpeed * Time.deltaTime);
        
        // Count down the lifetime and destroy when time is up.
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    // Called when the projectile enters a trigger collider.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If it hits a wall, destroy it immediately.
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
            return;
        }
        
        // If it hits the player, deal damage (only once) and destroy the projectile.
        if (collision.CompareTag("Player") && !hasDealtDamage)
        {
            hasDealtDamage = true;
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
