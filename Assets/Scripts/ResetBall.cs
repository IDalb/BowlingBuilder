using UnityEngine;

public class ResetBall : MonoBehaviour
{
    public GameObject ball;
    public Transform ballSpawn;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            //Destroy(other.gameObject);
            //Instantiate(newBall, ballSpawn.position, ballSpawn.rotation);

            ball.transform.position = ballSpawn.position;
            ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;


        }
    }
}
