using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health, maxHealth;

    public void TakeDamage(float amount) {
        health -= amount;
    }
}
