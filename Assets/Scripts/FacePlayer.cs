using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    // Angle offset in degrees, settable in the Inspector.
    public float angleOffset = 0f;

    private Transform target;

    void Start()
    {
        // Automatically find the player using the tag "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            target = playerObj.transform;
        else
            Debug.LogWarning("FacePlayer: No GameObject found with tag 'Player'.");
    }

    void Update()
    {
        if (target == null)
            return;

        // Calculate the direction from this object to the player.
        Vector2 direction = target.position - transform.position;
        // Calculate the angle in degrees.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Rotate the object with the angle offset.
        transform.rotation = Quaternion.Euler(0f, 0f, angle + angleOffset);
    }
}
