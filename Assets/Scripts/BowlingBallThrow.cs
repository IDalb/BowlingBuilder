using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BowlingBallThrow : MonoBehaviour
{
    private float throwForce = 0.5f;  // Force du lancer
    private float throwTorque = 10f;  // Force du spin (rotation)
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    private GameManager gameManager;

    private bool isHeld = false;
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrabStarted);
        grabInteractable.selectExited.AddListener(OnGrabEnded);
    }


    void OnGrabStarted(SelectEnterEventArgs arg0)
    {
        // Lorsque la boule est attrap�e, d�sactiver la physique (Rigidbody) pour la contr�ler manuellement.
        rb.isKinematic = true;
        isHeld = true;
    }

    void OnGrabEnded(SelectExitEventArgs arg0)
    {
        // Lorsque la boule est rel�ch�e, activer la physique pour permettre le lancer avec force.
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


            // rotation (A FIX)
            Quaternion controllerRotation = arg0.interactorObject.transform.rotation;
        
            Vector3 torque = controllerRotation * Vector3.forward * throwTorque;
            rb.AddTorque(torque, ForceMode.Impulse);

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
