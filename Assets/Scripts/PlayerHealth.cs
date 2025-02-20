using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health, maxHealth;
    private float Start {
        health = maxHealth;
    }
    
    public void TakeDamage(float amount) {
        health -= amount;
    }
}
