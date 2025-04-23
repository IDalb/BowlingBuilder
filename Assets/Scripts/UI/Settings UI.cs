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
#if UNITY_EDITOR
        GameObject telemetry = GameObject.FindGameObjectWithTag("Telemetry");
        if (telemetry)
        {
            //telemetry.GetComponent<Telemetry>().SaveData();
        }
#endif
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void ResetBallPosition() {
        if (FindAnyObjectByType<ResetBall>() == null) return;
        
        GameManager[] gameManagers = FindObjectsByType<GameManager>(FindObjectsSortMode.None);
        foreach (GameManager gameManager in gameManagers)
        {
            gameManager.ResetBallPosition();
            gameManager.RemoveFallenPins();
        }
    }

    public void GoToThrowArea() {
        
    }

    public void ExitLevel()
    {
        StartCoroutine(ExitLevelEnumerator());
    }

    private IEnumerator ExitLevelEnumerator()
    {
#if UNITY_EDITOR
        GameObject telemetry = GameObject.FindGameObjectWithTag("Telemetry");
        if (telemetry)
        {
            //telemetry.GetComponent<Telemetry>().SaveData();
        }
#endif
        FadeUI.Instance.Fade(true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync("Main Menu");
    }
}
