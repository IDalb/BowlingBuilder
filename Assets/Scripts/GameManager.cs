using System;
using System.Collections.Generic;
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

    private List<Pin> pinsList = new List<Pin>();


    void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();

        GameObject[] pins = GameObject.FindGameObjectsWithTag("Pin");
        foreach (GameObject pin in pins)
        {
            pinsList.Add(pin.GetComponent<Pin>());
        }
        scoreManager.setTotalPinsNb(pins.Length);

        ball.GetComponent<XRGrabInteractable>().selectEntered.AddListener(Grab);
        ball.GetComponent<XRGrabInteractable>().selectExited.AddListener(Release);

    }

    public void RemoveFallenPins()
    {
        // On parcourt la liste à l'envers pour éviter des erreurs lors de la suppression
        for (int i = pinsList.Count - 1; i >= 0; i--)
        {
            Pin pin = pinsList[i];
            if (pin.isFallen())
            {
                // Détruire l'objet pin
                Destroy(pin.gameObject);

                // Retirer le pin de la liste
                pinsList.RemoveAt(i);
            }
        }
    }

    private void Release(SelectExitEventArgs arg0)
    {
        ball.GetComponent<Renderer>().material = neutralMaterialRef;

        if (isInLaunchZone)
        {
            scoreManager.IncreaseThrowNumber();
        }
        else
        {
            //TODO bloquer quilles ou autre methode pour empecher le joueur de tirer
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
    
    public bool CanThrowBall() { return isInLaunchZone; }

}


