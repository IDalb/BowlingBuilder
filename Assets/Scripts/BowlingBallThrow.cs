using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BowlingBallThrow : MonoBehaviour
{
    private float throwForce = 0.5f;  // Force du lancer
    private float throwTorque = 10f;  // Force du spin (rotation)
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    private bool isHeld = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrabStarted);
        grabInteractable.selectExited.AddListener(OnGrabEnded);
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
        // force de lancer
        Vector3 throwDirection = arg0.interactorObject.transform.forward;
        rb.AddForce(throwDirection * throwForce, ForceMode.VelocityChange);


        // rotation (A FIX)
        Quaternion controllerRotation = arg0.interactorObject.transform.rotation;
        
        Vector3 torque = controllerRotation * Vector3.forward * throwTorque;
        rb.AddTorque(torque, ForceMode.Impulse);

    }
}
