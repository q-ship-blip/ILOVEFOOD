using UnityEngine;
using TMPro;
using System.Collections;

public class PickupNotificationUI : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public CanvasGroup canvasGroup;
    public float displayTime = 1.5f;
    public float fadeDuration = 1f;

    public void Initialize(string message)
    {
        textComponent.text = message;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(displayTime);

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = 1 - (t / fadeDuration);
            yield return null;
        }

        Destroy(gameObject);
    }
}
