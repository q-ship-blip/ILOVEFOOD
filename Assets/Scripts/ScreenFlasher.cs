using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFlasher : MonoBehaviour
{
    [Header("Flash Settings")]
    public Color damageFlashColor = new Color(1, 0, 0, 0.5f);  // Default: semi-transparent red
    public Color healFlashColor = new Color(1, 1, 0, 0.5f);    // Default: semi-transparent yellow
    public float flashDuration = 0.2f;

    private Image flashImage;
    private Coroutine flashRoutine;

    void Awake()
    {
        flashImage = GetComponent<Image>();
        flashImage.color = Color.clear;
    }

    public void FlashDamage()
    {
        StartFlash(damageFlashColor);
    }

    public void FlashHeal()
    {
        StartFlash(healFlashColor);
    }

    private void StartFlash(Color flashColor)
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine(flashColor));
    }

    private IEnumerator FlashRoutine(Color targetColor)
    {
        flashImage.color = targetColor;
        yield return new WaitForSeconds(flashDuration);
        flashImage.color = Color.clear;
        flashRoutine = null;
    }
}
