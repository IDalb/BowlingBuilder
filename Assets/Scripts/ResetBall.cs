using UnityEngine;

public class ResetBall : MonoBehaviour
{
    public GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    /* Detecte collisions de la balle dans la zone de respawn */
    {

        if (!other.CompareTag("Ball")) return;
        
        gameManager.ResetBallPosition();
        this.GetComponent<AudioSource>().Play(); // son quand la balle réapparaît
        gameManager.RemoveFallenPins();
    }

    
}
