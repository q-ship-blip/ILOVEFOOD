using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class InventoryEntry
{
    public string itemName;
    public int quantity;
    public GameObject itemPrefab;
}

public class PlayerInventory : MonoBehaviour
{
    [Header("Set starting items here")]
    public List<InventoryEntry> startingInventory = new List<InventoryEntry>();

    // Internal dictionary for fast access
    private Dictionary<string, InventoryEntry> inventoryDict = new Dictionary<string, InventoryEntry>();

    private void Awake()
    {
        // Build dictionary from the serialized list
        foreach (var entry in startingInventory)
        {
            if (!inventoryDict.ContainsKey(entry.itemName))
            {
                inventoryDict[entry.itemName] = entry;
            }
        }
    }

    public void AddItem(string itemName)
    {
        if (inventoryDict.ContainsKey(itemName))
        {
            inventoryDict[itemName].quantity++;
        }
        else if (startingInventory.Count < 10)
        {
            InventoryEntry newEntry = new InventoryEntry
            {
                itemName = itemName,
                quantity = 1,
                itemPrefab = null  // You can later assign this in UI if needed
            };
            startingInventory.Add(newEntry);
            inventoryDict[itemName] = newEntry;
        }
        else
        {
            Debug.Log("Inventory full! Can't carry more types of items.");
        }

        Debug.Log($"Picked up: {itemName}. Total: {inventoryDict[itemName].quantity}");
    }

    public int GetItemCount(string itemName)
    {
        return inventoryDict.ContainsKey(itemName) ? inventoryDict[itemName].quantity : 0;
    }

    public void PrintInventory()
    {
        Debug.Log("Player Inventory:");
        foreach (var entry in startingInventory)
        {
            Debug.Log($"{entry.itemName}: {entry.quantity}");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            PrintInventory();
        }
    }
}
