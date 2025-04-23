using System;
using UnityEngine;

public class Pin : MonoBehaviour
{
    private bool hasFallen = false;

    ScoreManager scoreManager;

    private float fallenAngleThreshold = 15f; // Angle à partir duquel une quille est considérée comme tombée

    public Material redMaterialRef; // materiau rouge

    private Vector3 initialUp; // vecteur qui pointe vers le ciel

    // audio
    [SerializeField] private AudioClip fallClip;
    private AudioSource audioSource;
    private Renderer _renderer;

    void Start()
    {
        _renderer = this.GetComponent<Renderer>();
        scoreManager = FindFirstObjectByType<ScoreManager>();

        initialUp = transform.up;

        audioSource = this.GetComponent<AudioSource>();
        
    }

    private void Update()
    /* Update pour verifier si la quille est tombée, appelé à chaque frame */
    {
        float angle = Vector3.Angle(initialUp, transform.up);
        
        if (angle > fallenAngleThreshold && !hasFallen)
        {
            hasFallen = true;
            audioSource.clip = fallClip;
            audioSource.Play();
            
            _renderer.material = redMaterialRef;
            
            if (scoreManager){
                scoreManager.IncreaseFallenPinsNb();
            }
        }
    }


    public bool isFallen()
    {
        return hasFallen;
    }

}
