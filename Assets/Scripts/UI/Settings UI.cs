using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsUI : MonoBehaviour
{
    // Hands
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;
    private bool invertedDominentHand = false;

    public void SetHandTransforms(Transform left, Transform right) {
        leftHandTransform = left;
        rightHandTransform = right;
    }

    public void SwapDominentHand() {
        if (leftHandTransform == null || rightHandTransform == null) return;
        
        invertedDominentHand = !invertedDominentHand;

        transform.SetParent(invertedDominentHand ? rightHandTransform : leftHandTransform);
    }

    public void ResetLevel() {
        StartCoroutine(ResetLevelEnumerator());
    }
    
    private IEnumerator ResetLevelEnumerator() {
        FadeUI.Instance.Fade(true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
