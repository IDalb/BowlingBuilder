using UnityEngine;

public class ItemDrawerItem : MonoBehaviour
{
    private void OnEnable()
    {
        float initialZ = transform.position.z;
        LeanTween.moveLocalZ(gameObject, initialZ - 0.35f, 1f).setEaseInOutSine().setLoopPingPong();
    }
}
