using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [Tooltip("Reference to the shield object that will be enabled/disabled and rotated normally.")]
    public Transform shieldTransform;

    [Tooltip("Reference to the object that will be flipped (rotated 180°) when within a specific angle range.")]
    public Transform flipTransform;

    [Tooltip("Movement speed while the shield is active.")]
    public float shieldSpeed = 2f;

    [Tooltip("Rotation offset (in degrees) to adjust the shield's rotation relative to the mouse.")]
    public float shieldRotationOffset = 0f;

    [Header("Shield Flip Settings")]
    [Tooltip("Enable or disable automatic flip on the flipTransform.")]
    public bool enableFlip = true;
    [Tooltip("Minimum angle (in degrees) for triggering the flip on flipTransform.")]
    public float flipMinAngle = 90f;
    [Tooltip("Maximum angle (in degrees) for triggering the flip on flipTransform.")]
    public float flipMaxAngle = 270f;

    // Toggle for enabling/disabling shield functionality
    public bool enableShield = true;

    private PlayerMovement playerMovement;
    private float originalSpeed;
    private bool shieldActive = false;

    void Start()
    {
        // Get the PlayerMovement component from the player.
        playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            originalSpeed = playerMovement.moveSpeed;
        }
        else
        {
            Debug.LogWarning("PlayerShield: No PlayerMovement component found on the player.");
        }

        // Ensure the shield is disabled at the start.
        if (shieldTransform != null)
        {
            shieldTransform.gameObject.SetActive(false);
        }
        // Optionally, disable the flipTransform as well if needed.
        if (flipTransform != null)
        {
            flipTransform.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // If shield functionality is disabled, ensure the shield is off.
        if (!enableShield)
        {
            if (shieldActive)
                DeactivateShield();
            return;
        }
        
        // Activate shield on right-click press.
        if (Input.GetMouseButtonDown(1))
        {
            ActivateShield();
        }
        
        // Deactivate shield on right-click release.
        if (Input.GetMouseButtonUp(1))
        {
            DeactivateShield();
        }

        // While shield is active, update rotations.
        if (shieldActive)
        {
            // Get mouse position in world space.
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            Vector2 direction = mousePos - transform.position;
            float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            // Calculate the final angle with offset.
            float finalAngle = NormalizeAngle(baseAngle + shieldRotationOffset);

            // Set shieldTransform rotation normally.
            if (shieldTransform != null)
            {
                shieldTransform.rotation = Quaternion.Euler(0f, 0f, finalAngle);
            }
            
            // If a flipTransform is assigned, apply flip logic.
            if (flipTransform != null)
            {
                float flipAngle = finalAngle;
                if (enableFlip && flipAngle >= flipMinAngle && flipAngle <= flipMaxAngle)
                {
                    flipAngle += 180f;
                    flipAngle = NormalizeAngle(flipAngle);
                }
                flipTransform.rotation = Quaternion.Euler(0f, 0f, flipAngle);
            }
        }
    }

    void ActivateShield()
    {
        shieldActive = true;
        if (shieldTransform != null)
            shieldTransform.gameObject.SetActive(true);
        if (flipTransform != null)
            flipTransform.gameObject.SetActive(true);
        if (playerMovement != null)
            playerMovement.moveSpeed = shieldSpeed;
    }

    void DeactivateShield()
    {
        shieldActive = false;
        if (shieldTransform != null)
            shieldTransform.gameObject.SetActive(false);
        if (flipTransform != null)
            flipTransform.gameObject.SetActive(false);
        if (playerMovement != null)
            playerMovement.moveSpeed = originalSpeed;
    }

    // Helper method to normalize an angle between 0 and 360 degrees.
    private float NormalizeAngle(float angle)
    {
        angle = angle % 360f;
        if (angle < 0)
            angle += 360f;
        return angle;
    }
}
