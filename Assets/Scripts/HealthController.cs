using UnityEngine;
using UnityEngine.UI;

public enum HeartStatus { Full, Half, Empty }

public class HealthController : MonoBehaviour
{
    public Image heartImage;             // Assign your heart's Image component.
    public Sprite fullHeartSprite;
    public Sprite halfHeartSprite;
    public Sprite emptyHeartSprite;

    public void SetHeartImage(HeartStatus status)
    {
        switch (status)
        {
            case HeartStatus.Full:
                heartImage.sprite = fullHeartSprite;
                break;
            case HeartStatus.Half:
                heartImage.sprite = halfHeartSprite;
                break;
            case HeartStatus.Empty:
                heartImage.sprite = emptyHeartSprite;
                break;
        }
    }
}
