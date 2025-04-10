using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [Header("Weapon Components")]
    public PlayerAttack playerAttack;
    public PlayerShield playerShield;

    [Header("Shield Unlock via Items")]
    public string shieldItemName = "Coin";
    public int shieldItemGoal = 1;

    [Header("Lock Objects")]
    public GameObject shieldLockObject;
    public GameObject swordLockObject;

    [Header("Item Collection Unlock (Optional Third Object)")]
    public string targetItemName = "Corn";
    public int itemPickupGoal = 5;
    public GameObject thirdLockObject;

    public PlayerInventory playerInventory;

    private bool shieldUnlocked = false;
    private bool swordUnlocked = false;
    private string activeScene => SceneManager.GetActiveScene().name;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        TryAssignReferencesIfTutorialFloor();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (activeScene != "TutorialFloor") return; // 💥 Only run on TutorialFloor

        if (!shieldUnlocked && playerInventory != null)
        {
            int shieldItemCount = playerInventory.GetItemCount(shieldItemName);
            if (shieldItemCount >= shieldItemGoal)
            {
                UnlockShield();
            }
        }

        if (!swordUnlocked && PeanutProjectile.blockedByShieldCount >= 3)
        {
            UnlockSword();
        }

        if (thirdLockObject != null && thirdLockObject.activeSelf && playerInventory != null)
        {
            int count = playerInventory.GetItemCount(targetItemName);
            if (count >= itemPickupGoal)
            {
                Debug.Log($"Collected enough {targetItemName} ({count}), disabling third lock object!");
                thirdLockObject.SetActive(false);
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryAssignReferencesIfTutorialFloor();
    }

    private void TryAssignReferencesIfTutorialFloor()
    {
        if (activeScene != "TutorialFloor") return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerAttack = player.GetComponent<PlayerAttack>();
            playerShield = player.GetComponent<PlayerShield>();
            playerInventory = player.GetComponent<PlayerInventory>();
        }

        // Find door objects in THIS scene only
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj.scene.name != activeScene) continue;

            switch (obj.name)
            {
                case "FirstDoor":
                    shieldLockObject = obj;
                    break;
                case "SecondDoor":
                    swordLockObject = obj;
                    break;
                case "ThirdDoor":
                    thirdLockObject = obj;
                    break;
            }
        }

        RecheckUnlocks();
    }

    private void RecheckUnlocks()
    {
        if (activeScene != "TutorialFloor") return;

        if (playerInventory == null) return;

        int shieldItemCount = playerInventory.GetItemCount(shieldItemName);
        if (shieldItemCount >= shieldItemGoal)
        {
            UnlockShield();
        }

        if (PeanutProjectile.blockedByShieldCount >= 3)
        {
            UnlockSword();
        }

        if (thirdLockObject != null && thirdLockObject.activeSelf)
        {
            int count = playerInventory.GetItemCount(targetItemName);
            if (count >= itemPickupGoal)
            {
                Debug.Log($"Re-check: disabling third lock object!");
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
