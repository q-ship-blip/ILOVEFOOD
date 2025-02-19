using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour{
    public Sprite fullHeart, halfHeart, emptyHeart;
    Image heartImage;

    private void Awake(){
        heartImage = GetComponent<Image>();
    }
}
