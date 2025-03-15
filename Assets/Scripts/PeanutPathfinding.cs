using UnityEngine;
using Pathfinding;

public class PeanutPathfinding : MonoBehaviour
{
    [Header("Pathfinding Settings")]
    // Distance within which the enemy will start chasing the player.
    public float chaseRange = 10f;
    // Speed at which the enemy moves.
    public float speed = 3f;
    // Distance to consider a waypoint reached.
    public float nextWaypointDistance = 0.6f;
    // How often (in seconds) to update the path.
    public float pathUpdateInterval = 0.5f;
    // When within this range (5 units) and with clear sight, stop and attack.
    public float attackStopRange = 5f;
    
    [Header("Smoothing Settings")]
    // How quickly the enemy accelerates/decelerates.
    public float smoothingTime = 0.1f;
    
    // Offset to adjust the enemy’s effective movement center.
    public Vector2 movementOffset = new Vector2(0, -0.5f);

    // This is set to true when the enemy is in attack position.
    public bool AttackPosition = false;

    private Transform target;     // Reference to the player's transform.
    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    private Collider2D enemyCollider;
    private Rigidbody2D rb;
    private Vector2 velocityRef = Vector2.zero;

    // Layer mask to ignore the enemy's own layer (update the layer name as needed)
    private int ignoreEnemyLayer;

    void Start()
    {
        // Find the player using the "Player" tag.
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }

        seeker = GetComponent<Seeker>();
        enemyCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        // Set the layer mask to ignore the "Enemy" layer.
        ignoreEnemyLayer = ~LayerMask.GetMask("Enemy");

        // Start updating the path at regular intervals.
        InvokeRepeating(nameof(UpdatePath), 0f, pathUpdateInterval);
    }

    // Calculate the enemy's effective center.
    Vector2 GetEnemyCenter()
    {
        if (enemyCollider != null)
            return (Vector2)enemyCollider.bounds.center + movementOffset;
        return (Vector2)transform.position + movementOffset;
    }

    // Update the path to the player if within chase range and not in attack condition.
    void UpdatePath()
    {
        if (target != null &&
            Vector2.Distance(GetEnemyCenter(), target.position) <= chaseRange &&
            !(Vector2.Distance(GetEnemyCenter(), target.position) <= attackStopRange && HasLineOfSight()))
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(GetEnemyCenter(), target.position, OnPathComplete);
            }
        }
        else
        {
            path = null;
        }
    }

    // Callback for when the path has been calculated.
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Checks whether there is a direct, unobstructed line of sight to the player.
    bool HasLineOfSight()
    {
        Vector2 enemyCenter = GetEnemyCenter();
        Vector2 direction = ((Vector2)target.position - enemyCenter).normalized;
        float distance = Vector2.Distance(enemyCenter, target.position);

        // Offset the starting point slightly to avoid hitting the enemy's own collider.
        Vector2 rayOrigin = enemyCenter + direction * 0.1f;
        
        // Perform a raycast from the enemy to the player, ignoring the enemy's own layer.
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, distance - 0.1f, ignoreEnemyLayer);

        // If the raycast hit something and it is the player, then there is a clear line of sight.
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }

    void FixedUpdate()
    {
        if (target == null)
            return;

        float distanceToTarget = Vector2.Distance(GetEnemyCenter(), target.position);

        // If within attack range (5 units) and with a clear line of sight, stop moving and set AttackPosition true.
        if (distanceToTarget <= attackStopRange && HasLineOfSight())
        {
            AttackPosition = true;
            rb.linearVelocity = Vector2.zero; // Stop movement.
            return;
        }
        else
        {
            AttackPosition = false;
        }

        if (path == null)
            return;

        Vector2 currentPosition = GetEnemyCenter();

        // Check if the end of the path is reached.
        if (currentWaypoint >= path.vectorPath.Count)
            return;

        // Move towards the current waypoint.
        Vector2 targetPos = (Vector2)path.vectorPath[currentWaypoint];
        Vector2 direction = (targetPos - currentPosition).normalized;
        Vector2 desiredVelocity = direction * speed;
        Vector2 smoothedVelocity = Vector2.SmoothDamp(rb.linearVelocity, desiredVelocity, ref velocityRef, smoothingTime);

        rb.MovePosition(rb.position + smoothedVelocity * Time.fixedDeltaTime);

        // When close enough to the waypoint, proceed to the next one.
        if (Vector2.Distance(currentPosition, targetPos) < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}
