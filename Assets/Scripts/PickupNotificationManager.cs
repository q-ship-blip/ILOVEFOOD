using UnityEngine;

public class PickupNotificationManager : MonoBehaviour
{
    public static PickupNotificationManager Instance;

    public GameObject notificationPrefab;
    public Transform notificationParent;

private void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    else
    {
        Destroy(gameObject);
    }
}


    public void ShowNotification(string message)
    {
        GameObject notifGO = Instantiate(notificationPrefab, notificationParent);
        var notifUI = notifGO.GetComponent<PickupNotificationUI>();
        notifUI.Initialize(message);
    }
}
