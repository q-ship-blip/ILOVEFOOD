using UnityEngine;

[System.Serializable]
public class DropItem
{
    public GameObject itemPrefab;
    [Range(0f, 1f)] public float dropChance;
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 100f;

    [Header("Item Drops")]
    public DropItem[] possibleDrops = new DropItem[5];

    private bool isDead = false;

    // Reference to the animator
    private SimpleAnimation spriteAnimator;  // Moved inside the class!

    void Start()
    {
        spriteAnimator = GetComponent<SimpleAnimation>();
        if (spriteAnimator == null)
        {
            Debug.LogError("SimpleAnimation component missing on " + gameObject.name);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        Debug.Log(gameObject.name + " took damage: " + damage);

        health -= damage;
        Debug.Log(gameObject.name + " health: " + health);

        if (health <= 0f)
        {
            Die();
        }
        else
        {
            spriteAnimator?.SetThirdState(0.2f);  // This will work if spriteAnimator was found in Start()
        }
    }

    private void Die()
    {
        if (isDead) return;

        Debug.Log(gameObject.name + " died!");
        isDead = true;

        DropLoot();
        Destroy(gameObject);
    }

    private void DropLoot()
{
    foreach (DropItem drop in possibleDrops)
    {
        if (drop.itemPrefab == null) continue;

        float chance = Random.Range(0f, 1f);
        if (chance <= drop.dropChance)
        {
            // Add slight random offset to the drop position
            Vector3 randomOffset = new Vector3(
                Random.Range(-0.5f, 0.5f),  // X-axis offset
                Random.Range(-0.5f, 0.5f),  // Y-axis offset
                0f                          // Z-axis (2D game)
            );

            Vector3 dropPosition = transform.position + randomOffset;

            Instantiate(drop.itemPrefab, dropPosition, Quaternion.identity);
        }
    }
}

}
