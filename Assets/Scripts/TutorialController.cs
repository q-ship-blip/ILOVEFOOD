using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [Header("Weapon Components")]
    public PlayerAttack playerAttack;   // Reference to the sword script
    public PlayerShield playerShield;   // Reference to the shield script

    [Header("Shield Unlock via Items")]
    [Tooltip("Name of the item needed to unlock the shield (must match ItemPickup.itemName).")]
    public string shieldItemName = "Coin";  // Default to "Coin"
    [Tooltip("Number of items required to unlock the shield.")]
    public int shieldItemGoal = 1;          // Default to 1

    [Header("Lock Objects")]
    [Tooltip("Object to disable when shield unlocks (e.g., a shield lock icon).")]
    public GameObject shieldLockObject;

    [Header("Sword Unlock via Peanut Blocks")]
    [Tooltip("Object to disable when sword unlocks (e.g., a sword lock icon).")]
    public GameObject swordLockObject;

    [Header("Item Collection Unlock (Optional Third Object)")]
    [Tooltip("Name of the item to track (must match ItemPickup.itemName).")]
    public string targetItemName;
    [Tooltip("Number of items required to unlock/disable the third object.")]
    public int itemPickupGoal = 5;
    [Tooltip("Third object to disable when enough items are collected.")]
    public GameObject thirdLockObject;

    [Tooltip("Reference to the player's inventory.")]
    public PlayerInventory playerInventory;

    private bool shieldUnlocked = false;
    private bool swordUnlocked = false;

    void Start()
    {
        // Initially disable the weapon scripts.
        if (playerAttack != null)
            playerAttack.enabled = false;
        if (playerShield != null)
            playerShield.enabled = false;
    }

    void Update()
    {
        // 1) Check if we can unlock the shield by picking up enough of "shieldItemName".
        if (!shieldUnlocked && playerInventory != null)
        {
            int shieldItemCount = playerInventory.GetItemCount(shieldItemName);
            if (shieldItemCount >= shieldItemGoal)
            {
                UnlockShield();
            }
        }

        // 2) Unlock sword after 3 peanuts are blocked.
        if (!swordUnlocked && PeanutProjectile.blockedByShieldCount >= 3)
        {
            UnlockSword();
        }

        // 3) Optionally disable a third object after collecting enough of "targetItemName".
        if (playerInventory != null && thirdLockObject != null && thirdLockObject.activeSelf)
        {
            int count = playerInventory.GetItemCount(targetItemName);
            if (count >= itemPickupGoal)
            {
                Debug.Log($"Collected enough {targetItemName} ({count}), disabling third lock object!");
                thirdLockObject.SetActive(false);
            }
        }
    }

    private void UnlockShield()
    {
        shieldUnlocked = true;
        Debug.Log("Shield unlocked by item pickup!");

        if (playerShield != null)
            playerShield.enabled = true;

        if (shieldLockObject != null)
            shieldLockObject.SetActive(false);
    }

    private void UnlockSword()
    {
        swordUnlocked = true;
        Debug.Log("Sword unlocked after blocking peanuts!");

        if (playerAttack != null)
            playerAttack.enabled = true;

        if (swordLockObject != null)
            swordLockObject.SetActive(false);
    }
}
