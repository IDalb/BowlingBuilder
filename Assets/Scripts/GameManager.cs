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
    private Vector3 ballRespawnPosition;

    public GameObject platform;
    public Material neutralMaterialRef;
    public Material highlightedMaterialRef;

    public bool isMainGameManager = true; // niveau tutoriel : 2 gamemanagers

    private string playerTag = "MainCamera";

    public ScoreManager scoreManager;

    private bool isInLaunchZone = false;

    private List<Pin> pinsList = new List<Pin>();

    void Start()
    {
        ball.GetComponent<BowlingBallThrow>().registerGameManager(this); // un game manager associe� � chaque bqlle
        
        
        ballRespawnPosition = ball.transform.position; // initialiser position de retour de la balle

        GameObject[] pins = null;

        // initialize pins
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
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name != "tutorial")
        {
            scoreManager = FindFirstObjectByType<ScoreManager>();
        
            scoreManager.setTotalPinsNb(pins.Length);
            
        }

        ball.GetComponent<XRGrabInteractable>().selectEntered.AddListener(Grab);
        ball.GetComponent<XRGrabInteractable>().selectExited.AddListener(Release);

        
        // Get the name of the current scene
        string sceneName = currentScene.name;
        int index = sceneName.IndexOf(" ") + 1;
        string levelNumber = sceneName.Substring(index);
        if (int.Parse(levelNumber) != levelIndex)
        {
            levelIndex = int.Parse(levelNumber);
        }
        
    }

    
    public void ResetBallPosition()
    /* Fonction permettant de replacer la balle à sa position de départ */
    {
        // Replacer la balle
        ball.transform.position = ballRespawnPosition;
        // Annuler tout mouvement qu'elle avait
        ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ball.GetComponent<AudioSource>().Stop(); // arreter le son balle qui roule

        // Remove wind particles
        if (ball.GetComponent<BowlingBallThrow>() != null)
            Destroy(ball.GetComponent<BowlingBallThrow>().windParticlesInstance);
    }
    
    
    
    public bool toggleLevelPhysics(bool enable)
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        GameObject[] ressources = GameObject.FindGameObjectsWithTag("Ressource");
        if (enable) 
        {
            // activer la gravit� des quilles
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
            // gravit� des quilles desactiv�es
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

        if(pinsList.Count == 0 && scoreManager)
        {
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
        Debug.Log("Grab detected");
        ball.GetComponent<Renderer>().material = highlightedMaterialRef;
    }


    // D�tecter quand le joueur entre dans la zone de lancement
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            platform.GetComponent<Renderer>().material = highlightedMaterialRef;

            isInLaunchZone = true;
            
        }
    }

    // D�tecter quand le joueur sort de la zone de lancement
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


