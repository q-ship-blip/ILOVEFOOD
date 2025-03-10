using UnityEngine;
using Pathfinding;

public class EnemyPathfinding : MonoBehaviour
{
    public float chaseRange = 5f;             // Distance within which the enemy chases the player.
    public float speed = 3f;                  // Movement speed.
    public float nextWaypointDistance = 0.5f;   // Distance to consider a waypoint reached.
    public float pathUpdateInterval = 0.5f;     // How often to update the path.
    public float attackStopRange = 1f;          // Distance at which enemy stops to attack.
    
    // Offset to adjust the effective movement pivot (set this in the Inspector)
    public Vector2 movementOffset = new Vector2(0, -0.5f);
    
    // This is set to true when the enemy is in attack position.
    public bool AttackPosition = false;
    
    private Transform target;                 // The player's transform.
    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    private Collider2D enemyCollider;
    private Rigidbody2D rb;                   // Rigidbody2D reference for physics movement.

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
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating(nameof(UpdatePath), 0f, pathUpdateInterval);
    }

    // Get the enemy's effective center using its collider plus the offset.
    Vector2 GetEnemyCenter()
    {
        if (enemyCollider != null)
            return (Vector2)enemyCollider.bounds.center + movementOffset;
        return (Vector2)transform.position + movementOffset;
    }

    void UpdatePath()
    {
        if (target != null &&
            Vector2.Distance(GetEnemyCenter(), target.position) <= chaseRange &&
            Vector2.Distance(GetEnemyCenter(), target.position) > attackStopRange)
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(GetEnemyCenter(), target.position, OnPathComplete);
            }
        }
        else
        {
            path = null;  // Clear the path if the target is out of range or within attack range.
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            Debug.Log("Path updated. Waypoints count: " + path.vectorPath.Count);
        }
    }

    void FixedUpdate()
    {
        if (target == null)
            return;
        
        float distanceToTarget = Vector2.Distance(GetEnemyCenter(), target.position);
        Debug.Log("Distance to target: " + distanceToTarget + " | AttackStopRange: " + attackStopRange);

        // If within attack range, stop moving.
        if (distanceToTarget <= attackStopRange)
        {
            AttackPosition = true;
            Debug.Log("In attack range, not moving.");
            return;
        }
        else
        {
            AttackPosition = false;
        }

        if (path == null)
        {
            Debug.Log("No path available.");
            return;
        }

        Vector2 currentPosition = GetEnemyCenter();

        if (currentWaypoint >= path.vectorPath.Count)
        {
            Debug.Log("Reached end of path.");
            return;
        }

        // Calculate movement direction.
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - currentPosition).normalized;
        Vector2 movement = direction * speed * Time.fixedDeltaTime;
        
        // Move using Rigidbody2D so collisions are handled.
        rb.MovePosition(rb.position + movement);
        Debug.Log("Moving enemy by: " + movement);

        // Advance to next waypoint if close enough.
        if (Vector2.Distance(currentPosition, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
        {
            currentWaypoint++;
            Debug.Log("Advancing to waypoint: " + currentWaypoint);
        }
    }
}
