using System;
using UnityEngine;

public class pin : MonoBehaviour
{
    private bool hasFallen = false;

    private GameManager gameManager;

    private float fallenAngleThreshold = 30f; // Angle à partir duquel une quille est considérée comme tombée

    public Material redMaterialRef;

    private Vector3 initialUp;

    void Start()
    {
        // Trouve le GameManager dans la sc�ne
        gameManager = GameObject.FindFirstObjectByType<GameManager>();

        initialUp = transform.up;
        
    }

    private void Update()
    {
        float angle = Vector3.Angle(initialUp, transform.up);
        
        if (angle > fallenAngleThreshold && !hasFallen)
        {
            hasFallen = true;
            gameManager.PinFallen(); // Informe le GameManager que cette quille est tombée
            this.GetComponent<Renderer>().material = redMaterialRef;
        }
    }


    // //------------------------ PAS FONCTIONNEL PB DE DETECTION AVEC LE SOL
    //
    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (!isFallen && collision.gameObject.CompareTag("Ground"))
    //     {
    //         
    //         gameManager.PinFallen(); // Informe le GameManager que cette quille est tomb�e
    //         pinParent.GetComponent<Renderer>().material = redMaterialRef;
    //         isFallen = true;
    //
    //     }
    // }
}
