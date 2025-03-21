using System;
using UnityEngine;

public class Pin : MonoBehaviour
{
    private bool hasFallen = false;

    ScoreManager scoreManager;

    private float fallenAngleThreshold = 15f; // Angle à partir duquel une quille est considérée comme tombée

    public Material redMaterialRef;

    private Vector3 initialUp;

    void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();

        initialUp = transform.up;
        
    }

    private void Update()
    {
        float angle = Vector3.Angle(initialUp, transform.up);
        
        if (angle > fallenAngleThreshold && !hasFallen)
        {
            hasFallen = true;
            scoreManager.IncreaseFallenPinsNb();
            this.GetComponent<Renderer>().material = redMaterialRef;
        }
    }


    public bool isFallen()
    {
        return hasFallen;
    }

}
