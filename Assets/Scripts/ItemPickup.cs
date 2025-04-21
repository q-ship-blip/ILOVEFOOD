using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Tooltip("What should this item be counted as in the inventory?")]
    public string inventoryItemName;
    public int healAmt;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            PlayerHealth health = other.GetComponent<PlayerHealth>();

            if (inventory != null)
            {
                inventory.AddItem(inventoryItemName);

                if (health != null)
                    health.Heal(healAmt);  // heals 1 full heart

                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Player is missing PlayerInventory script!");
            }
        }
        
    }
}
