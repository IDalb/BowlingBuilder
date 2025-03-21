using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GameManager : MonoBehaviour
{

    
    public GameObject ball;

    public GameObject platform;
    public Material neutralMaterialRef;
    public Material highlightedMaterialRef;

    private string playerTag = "MainCamera";

    ScoreManager scoreManager;

    private bool isInLaunchZone = false; 

    //private int fallenPins = 0;
    //private int throwsNb = 3;

    private GameObject[] pins;


    void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();

        GameObject[] pins = GameObject.FindGameObjectsWithTag("Pin");
        scoreManager.setTotalPinsNb(pins.Length);

        ball.GetComponent<XRGrabInteractable>().selectEntered.AddListener(Grab);
        ball.GetComponent<XRGrabInteractable>().selectExited.AddListener(Release);

    }

    

    public void PinFallen()
    {
        // Augmente le compteur des quilles tombées
        //fallenPins++;
        scoreManager.IncreaseFallenPinsNb();

    }

    private void Release(SelectExitEventArgs arg0)
    {
        ball.GetComponent<Renderer>().material = neutralMaterialRef;

        if (isInLaunchZone)
        {
            //throwsNb++;
            scoreManager.IncreaseThrowNumber();
        }
        else
        {
            //TODO bloquer quilles ou autre
            Debug.Log("Lancer invalide");
        }
    }

    private void Grab(SelectEnterEventArgs arg0)
    {
        ball.GetComponent<Renderer>().material = highlightedMaterialRef;
    }


    // Détecter quand le joueur entre dans la zone de lancement
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            platform.GetComponent<Renderer>().material = highlightedMaterialRef;
            isInLaunchZone = true;
        }
    }

    // Détecter quand le joueur sort de la zone de lancement
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            platform.GetComponent<Renderer>().material = neutralMaterialRef;
            isInLaunchZone = false;
        }
    }
    

}


