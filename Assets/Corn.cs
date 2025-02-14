using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private float health;

    public void TakeDamage(float damage) {
        health -= damage;
        Debug.Log(health);
        if (health <= 0f) {
            Destroy(gameObject);
            Debug.Log("Enemy died");
        }
    }
}
