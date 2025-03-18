using UnityEngine;

public class PeanutShooter : MonoBehaviour
{
    [Tooltip("Prefab of the peanut projectile to spawn.")]
    public GameObject peanutProjectilePrefab;

    [Tooltip("Delay between shots in seconds.")]
    public float shotDelay = 1f;

    // Timer to track when to shoot next.
    private float shotTimer;

    // Reference to the PeanutPathfinding on the parent object.
    private PeanutPathfinding peanutPathfinding;

    void Start()
    {
        // Initialize the timer.
        shotTimer = shotDelay;
        
        // Use GetComponentInParent to find the PeanutPathfinding component on the parent.
        peanutPathfinding = GetComponentInParent<PeanutPathfinding>();
        if (peanutPathfinding == null)
        {
            Debug.LogWarning("PeanutShooter: No PeanutPathfinding component found in parent. The shooter will fire regardless of attack position.");
        }
    }

    void Update()
    {
        // Check if the parent is in attack position.
        bool isInAttackPosition = peanutPathfinding ? peanutPathfinding.AttackPosition : true;

        if (isInAttackPosition)
        {
            shotTimer -= Time.deltaTime;
            if (shotTimer <= 0f)
            {
                ShootPeanut();
                shotTimer = shotDelay; // Reset the timer after shooting.
            }
        }
    }

    void ShootPeanut()
    {
        if (peanutProjectilePrefab != null)
        {
            // Instantiate the peanut projectile at the current position and rotation.
            Instantiate(peanutProjectilePrefab, transform.position, transform.rotation);
        }
        else
        {
            Debug.LogWarning("PeanutShooter: Peanut Projectile Prefab is not assigned in the Inspector!");
        }
    }
}
