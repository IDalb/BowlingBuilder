using System;
using System.Collections;
using System.Collections.Generic;
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
        ball.GetComponent<BowlingBallThrow>().registerGameManager(this);// un game manager par balle  
        
        ballRespawnPosition = ball.transform.position; // initialiser position de retour de la balle

        GameObject[] pins = null;

        // initialiser pins
        if (isMainGameManager)
        {
            pins = GameObject.FindGameObjectsWithTag("Pin");
            foreach (GameObject pin in pins)
            {
                pinsList.Add(pin.GetComponent<Pin>());
            }
        }
        // Dans le cas ou il y a deux gamemanager (niveau tuto)
        else
        {
            pins = GameObject.FindGameObjectsWithTag("Pin2");
            foreach (GameObject pin in pins)
            {
                pinsList.Add(pin.GetComponent<Pin>());
            }
        }


        Scene currentScene = SceneManager.GetActiveScene();
        // pas de scoreManager dans le niveau tuto
        if (currentScene.name != "Tutorial")
        {
            scoreManager = FindFirstObjectByType<ScoreManager>();
            scoreManager.setTotalPinsNb(pins.Length);
        }

        // ecouteurs qudn la balle est attrapee/relachee
        ball.GetComponent<XRGrabInteractable>().selectEntered.AddListener(Grab);
        ball.GetComponent<XRGrabInteractable>().selectExited.AddListener(Release);

        
        // Nom du niveau
        string sceneName = currentScene.name;
        int index = sceneName.IndexOf(" ") + 1;
        string levelNumber = sceneName.Substring(index);
        if (levelNumber != sceneName && int.Parse(levelNumber) != levelIndex)
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

        ball.GetComponent<BowlingBallThrow>().setIsBallThrown(false);

        toggleMoveBlock(true);

        // Enlever les particules de vent
        if (ball.GetComponent<BowlingBallThrow>() != null)
            Destroy(ball.GetComponent<BowlingBallThrow>().windParticlesInstance);
    }

    public bool toggleMoveBlock(bool enable)
    /* Fonction pour la gestion de la physique des blocs */
    {
        GameObject[] ressources = GameObject.FindGameObjectsWithTag("Ressource");

        if (enable)
        {
            foreach (GameObject ressource in ressources)
            {
                ressource.gameObject.layer = LayerMask.NameToLayer("ConstructionBlock");
            }
            return true;
        }
        else
        {
            foreach (GameObject ressource in ressources)
            {
                ressource.gameObject.layer = LayerMask.NameToLayer("StaticBlock");
            }
            return false;
        }
    }
    
    public bool toggleLevelPhysics(bool enable)
    /* Fonction pour la gestion de la physique du niveau */
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        if (enable) 
        {
            // activer la gravit� des quilles
            foreach (Pin pin in pinsList)
            {
                pin.gameObject.layer = LayerMask.NameToLayer("ActivePin");
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
            return false;
        }
    }

    IEnumerator GoToNextLevel()
    /* Fonction pour changer de niveau */
    {
        yield return new WaitForSeconds(10);

        int newSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        if (newSceneIndex < 0 || newSceneIndex >= SceneManager.sceneCountInBuildSettings) {
            newSceneIndex = 0;
        }

        string nextSceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(newSceneIndex));
        
        if (nextSceneName.StartsWith("Level "))
            SceneManager.LoadScene(nextSceneName);
        else
            SceneManager.LoadScene("Main Menu");
    }

    public void RemoveFallenPins()
    // Retirer les quilles du terrain
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
    /* Quand on lache la balle */
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
    /* Quand on attrape la balle */
    {
        Debug.Log("Grab detected");
        ball.GetComponent<Renderer>().material = highlightedMaterialRef;
    }


    void OnTriggerEnter(Collider other)
    // D�tecter quand le joueur entre dans la zone de lancement
    {
        if (other.CompareTag(playerTag))
        {
            platform.GetComponent<Renderer>().material = highlightedMaterialRef;

            isInLaunchZone = true;
            
        }
    }

    void OnTriggerExit(Collider other)
    // D�tecter quand le joueur sort de la zone de lancement
    {
        if (other.CompareTag(playerTag))
        {

            platform.GetComponent<Renderer>().material = neutralMaterialRef;
            
            isInLaunchZone = false;
        }
    }
    
    public bool CanThrowBall() { return isInLaunchZone; }



}


