using UnityEngine;

public class BurgerShooter : MonoBehaviour
{
    [Tooltip("List of burger projectile prefabs to choose from randomly.")]
    public GameObject[] burgerProjectilePrefabs;

    [Tooltip("Delay between shots in seconds.")]
    public float shotDelay = 1f;

    private float shotTimer;
    private PeanutPathfinding PeanutPathfinding;

    void Start()
    {
        shotTimer = shotDelay;
        PeanutPathfinding = GetComponentInParent<PeanutPathfinding>();
        if (PeanutPathfinding == null)
        {
            Debug.LogWarning("BurgerShooter: No PeanutPathfinding component found in parent. The shooter will fire regardless of attack position.");
        }
    }

    void Update()
    {
        bool isInAttackPosition = PeanutPathfinding ? PeanutPathfinding.AttackPosition : true;
        if (isInAttackPosition)
        {
            shotTimer -= Time.deltaTime;
            if (shotTimer <= 0f)
            {
                ShootRandomBurger();
                shotTimer = shotDelay;
            }
        }
    }

    void ShootRandomBurger()
    {
        if (burgerProjectilePrefabs != null && burgerProjectilePrefabs.Length > 0)
        {
            int index = Random.Range(0, burgerProjectilePrefabs.Length);
            GameObject selectedPrefab = burgerProjectilePrefabs[index];

            if (selectedPrefab != null)
            {
                Instantiate(selectedPrefab, transform.position, transform.rotation);
            }
            else
            {
                Debug.LogWarning("BurgerShooter: Selected projectile prefab is null.");
            }
        }
        else
        {
            Debug.LogWarning("BurgerShooter: No projectile prefabs assigned in the Inspector.");
        }
    }
}
