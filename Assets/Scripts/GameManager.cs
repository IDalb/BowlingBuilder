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

    private bool gameStarted = false; //false : edit mode , true : throw ball mode

    private bool isInLaunchZone = false; 
    private bool hasBallInHand = false;

    private int fallenPins = 0;

    private GameObject[] pins;


    void Start()
    {
        //GameObject[] pins = GameObject.FindGameObjectsWithTag("Pin");
        //PinsCount = pins.Length;
        //Debug.Log(PinsCount);

        ball.GetComponent<XRGrabInteractable>().selectEntered.AddListener(Grab);
        ball.GetComponent<XRGrabInteractable>().selectExited.AddListener(Release);

    }

    public void PinFallen()
    {
        // Augmente le compteur des quilles tombées
        fallenPins++;
        Debug.Log("Une quille est tombée ! Total : " + fallenPins);

    }

    private void Release(SelectExitEventArgs arg0)
    {
        ball.GetComponent<Renderer>().material = neutralMaterialRef;
        hasBallInHand = false;
    }

    private void Grab(SelectEnterEventArgs arg0)
    {
        ball.GetComponent<Renderer>().material = highlightedMaterialRef;
        hasBallInHand = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInLaunchZone && hasBallInHand)
        {
            // TODO : enable throw
            gameStarted = true;
            //Debug.Log("Enable throw");
        }
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


