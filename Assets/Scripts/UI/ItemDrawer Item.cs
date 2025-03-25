using UnityEngine;

public class ItemDrawerItem : MonoBehaviour
{
    private void OnEnable()
    {
        LeanTween.rotateZ(gameObject, 360, 2f).setLoopClamp();
        float initialY = transform.position.y;
        LeanTween.moveLocalZ(gameObject, initialY - 600f, 0.75f).setEaseInOutCirc().setLoopPingPong();
    }
}
