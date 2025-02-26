using UnityEngine;

public class ResetBall : MonoBehaviour
{
    public GameObject newBall;
    public Transform ballSpawn;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            Destroy(other.gameObject);
            Instantiate(newBall, ballSpawn.position, ballSpawn.rotation);
        }
    }
}
