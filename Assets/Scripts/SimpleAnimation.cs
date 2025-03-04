using UnityEngine;
using System.Collections;

public class SimpleAnimation : MonoBehaviour
{
    public Sprite sprite1;  // First frame
    public Sprite sprite2;  // Second frame
    public Sprite sprite3;  // Third state sprite

    public float frameRate = 0.5f; // Seconds between frame swaps (0.5 = 2 frames per second)

    private SpriteRenderer spriteRenderer;
    private float timer;
    private bool showingSprite1 = true;
    private bool isInThirdState = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SimpleSpriteAnimator needs a SpriteRenderer on the same object!");
            enabled = false; // Disable this script if no sprite renderer
            return;
        }

        spriteRenderer.sprite = sprite1;  // Start with sprite1
    }

    void Update()
    {
        if (isInThirdState) return; // Skip animation if in third state

        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer = 0f;
            showingSprite1 = !showingSprite1;  // Toggle between sprite1 and sprite2
            spriteRenderer.sprite = showingSprite1 ? sprite1 : sprite2;
        }
    }

    // Call this method to switch to the third state for a set duration
    public void SetThirdState(float duration)
    {
        if (!isInThirdState) // Prevent overlapping calls
        {
            StartCoroutine(ThirdStateCoroutine(duration));
        }
    }

    private IEnumerator ThirdStateCoroutine(float duration)
    {
        isInThirdState = true;
        spriteRenderer.sprite = sprite3;

        yield return new WaitForSeconds(duration);

        isInThirdState = false;
        timer = 0f; // Reset timer to ensure smooth transition back to animation
        showingSprite1 = true;
        spriteRenderer.sprite = sprite1;
    }
}
