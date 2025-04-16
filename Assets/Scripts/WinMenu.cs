using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    float dt = 11;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string plural = "";
        if(GameObject.FindFirstObjectByType<GameManager>().GetComponent<GameManager>().scoreManager.GetThrowCount() > 1)
        {
            plural = "s";
        }
        dt -= Time.deltaTime;
        GameObject.FindGameObjectWithTag("WinMessage").GetComponent<TMP_Text>().text = "Vous avez gagné avec " + GameObject.FindFirstObjectByType<GameManager>().GetComponent<GameManager>().scoreManager.GetThrowCount() + " lancé" + plural + "\nProchain niveaux dans " + (int)dt + "s";
        GameObject winMenu = GameObject.FindGameObjectWithTag("WinMenu");
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");

        const float interpolationFactor = 0.01f;

        winMenu.transform.eulerAngles = new Vector3(
        winMenu.transform.eulerAngles.x,
        winMenu.transform.eulerAngles.y * (1 - interpolationFactor) + camera.transform.eulerAngles.y * interpolationFactor,
        winMenu.transform.eulerAngles.z
      ); 
        //        winMenu.transform.eulerAngles.x * 0.9f + camera.transform.eulerAngles.y * 0.1f + 90,

    }
}
