using System.Collections;
using UnityEngine;

public class CornSwordSwipe : MonoBehaviour
{
    public float swipeDuration = 0.2f;  // Duration of the swipe animation
    public float swipeAngle = 120f;     // Total angle to rotate during the swipe (0 to 120°)
    public float attackCooldown = 1f;   // Time between swipes

    private bool isSwiping = false;
    private bool canAttack = true;

    private EnemyPathfinding enemyPathfinding;
    private GameObject gfx;  // Reference to the child "GFX" that holds the sword visuals

    private void Awake()
    {
        Debug.Log("[CornSwordSwipe] Awake is running!");
    }

    private void Start()
    {
        enemyPathfinding = GetComponentInParent<EnemyPathfinding>();
        Debug.Log("[CornSwordSwipe] Start is running!");

        if (enemyPathfinding == null)
        {
            Debug.LogError("[CornSwordSwipe] No EnemyPathfinding script found on parent!");
        }

        // Find the child object named "GFX"
        Transform gfxTransform = transform.Find("GFX");
        if (gfxTransform != null)
        {
            gfx = gfxTransform.gameObject;
        }
        else
        {
            Debug.LogError("[CornSwordSwipe] GFX child not found!");
        }

        // Initially disable the GFX so the sword is not visible
        if (gfx != null)
            gfx.SetActive(false);
    }

    private void Update()
    {
        Debug.Log("[CornSwordSwipe] Update is running...");

        if (enemyPathfinding == null)
        {
            Debug.LogWarning("[CornSwordSwipe] No EnemyPathfinding reference! Skipping attack check.");
            return;
        }

        if (enemyPathfinding.AttackPosition)
        {
            Debug.Log("[CornSwordSwipe] Enemy is in attack range!");
        }
        else
        {
            Debug.Log("[CornSwordSwipe] Enemy is NOT in attack range.");
        }

        // Automatically attack if in range and ready
        if (enemyPathfinding.AttackPosition && canAttack && !isSwiping)
        {
            Debug.Log("[CornSwordSwipe] Performing attack...");
            PerformSwipe();
        }
    }

    public void PerformSwipe()
    {
        if (!canAttack)
        {
            Debug.Log("[CornSwordSwipe] Attack is on cooldown.");
            return;
        }

        Debug.Log("[CornSwordSwipe] Sword swipe started!");

        // Enable the GFX child to show the sword visuals
        if (gfx != null)
            gfx.SetActive(true);

        // Reset rotation to 0 before starting the swipe
        transform.localRotation = Quaternion.Euler(0, 0, 0);

        StartCoroutine(SwipeRoutine());
    }

    private IEnumerator SwipeRoutine()
    {
        isSwiping = true;
        canAttack = false;
        float elapsedTime = 0f;

        // Rotate from 0 to swipeAngle over swipeDuration
        while (elapsedTime < swipeDuration)
        {
            float t = elapsedTime / swipeDuration;
            float currentAngle = Mathf.Lerp(0, swipeAngle, t);
            transform.localRotation = Quaternion.Euler(0, 0, currentAngle);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("[CornSwordSwipe] Sword swipe completed.");

        // Reset rotation to 0 after swipe is complete
        transform.localRotation = Quaternion.Euler(0, 0, 0);

        // Disable the GFX child after the swipe
        if (gfx != null)
            gfx.SetActive(false);

        isSwiping = false;

        // Wait for cooldown before allowing next attack
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        Debug.Log("[CornSwordSwipe] Attack is ready again.");
    }
}
