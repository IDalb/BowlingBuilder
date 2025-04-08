using UnityEngine;

public class ResetBall : MonoBehaviour
{
    public GameObject ball;
    public Transform ballSpawn;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            ResetBallPosition();
            gameManager.RemoveFallenPins();
        }
    }

    public void ResetBallPosition()
    {
        ball.transform.position = ballSpawn.position;
        ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ball.GetComponent<AudioSource>().Stop(); // arreter le son balle qui roule
        this.GetComponent<AudioSource>().Play(); // son restart

        // Remove wind particles
        if (ball.GetComponent<BowlingBallThrow>() != null)
            Destroy(ball.GetComponent<BowlingBallThrow>().windParticlesInstance);
    }
}
