using UnityEngine;
using Pathfinding;

public class EnemyPathfinding : MonoBehaviour
{
    [Header("Pathfinding Settings")]
    public float chaseRange = 5f;             // Distance within which the enemy chases the player.
    public float speed = 3f;                  // Base movement speed.
    public float nextWaypointDistance = 0.6f;   // Distance to consider a waypoint reached.
    public float pathUpdateInterval = 0.5f;     // How often to update the path.
    public float attackStopRange = 1f;          // Distance at which enemy stops to attack.

    [Header("Smoothing Settings")]
    public float smoothingTime = 0.1f;          // How quickly the enemy accelerates/decelerates.

    // Offset to adjust the effective movement pivot (set in Inspector)
    public Vector2 movementOffset = new Vector2(0, -0.5f);
    
    // This is set to true when the enemy is in attack position.
    public bool AttackPosition = false;
    
    private Transform target;                 // The player's transform.
    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    private Collider2D enemyCollider;
    private Rigidbody2D rb;                   // For physics-based movement.
    private Vector2 velocityRef = Vector2.zero; // Used for SmoothDamp.

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

    // Returns the enemy's effective center using its collider plus the offset.
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
            path = null;  // Clear the path if target is out of chase range or within attack range.
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
        
        float distanceToTarget = Vector2.Distance(GetEnemyCenter(), target.position);

        // If within attack range, set AttackPosition true and stop movement.
        if (distanceToTarget <= attackStopRange)
        {
            AttackPosition = true;
            rb.linearVelocity = Vector2.zero; // Stop moving
            return;
        }
        else
        {
            AttackPosition = false;
        }

        if (path == null)
            return;

        Vector2 currentPosition = GetEnemyCenter();

        // Check if we've reached the end of the path.
        if (currentWaypoint >= path.vectorPath.Count)
            return;

        // Calculate the target position from the current waypoint.
        Vector2 targetPos = (Vector2)path.vectorPath[currentWaypoint];
        Vector2 direction = (targetPos - currentPosition).normalized;
        Vector2 desiredVelocity = direction * speed;

        // Smoothly interpolate velocity.
        Vector2 smoothedVelocity = Vector2.SmoothDamp(rb.linearVelocity, desiredVelocity, ref velocityRef, smoothingTime);

        // Use MovePosition for physics-based movement.
        rb.MovePosition(rb.position + smoothedVelocity * Time.fixedDeltaTime);

        // If close enough to the waypoint, move to the next one.
        if (Vector2.Distance(currentPosition, targetPos) < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}
