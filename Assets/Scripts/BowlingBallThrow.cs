using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BowlingBallThrow : MonoBehaviour
{
    public float throwForce = 2;  // Force du lancer
    public float throwTorque = 5;  // Force du spin (rotation)
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    private GameManager gameManager;

    //private bool isHeld = false;
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        rb = gameObject.GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrabStarted);
        grabInteractable.selectExited.AddListener(OnGrabEnded);
    }

    void FixedUpdate()
    {
        Vector3 velocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        velocity.Normalize();
        velocity *= throwForce;
        rb.linearVelocity.Set(velocity.x, rb.linearVelocity.y, rb.linearVelocity.z);
    }

    void OnGrabStarted(SelectEnterEventArgs arg0)
    {
        // Lorsque la boule est attrapée, désactiver la physique (Rigidbody) pour la contrôler manuellement.
        rb.isKinematic = true;
        gameManager.setIsBallHeld(true);

        gameManager.toggleLevelPhysics(false); // desactiver physique quilles

    }

    void OnGrabEnded(SelectExitEventArgs arg0)
    {
        // Lorsque la boule est relâchée, activer la physique pour permettre le lancer avec force.
        rb.isKinematic = false;
        gameManager.setIsBallHeld(false);

        // Appliquer la force et la rotation pour le lancer.
        ThrowBall(arg0);
    }

    void ThrowBall(SelectExitEventArgs arg0)
    {
        if (gameManager.CanThrowBall())
        {
            // force de lancer

            Vector3 throwDirection = arg0.interactorObject.transform.forward;
            rb.AddForce(throwDirection * throwForce, ForceMode.VelocityChange);
            // rotation
            Vector3 topPosition = transform.position + Vector3.up * rb.transform.localScale.y;  // Top is in the +Y direction from the center of the sphere
            Vector3 downPosition = transform.position - Vector3.up * rb.transform.localScale.y;  // Top is in the +Y direction from the center of the sphere
            rb.AddForceAtPosition(arg0.interactorObject.transform.up * throwTorque, topPosition, ForceMode.Force);
            // rb.AddForceAtPosition(-arg0.interactorObject.transform.up * throwTorque, downPosition, ForceMode.Force);

            gameManager.toggleLevelPhysics(true); // activer physique quilles

            Debug.Log(rb.linearVelocity);
        }
        else
        {
            Debug.Log("Go in the start zone");
            
            
            // boule posee au sol ss mouvement
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;

            rb.isKinematic = true;

        }

    }
}
