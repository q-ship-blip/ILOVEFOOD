using UnityEngine;
using Pathfinding;

public class EnemyPathfinding : MonoBehaviour
{
    public float chaseRange = 5f;             // Distance within which the enemy will start chasing the player.
    public float speed = 3f;                  // Movement speed.
    public float nextWaypointDistance = 0.5f;   // Distance from waypoint to consider it reached.
    public float pathUpdateInterval = 0.5f;     // How often to update the path.
    
    // When the enemy is within this range, it will stop moving and "attack"
    public float attackStopRange = 1f;
    
    // This variable is set to true when the enemy is in attack position and can be used by other scripts.
    public bool AttackPosition = false;
    
    // Offset to adjust the effective movement pivot (set this in the Inspector)
    public Vector2 movementOffset = new Vector2(0, -0.5f);
    
    private Transform target;                 // The player's transform.
    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    private Collider2D enemyCollider;         // The enemy's collider.
    
    void Start()
    {
        // Find the player by tag if not assigned.
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }

        seeker = GetComponent<Seeker>();
        enemyCollider = GetComponent<Collider2D>();
        InvokeRepeating(nameof(UpdatePath), 0f, pathUpdateInterval);
    }

    // Returns the enemy's effective center, adjusted by movementOffset.
    Vector2 GetEnemyCenter()
    {
        if (enemyCollider != null)
            return (Vector2)enemyCollider.bounds.center + movementOffset;
        return (Vector2)transform.position + movementOffset;
    }

    void UpdatePath()
    {
        // Only update the path if the player is within chase range and outside of attack range.
        if (target != null &&
            Vector2.Distance(GetEnemyCenter(), target.position) <= chaseRange &&
            Vector2.Distance(GetEnemyCenter(), target.position) > attackStopRange)
        {
            if (seeker.IsDone())
                seeker.StartPath(GetEnemyCenter(), target.position, OnPathComplete);
        }
        else
        {
            path = null;  // Clear the path if the target is out of range or already within attack range.
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (target == null)
            return;
        
        // Check if the enemy is close enough to the player to stop and "attack".
        if (Vector2.Distance(GetEnemyCenter(), target.position) <= attackStopRange)
        {
            AttackPosition = true;
            return; // Stop movement when in attack range.
        }
        else
        {
            AttackPosition = false;
        }
        
        if (path == null)
            return;

        Vector2 currentPosition = GetEnemyCenter();

        if (currentWaypoint >= path.vectorPath.Count)
            return;

        // Move towards the next waypoint.
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - currentPosition).normalized;
        Vector2 movement = direction * speed * Time.fixedDeltaTime;
        transform.position += (Vector3)movement;

        // Check if the enemy is close enough to the current waypoint to consider it reached.
        if (Vector2.Distance(currentPosition, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}
