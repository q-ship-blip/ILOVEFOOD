using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName;  // Set this in the inspector for each item type (e.g., "HealthPotion", "Coin", etc.)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();

            if (inventory != null)
            {
                inventory.AddItem(itemName);
                Destroy(gameObject);  // Item disappears after pickup
            }
            else
            {
                Debug.LogError("Player is missing PlayerInventory script!");
            }
        }
    }
}
