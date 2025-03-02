using System.Collections.Generic;
using UnityEngine;

public class HeartBarController : MonoBehaviour
{
    public GameObject heartPrefab;   // Drag your heart prefab here in the Inspector.
    public PlayerHealth playerHealth;

    private List<HealthController> hearts = new List<HealthController>();

    public void CreateHearts(int maxHearts)
    {
        ClearHearts();

        for (int i = 0; i < maxHearts; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, transform);
            HealthController heartController = newHeart.GetComponent<HealthController>();

            if (heartController != null)
            {
                hearts.Add(heartController);
            }
            else
            {
                Debug.LogError("Heart prefab missing HealthController script!");
            }
        }
    }

    public void ClearHearts()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        hearts.Clear();
    }

    public void UpdateHearts()
    {
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth not assigned to HeartBarController!");
            return;
        }

        int health = playerHealth.currentHealth;

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartHealth = health - (i * 2);

            if (heartHealth >= 2)
            {
                hearts[i].SetHeartImage(HeartStatus.Full);
            }
            else if (heartHealth == 1)
            {
                hearts[i].SetHeartImage(HeartStatus.Half);
            }
            else
            {
                hearts[i].SetHeartImage(HeartStatus.Empty);
            }
        }
    }
}
