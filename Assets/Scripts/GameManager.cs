using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GameManager : MonoBehaviour
{
    static int levelIndex = 1;

    public GameObject ball;

    public GameObject platform;
    public Material neutralMaterialRef;
    public Material highlightedMaterialRef;

    private string playerTag = "MainCamera";

    ScoreManager scoreManager;

    private bool isInLaunchZone = false;
    private bool isBallHeld = false;
    public void setIsBallHeld(bool res) {isBallHeld = res; }

    private List<Pin> pinsList = new List<Pin>();


    void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();

        GameObject[] pins = GameObject.FindGameObjectsWithTag("Pin");
        foreach (GameObject pin in pins)
        {
            pinsList.Add(pin.GetComponent<Pin>());
            pin.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            pin.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            pin.gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
        scoreManager.setTotalPinsNb(pins.Length);

        ball.GetComponent<XRGrabInteractable>().selectEntered.AddListener(Grab);
        ball.GetComponent<XRGrabInteractable>().selectExited.AddListener(Release);

        Scene currentScene = SceneManager.GetActiveScene();

        // Get the name of the current scene
        string sceneName = currentScene.name;
        int index = sceneName.IndexOf(" ") + 1;
        string levelNumber = sceneName.Substring(index);
        if (levelNumber != sceneName)
        {
            levelIndex = int.Parse(levelNumber);
        }
    }

    public bool toggleLevelPhysics(bool enable)
    {
        if (enable) 
        {
            // activer la gravit� des quilles
            foreach (Pin pin in pinsList)
            {
                pin.gameObject.layer = LayerMask.NameToLayer("ActivePin");
                //pin.gameObject.GetComponent<CapsuleCollider>().enabled = true;
                //pin.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                //pin.gameObject.GetComponent<Rigidbody>().useGravity = true;
            }
            return true;
        }
        else
        {
            // gravit� des quilles desactiv�es
            foreach (Pin pin in pinsList)
            {
                pin.gameObject.layer = LayerMask.NameToLayer("InactivePin");

                //pin.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                //pin.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                //pin.gameObject.GetComponent<Rigidbody>().useGravity = false;
            }
            return false;
        }
    }

    public void RemoveFallenPins()
    {
        // On parcourt la liste � l'envers pour �viter des erreurs lors de la suppression
        for (int i = pinsList.Count - 1; i >= 0; i--)
        {
            Pin pin = pinsList[i];
            if (pin.isFallen())
            {
                // D�truire l'objet pin
                Destroy(pin.gameObject);

                // Retirer le pin de la liste
                pinsList.RemoveAt(i);
            }
        }
        if(pinsList.Count == 0)
        {
            levelIndex++;
            SceneManager.LoadScene("level " + levelIndex.ToString());
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
            Debug.Log("Lancer invalide");
        }
    }

    private void Grab(SelectEnterEventArgs arg0)
    {
        ball.GetComponent<Renderer>().material = highlightedMaterialRef;
    }


    // D�tecter quand le joueur entre dans la zone de lancement
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            platform.GetComponent<Renderer>().material = highlightedMaterialRef;
            // GameObject[] ressources = GameObject.FindGameObjectsWithTag("Ressource");

            //// desactive les collisions entre blocs et quilles
            //foreach(GameObject constructionObject in ressources)
            //{
            //    constructionObject.layer = LayerMask.NameToLayer("ConstructionBlock");
            //}
            isInLaunchZone = true;
            
        }
    }

    // D�tecter quand le joueur sort de la zone de lancement
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            //GameObject[] ressources = GameObject.FindGameObjectsWithTag("Ressource");

            //// active le grab des blocs
            //foreach (GameObject constructionObject in ressources)
            //{
            //    constructionObject.layer = LayerMask.NameToLayer("Default");
            //}
            //platform.GetComponent<Renderer>().material = neutralMaterialRef;
            
            isInLaunchZone = false;
        }
    }
    
    public bool CanThrowBall() { return isInLaunchZone; }



}


