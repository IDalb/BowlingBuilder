using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BowlingBallThrow : MonoBehaviour
{
    private float throwForce = 0.5f;  // Force du lancer
    private float throwTorque = 0.5f;  // Force du spin (rotation)
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    

    private GameManager gameManager;

    private bool isHeld = false;
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
        // Apply torque to make the ball rotate in a specific direction (e.g., clockwise)

    }

    void OnGrabStarted(SelectEnterEventArgs arg0)
    {
        // Lorsque la boule est attrapée, désactiver la physique (Rigidbody) pour la contrôler manuellement.
        rb.isKinematic = true;
        isHeld = true;
    }

    void OnGrabEnded(SelectExitEventArgs arg0)
    {
        // Lorsque la boule est relâchée, activer la physique pour permettre le lancer avec force.
        rb.isKinematic = false;
        isHeld = false;

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
            rb.AddForceAtPosition(arg0.interactorObject.transform.up * throwTorque, topPosition, ForceMode.Force);


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
