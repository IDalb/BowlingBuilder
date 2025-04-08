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
        dt -= Time.deltaTime;
        GameObject.FindGameObjectWithTag("WinMessage").GetComponent<TMP_Text>().text = "You won with " + GameObject.FindFirstObjectByType<GameManager>().GetComponent<GameManager>().scoreManager.GetThrowCount() + " throw\nNext level in " + (int)dt + "s";
        GameObject winMenu = GameObject.FindGameObjectWithTag("WinMenu");
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");

        //winMenu.transform.parent.eulerAngles = new Vector3(
        //winMenu.transform.eulerAngles.x,
        //camera.transform.eulerAngles.y + 90,
        //winMenu.transform.eulerAngles.z
    //); 
    }
}
