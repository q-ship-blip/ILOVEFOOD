using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Tooltip("What should this item be counted as in the inventory?")]
    public string inventoryItemName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();

            if (inventory != null)
            {
                inventory.AddItem(inventoryItemName);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Player is missing PlayerInventory script!");
            }
        }
    }
}
