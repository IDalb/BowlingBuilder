using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsUI : MonoBehaviour
{
    public void ResetLevel() {
        StartCoroutine(ResetLevelEnumerator());
    }
    
    private IEnumerator ResetLevelEnumerator() {
        if (FadeUI.Instance != null)
        {
            FadeUI.Instance.Fade(true);
            yield return new WaitForSeconds(1f);
        }
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void ResetBallPosition() {
        if (FindAnyObjectByType<ResetBall>() == null) return;
        FindAnyObjectByType<ResetBall>().ResetBallPosition();
    }

    public void GoToThrowArea() {
        
    }

    public void ExitLevel()
    {
        StartCoroutine(ExitLevelEnumerator());
    }

    private IEnumerator ExitLevelEnumerator()
    {
        FadeUI.Instance.Fade(true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync("Main Menu");
    }
}
