using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBarController : MonoBehaviour
{
    public GameObject heartPrefab;
    public PlayerHealth playerHealth;

    List<HealthController> hearts = new List<HealthController>();

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
    }

    public void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthController>();

    }
}
