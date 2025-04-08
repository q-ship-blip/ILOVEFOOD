using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [Header("Weapon Components")]
    public PlayerAttack playerAttack;   // Reference to the sword script
    public PlayerShield playerShield;   // Reference to the shield script

    [Header("Shield Unlock via Items")]
    [Tooltip("Name of the item needed to unlock the shield (must match ItemPickup.inventoryItemName).")]
    public string shieldItemName = "Coin";
    [Tooltip("Number of items required to unlock the shield.")]
    public int shieldItemGoal = 1;

    [Header("Lock Objects")]
    public GameObject shieldLockObject;
    public GameObject swordLockObject;

    [Header("Item Collection Unlock (Optional Third Object)")]
    public string targetItemName;
    public int itemPickupGoal = 5;
    public GameObject thirdLockObject;

    [Tooltip("Reference to the player's inventory.")]
    public PlayerInventory playerInventory;

    private bool shieldUnlocked = false;
    private bool swordUnlocked = false;

    void Start()
    {
        if (playerAttack != null) playerAttack.enabled = false;
        if (playerShield != null) playerShield.enabled = false;
    }

    void Update()
    {
        // 1) Unlock Shield
        if (!shieldUnlocked && playerInventory != null)
        {
            int shieldItemCount = playerInventory.GetItemCount(shieldItemName);
            if (shieldItemCount >= shieldItemGoal)
            {
                UnlockShield();
            }
        }

        // 2) Unlock Sword after blocking 3 peanuts
        if (!swordUnlocked && PeanutProjectile.blockedByShieldCount >= 3)
        {
            UnlockSword();
        }

        // 3) Optional third lock object
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

        if (playerShield != null) playerShield.enabled = true;
        if (shieldLockObject != null) shieldLockObject.SetActive(false);
    }

    private void UnlockSword()
    {
        swordUnlocked = true;
        Debug.Log("Sword unlocked after blocking peanuts!");

        if (playerAttack != null) playerAttack.enabled = true;
        if (swordLockObject != null) swordLockObject.SetActive(false);
    }
}
