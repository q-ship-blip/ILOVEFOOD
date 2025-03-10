using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    [SerializeField] private Animator anim;
    [SerializeField] private float meleeSpeed;
    [SerializeField] private float damage;

    private float timeUntilMelee;
    private bool isAttacking = false;  // Flag for when the player is attacking

    void Update() {
        if (timeUntilMelee <= 0f) {
            if (Input.GetMouseButtonDown(0)) {
                isAttacking = true;
                anim.SetTrigger("Attack");
                timeUntilMelee = meleeSpeed;
                // Clear the flag after a short duration; adjust as needed
                Invoke(nameof(EndAttack), 0.2f);
            }
        } else {
            timeUntilMelee -= Time.deltaTime;
        }
    }

    // Called after the attack is done to clear the flag
    void EndAttack() {
        isAttacking = false;
    }

    // Only apply damage when in an active attack state
    private void OnTriggerEnter2D(Collider2D other) {
        if (isAttacking && other.CompareTag("Enemy")) {
            other.GetComponent<Enemy>().TakeDamage(damage);
            Debug.Log("attacked");
        }
    }
}
