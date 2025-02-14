using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private float health;

    public void TakeDamage(float damage) {
        Debug.Log(gameObject.name + " took damage: " + damage); // Debug Log

        health -= damage;
        Debug.Log(gameObject.name + " health: " + health);

        if (health <= 0f) {
            Debug.Log(gameObject.name + " died!");
            Destroy(gameObject);
        }
    }
}
