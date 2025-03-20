using UnityEngine;
using Pathfinding;

public class EnemyPathfinding : MonoBehaviour
{
    [Header("Pathfinding Settings")]
    public float chaseRange = 5f;             // Distance within which the enemy chases the player.
    public float speed = 3f;                  // Base movement speed.
    public float nextWaypointDistance = 0.6f; // Distance to consider a waypoint reached.
    public float pathUpdateInterval = 0.5f;   // How often to update the path.
    public float attackStopRange = 1f;        // Distance at which enemy stops to attack.

    [Header("Smoothing Settings")]
    public float smoothingTime = 0.1f;        // How quickly the enemy accelerates/decelerates.

    // Offset to adjust the effective movement pivot (set in Inspector)
    public Vector2 movementOffset = new Vector2(0, -0.5f);

    // True when the enemy is in attack position
    public bool AttackPosition = false;

    private Transform target;                 // The player's transform.
    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    private Collider2D enemyCollider;
    private Rigidbody2D rb;
    private Vector2 velocityRef = Vector2.zero; // Used for SmoothDamp.

    // How close the final path node must be to the player's position for the path to be considered "complete"
    public float endReachedThreshold = 0.5f;

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

        // Repeatedly request new paths
        InvokeRepeating(nameof(UpdatePath), 0f, pathUpdateInterval);
    }

    // Returns the enemy's effective center using its collider plus the offset.
    Vector2 GetEnemyCenter()
    {
        if (enemyCollider != null)
            return (Vector2)enemyCollider.bounds.center + movementOffset;
        return (Vector2)transform.position + movementOffset;
    }

    void UpdatePath()
    {
        if (target != null)
        {
            float distance = Vector2.Distance(GetEnemyCenter(), target.position);

            // If within chase range but outside attack range, try to pathfind
            if (distance <= chaseRange && distance > attackStopRange && seeker.IsDone())
            {
                seeker.StartPath(GetEnemyCenter(), target.position, OnPathComplete);
            }
            else
            {
                path = null; // Clear path if out of chase range or too close to attack
            }
        }
        else
        {
            path = null;
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error && p.vectorPath != null && p.vectorPath.Count > 1)
        {
            // Check if the last node is near the player's position
            Vector3 lastNode = p.vectorPath[p.vectorPath.Count - 1];
            float distanceToPlayer = Vector3.Distance(lastNode, target.position);

            // If the path's end is close enough to the player's position, accept it; otherwise, discard
            if (distanceToPlayer <= endReachedThreshold)
            {
                path = p;
                currentWaypoint = 0;
            }
            else
            {
                // Partial path: discard
                path = null;
            }
        }
        else
        {
            // If there's an error or too few nodes, discard
            path = null;
        }
    }

    void FixedUpdate()
    {
        if (target == null) return;

        float distanceToTarget = Vector2.Distance(GetEnemyCenter(), target.position);

        // If within attack range, stop and set AttackPosition
        if (distanceToTarget <= attackStopRange)
        {
            AttackPosition = true;
            rb.linearVelocity = Vector2.zero; 
            return;
        }
        else
        {
            AttackPosition = false;
        }

        // If path is null (no complete path), stop
        if (path == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Follow the path
        Vector2 currentPosition = GetEnemyCenter();

        // If we've reached the end of the path, do nothing
        if (currentWaypoint >= path.vectorPath.Count)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 targetPos = (Vector2)path.vectorPath[currentWaypoint];
        Vector2 direction = (targetPos - currentPosition).normalized;
        Vector2 desiredVelocity = direction * speed;

        // Smoothly interpolate velocity
        Vector2 smoothedVelocity = Vector2.SmoothDamp(rb.linearVelocity, desiredVelocity, ref velocityRef, smoothingTime);

        // Move the enemy
        rb.MovePosition(rb.position + smoothedVelocity * Time.fixedDeltaTime);

        // If we're close to the current waypoint, move to the next
        if (Vector2.Distance(currentPosition, targetPos) < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}
