using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Tooltip("Reference to the child object 'Sword' that will be rotated.")]
    public Transform swordTransform;
    
    [Tooltip("Total duration of the swipe in seconds.")]
    public float swipeDuration = 0.2f;
    
    [Tooltip("Cooldown time after a swipe before another can be initiated.")]
    public float swipeCooldown = 0.5f;
    
    [Tooltip("Rotation offset (in degrees) to adjust the swipe direction.")]
    public float rotationOffset = 0f;

    // Flag to prevent overlapping swipes
    private bool isSwiping = false;

    // Toggle for enabling/disabling attacks
    public bool enableAttack = true;
    
    void Update()
    {
        // Check if attacking is enabled
        if (!enableAttack)
            return;

        // Start attack on left mouse button click if not already swiping.
        if (Input.GetMouseButtonDown(0) && !isSwiping)
        {
            StartCoroutine(SwipeCoroutine());
        }
    }
    
    IEnumerator SwipeCoroutine()
    {
        isSwiping = true;
        
        // Capture the mouse position in world space once at the start.
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        
        // Calculate the base angle from the player's position to the mouse position.
        Vector2 direction = mousePos - transform.position;
        float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Set up the swipe: starting 80° behind and ending 80° ahead of the base angle.
        float startAngle = baseAngle - 80f;
        float endAngle = baseAngle + 80f;
        
        // Set the sword's rotation to the start angle first,
        // then enable the sword so that the trail begins with the correct angle.
        if (swordTransform != null)
        {
            swordTransform.localRotation = Quaternion.Euler(0f, 0f, startAngle + rotationOffset);
            swordTransform.gameObject.SetActive(true);
        }
        
        float elapsed = 0f;
        while (elapsed < swipeDuration)
        {
            // Interpolate between the start and end angles.
            float t = elapsed / swipeDuration;
            float currentAngle = Mathf.Lerp(startAngle, endAngle, t) + rotationOffset;
            
            if (swordTransform != null)
                swordTransform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        // Ensure the sword ends at the final angle (with offset).
        if (swordTransform != null)
            swordTransform.localRotation = Quaternion.Euler(0f, 0f, endAngle + rotationOffset);
        
        // Immediately disable the sword after the swipe.
        if (swordTransform != null)
            swordTransform.gameObject.SetActive(false);
        
        // Wait for the cooldown period before allowing another swipe.
        yield return new WaitForSeconds(swipeCooldown);
        
        isSwiping = false;
    }
}
