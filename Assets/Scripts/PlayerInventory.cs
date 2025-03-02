using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    // This dictionary will hold item names and the count of how many the player has.
    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    // Call this when player picks up an item
    public void AddItem(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName]++;
        }
        else
        {
            if (inventory.Count < 10)  // Limit to 10 item types
            {
                inventory[itemName] = 1;
            }
            else
            {
                Debug.Log("Inventory full! Can't carry more types of items.");
            }
        }

        Debug.Log($"Picked up: {itemName}. Total: {inventory[itemName]}");
    }

    // Optional - for checking the count of a specific item
    public int GetItemCount(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            return inventory[itemName];
        }
        return 0;
    }

    // Debug method to show all items
    public void PrintInventory()
    {
        Debug.Log("Player Inventory:");
        foreach (var item in inventory)
        {
            Debug.Log($"{item.Key}: {item.Value}");
        }
    }
        void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
        PrintInventory();
        }
    }
}

