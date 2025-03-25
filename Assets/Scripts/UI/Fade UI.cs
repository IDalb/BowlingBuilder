using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    // Singleton pattern
    private static FadeUI _instance;
    public static FadeUI Instance { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    [SerializeField] private Image fadeImage;

    public void Fade(bool active, float duration = 1f) {
        if (fadeImage == null) return;

        LeanTween.value(fadeImage.gameObject,
            (float a) => { fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, a); },
            active ? 0 : 1,
            active ? 1 : 0,
            duration
        );
    }
}
