using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [Header("Weapon Components")]
    public PlayerAttack playerAttack;   // Reference to the sword script
    public PlayerShield playerShield;   // Reference to the shield script

    [Header("Lock Objects")]
    public GameObject shieldLockObject; // Object to disable when shield unlocks
    public GameObject swordLockObject;  // Object to disable when sword unlocks

    [Header("Optional Wall to Disable")]
    public GameObject someWall;         // Wall or barrier to disable when sword unlocks

    private bool shieldUnlocked = false;
    private bool swordUnlocked = false;
    private int blockedPeanuts = 0;

    void Start()
    {
        // Initially disable the weapon scripts
        if (playerAttack != null)
            playerAttack.enabled = false;
        if (playerShield != null)
            playerShield.enabled = false;
    }

    void Update()
    {
        // Unlock shield on any WASD input
        if (!shieldUnlocked && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                                 Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            UnlockShield();
        }
    }

    public void RegisterPeanutBlock()
    {
        // Only count blocks if the shield is unlocked
        if (!shieldUnlocked)
            return;

        blockedPeanuts++;
        Debug.Log("Peanut blocked! Count: " + blockedPeanuts);

        if (blockedPeanuts >= 3 && !swordUnlocked)
        {
            UnlockSword();
        }
    }

    private void UnlockShield()
    {
        shieldUnlocked = true;
        Debug.Log("Shield unlocked!");

        if (playerShield != null)
            playerShield.enabled = true;

        if (shieldLockObject != null)
            shieldLockObject.SetActive(false);
    }

    private void UnlockSword()
    {
        swordUnlocked = true;
        Debug.Log("UnlockSword() method called.");
        Debug.Log("Sword unlocked!");

        if (playerAttack != null)
            playerAttack.enabled = true;

        if (swordLockObject != null)
            swordLockObject.SetActive(false);

        if (someWall != null)
            someWall.SetActive(false);
    }
}
