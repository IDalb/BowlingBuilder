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
        // message de victoire avec nb de lanc�s
        string plural = "";
        if(GameObject.FindFirstObjectByType<GameManager>().GetComponent<GameManager>().scoreManager.GetThrowCount() > 1)
        {
            plural = "s";
        }
        dt -= Time.deltaTime;
        GameObject.FindGameObjectWithTag("WinMessage").GetComponent<TMP_Text>().text = "Vous avez gagn� avec " + GameObject.FindFirstObjectByType<GameManager>().GetComponent<GameManager>().scoreManager.GetThrowCount() + " lanc�" + plural + "\nProchain niveau dans " + (int)dt + "s";
        
        GameObject winMenu = GameObject.FindGameObjectWithTag("WinMenu");
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");

        // Avoir l'ecran de victoire qui suit la camera
        const float interpolationFactor = 0.01f;

        winMenu.transform.eulerAngles = new Vector3(
        winMenu.transform.eulerAngles.x,
        winMenu.transform.eulerAngles.y * (1 - interpolationFactor) + camera.transform.eulerAngles.y * interpolationFactor,
        winMenu.transform.eulerAngles.z
      ); 

    }
}
