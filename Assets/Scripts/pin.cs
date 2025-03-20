using UnityEngine;

public class pin : MonoBehaviour
{
    private bool isFallen = false;

    private GameManager gameManager;

    public GameObject pinParent;

    public Material redMaterialRef;

    void Start()
    {
        // Trouve le GameManager dans la scène
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
        
    }
    
    
    //------------------------ PAS FONCTIONNEL PB DE DETECTION AVEC LE SOL

    private void OnCollisionEnter(Collision collision)
    {
        if (!isFallen && collision.gameObject.CompareTag("Ground"))
        {
            
            gameManager.PinFallen(); // Informe le GameManager que cette quille est tombée
            pinParent.GetComponent<Renderer>().material = redMaterialRef;
            isFallen = true;

        }
    }
}
