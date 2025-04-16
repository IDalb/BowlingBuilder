using UnityEngine;

public class ResetBall : MonoBehaviour
{
    public GameManager gameManager;

    private void Start()
    {
    }
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (!other.CompareTag("Ball")) return;
        
        gameManager.ResetBallPosition();
        this.GetComponent<AudioSource>().Play(); // son quand la balle réapparaît
        gameManager.RemoveFallenPins();
    }

    
}
