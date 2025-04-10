using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Movement speed in units per second.")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        // Get the Rigidbody2D component attached to the player.
        rb = GetComponent<Rigidbody2D>();
        if(rb == null)
        {
            Debug.LogError("PlayerMovement requires a Rigidbody2D component.");
        }
       
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("Spawn");
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position;
        }
        else
        {
            Debug.LogWarning("No Spawn point found in this scene!");
        }
    }

    void Update()
    {
        // Read input from both WASD and arrow keys.
        // Input.GetAxisRaw returns -1, 0, or 1 so diagonal movement won't be faster.
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        // Normalize the movement vector to avoid faster diagonal movement.
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        // Move the player using Rigidbody2D.MovePosition for smooth movement.
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
