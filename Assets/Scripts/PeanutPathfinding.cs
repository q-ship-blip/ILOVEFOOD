using UnityEngine;
using Pathfinding;

public class PeanutPathfinding : MonoBehaviour
{
    [Header("Pathfinding Settings")]
    public float chaseRange = 10f;
    public float speed = 3f;
    public float nextWaypointDistance = 0.6f;
    public float pathUpdateInterval = 0.5f;
    public float attackStopRange = 5f;
    
    [Header("Smoothing Settings")]
    public float smoothingTime = 0.1f;
    
    public Vector2 movementOffset = new Vector2(0, -0.5f);
    public bool AttackPosition = false;

    // List any additional tags to ignore in the raycast (e.g., "Shield", "NonBlocking", etc.)
    [Tooltip("List of tags to ignore when determining line of sight.")]
    public string[] ignoredTags = new string[] { "Shield" };

    private Transform target;
    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    private Collider2D enemyCollider;
    private Rigidbody2D rb;
    private Vector2 velocityRef = Vector2.zero;

    // This mask currently ignores the "Enemy" layer.
    private int ignoreEnemyLayer;

    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }

        seeker = GetComponent<Seeker>();
        enemyCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        ignoreEnemyLayer = ~LayerMask.GetMask("Enemy");

        InvokeRepeating(nameof(UpdatePath), 0f, pathUpdateInterval);
    }

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

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Modified HasLineOfSight method that ignores any colliders with a tag listed in ignoredTags.
    bool HasLineOfSight()
    {
        Vector2 enemyCenter = GetEnemyCenter();
        Vector2 direction = ((Vector2)target.position - enemyCenter).normalized;
        float distance = Vector2.Distance(enemyCenter, target.position);
        Vector2 rayOrigin = enemyCenter + direction * 0.1f;

        RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, direction, distance - 0.1f, ignoreEnemyLayer);

        // Sort the hits by distance (closest first).
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit2D hit in hits)
        {
            // If the hit object's tag is in the ignoredTags list, skip it.
            foreach (string tag in ignoredTags)
            {
                if (hit.collider.CompareTag(tag))
                {
                    // Continue to next hit if this one should be ignored.
                    goto NextHit;
                }
            }

            // If the first non-ignored hit is the player, line of sight is clear.
            if (hit.collider.CompareTag("Player"))
                return true;

            // Otherwise, something is blocking the view.
            return false;

            NextHit: ; // Label used for skipping the rest of the loop iteration.
        }

        // If no blocking hit is found, assume clear line of sight.
        return true;
    }

    void FixedUpdate()
    {
        if (target == null)
            return;

        float distanceToTarget = Vector2.Distance(GetEnemyCenter(), target.position);

        if (distanceToTarget <= attackStopRange && HasLineOfSight())
        {
            AttackPosition = true;
            rb.linearVelocity = Vector2.zero;
            return;
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

        Vector2 targetPos = (Vector2)path.vectorPath[currentWaypoint];
        Vector2 moveDirection = (targetPos - currentPosition).normalized;
        Vector2 desiredVelocity = moveDirection * speed;
        Vector2 smoothedVelocity = Vector2.SmoothDamp(rb.linearVelocity, desiredVelocity, ref velocityRef, smoothingTime);

        rb.MovePosition(rb.position + smoothedVelocity * Time.fixedDeltaTime);

        if (Vector2.Distance(currentPosition, targetPos) < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}
