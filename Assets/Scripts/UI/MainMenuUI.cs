using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void LoadLevel(string name)
    {
#if UNITY_EDITOR
        GameObject telemetry = GameObject.FindGameObjectWithTag("Telemetry");
        if (telemetry)
        {
            telemetry.GetComponent<Telemetry>().SaveData();
        }
#endif
        SceneManager.LoadScene(name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
