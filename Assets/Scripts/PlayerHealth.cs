using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;     // Total health in **half hearts**.
    public int currentHealth;      // Current health in **half hearts**.

    public HeartBarController heartBar;

    void Start()
    {
        Debug.Log("Max Health: " + maxHealth);
        currentHealth = maxHealth;

        // Make sure heartBar isn't null
        if (heartBar != null)
        {
            heartBar.CreateHearts(maxHealth / 2);   // Divide by 2 since each heart is 2 health
            heartBar.UpdateHearts();
        }
        else
        {
            Debug.LogError("HeartBarController is not assigned to PlayerHealth!");
        }
    } 
    void Update(){
        if (Input.GetKeyDown(KeyCode.Space)){
            TakeDamage(1);  // Lose 1 half-heart per press
            Debug.Log("Took Damage! Current Health: " + currentHealth);
        }   
        if (Input.GetKeyDown(KeyCode.H)) {
        Heal(1);  // Lose 1 half-heart per press
            Debug.Log("Healed! Current Health: " + currentHealth);
        }
    }

    public void TakeDamage(int halfHearts){
        currentHealth -= halfHearts;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth < 1)  // Health is 0 or less
        {
            OnPlayerDeath();
        }
        // Update hearts after taking damage
        heartBar.UpdateHearts();
    }

    // Optional healing method
    public void Heal(int halfHearts){
        currentHealth += halfHearts;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        heartBar.UpdateHearts();
    }
    private void OnPlayerDeath()
    {
        // Change to the "GameOver" scene or whichever scene you want.
       // SceneManager.LoadScene("GameOver");  // Make sure "GameOver" exists in your Build Settings!
       Debug.Log(gameObject.name + " died!");
       Destroy(gameObject);
    }    
}
