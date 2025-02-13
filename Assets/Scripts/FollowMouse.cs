using UnityEngine;

public class FaceMouse : MonoBehaviour
{
    void Update()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction from the sprite to the mouse
        Vector3 direction = mousePosition - transform.position;
        direction.z = 0; // Ensure no unwanted rotation in the Z axis

        // Calculate the angle and apply rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 225;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
