using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GameManager : MonoBehaviour
{
    public static int levelIndex = 1;

    public GameObject ball;

    public GameObject platform;
    public Material neutralMaterialRef;
    public Material highlightedMaterialRef;

    public bool isMainGameManager = true; // niveau tutoriel : 2 gamemanagers

    private string playerTag = "MainCamera";

    public ScoreManager scoreManager;

    private bool isInLaunchZone = false;
    private bool isBallHeld = false;
    public void setIsBallHeld(bool res) {isBallHeld = res; }

    private List<Pin> pinsList = new List<Pin>();

    void Start()
    {
        ball.GetComponent<BowlingBallThrow>().registerGameManager(this); // un game manager associeé à chaque bqlle
        
        
        Scene currentScene = SceneManager.GetActiveScene();
        // Get the name of the current scene
        string sceneName = currentScene.name;
        int index = sceneName.IndexOf(" ") + 1;
        string levelNumber = sceneName.Substring(index);
        if (levelNumber != sceneName)
        {
            levelIndex = int.Parse(levelNumber);
        }

        GameObject[] pins = null;

        // initialyse pins
        if (isMainGameManager)
        {
            pins = GameObject.FindGameObjectsWithTag("Pin");
            foreach (GameObject pin in pins)
            {
                pinsList.Add(pin.GetComponent<Pin>());
            }
        }
        else
        {
            pins = GameObject.FindGameObjectsWithTag("Pin2");
            foreach (GameObject pin in pins)
            {
                pinsList.Add(pin.GetComponent<Pin>());
            }
        }


        // pas de scoreManager dans le niveau tuto
        if (currentScene.name != "tutorial")
        {
            scoreManager = FindFirstObjectByType<ScoreManager>();
        
            scoreManager.setTotalPinsNb(pins.Length);
            
        }


        ball.GetComponent<XRGrabInteractable>().selectEntered.AddListener(Grab);
        ball.GetComponent<XRGrabInteractable>().selectExited.AddListener(Release);

        

        
    }

    public bool toggleLevelPhysics(bool enable)
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        GameObject[] ressources = GameObject.FindGameObjectsWithTag("Ressource");
        if (enable) 
        {
            // activer la gravité des quilles
            foreach (Pin pin in pinsList)
            {
                pin.gameObject.layer = LayerMask.NameToLayer("ActivePin");
            }
            foreach (GameObject ball in balls)
            {
                    ball.gameObject.layer = LayerMask.NameToLayer("Ball");
            }
            foreach (GameObject ressource in ressources)
            {
                ressource.gameObject.layer = LayerMask.NameToLayer("StaticBlock");
            }
            return true;
        }
        else
        {
            // gravité des quilles desactivées
            foreach (Pin pin in pinsList)
            {
                pin.gameObject.layer = LayerMask.NameToLayer("InactivePin");
            }
            foreach (GameObject ball in balls)
            {
                //ball.gameObject.layer = LayerMask.NameToLayer("InactiveBall");     
            }
            foreach (GameObject ressource in ressources)
            {
                ressource.gameObject.layer = LayerMask.NameToLayer("ConstructionBlock");
            }
            return false;
        }
    }

    IEnumerator GoToNextLevel()
    {
        yield return new WaitForSeconds(10);
        Scene nextScene = SceneManager.GetSceneByName("level " + (levelIndex + 1).ToString());

        int totalScenes = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;

        if (nextScene.buildIndex <= totalScenes)
        {
            levelIndex++;
            SceneManager.LoadScene("level " + levelIndex.ToString());
        }
        else
        {
            SceneManager.LoadScene("Main Menu");

        }
        // now do something
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

        if(pinsList.Count == 0 && scoreManager)
        {
            levelIndex++;
#if UNITY_EDITOR
            GameObject telemetry = GameObject.FindGameObjectWithTag("Telemetry");
            if (telemetry)
            {
                telemetry.GetComponent<Telemetry>().SaveData();
            }
#endif

            GameObject winMenu = GameObject.FindGameObjectWithTag("WinMenu").transform.GetChild(0).gameObject;
            winMenu.SetActive(true);
            StartCoroutine(GoToNextLevel());
        }
    }

    private void Release(SelectExitEventArgs arg0)
    {
        ball.GetComponent<Renderer>().material = neutralMaterialRef;

        if (isInLaunchZone && scoreManager)
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


