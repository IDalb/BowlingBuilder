using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BowlingBallThrow : MonoBehaviour
{
    public float throwForce = 2;  // Force du lancer
    public float throwTorque = 5;  // Force du spin (rotation)
    [SerializeField] private float headDeviationForce = 0.1f; // Force de déviation en s'inclinant
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    private GameManager gameManager;
    
    [SerializeField] private AudioClip rollClip;
    [SerializeField] private AudioClip errorClip;

    private AudioSource audioSource;

    //private bool isHeld = false;
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        rb = gameObject.GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrabStarted);
        grabInteractable.selectExited.AddListener(OnGrabEnded);
        
        audioSource = this.GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        Vector3 velocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        velocity.Normalize();
        
        // Slightly move the ball left or right by tilting the head
        if (Keyboard.current.gKey.isPressed) {
            Debug.Log("G pressed");
            velocity -= Vector3.Cross(velocity, Vector3.up).normalized * headDeviationForce;
        }
        else if (Keyboard.current.hKey.isPressed) {
            Debug.Log("H pressed");
            velocity += Vector3.Cross(velocity, Vector3.up).normalized * headDeviationForce;
        }
        
        velocity *= throwForce;
        rb.linearVelocity.Set(velocity.x, rb.linearVelocity.y, velocity.z);
    }

    void OnGrabStarted(SelectEnterEventArgs arg0)
    {
        // Lorsque la boule est attrap�e, d�sactiver la physique (Rigidbody) pour la contr�ler manuellement.
        rb.isKinematic = true;
        gameManager.setIsBallHeld(true);

        gameManager.toggleLevelPhysics(false); // desactiver physique quilles

        audioSource.Stop();
    }

    void OnGrabEnded(SelectExitEventArgs arg0)
    {
        // Lorsque la boule est rel�ch�e, activer la physique pour permettre le lancer avec force.
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

            // Son de balle qui roule
            audioSource.clip = rollClip;
            audioSource.Play();
            
            Debug.Log(rb.linearVelocity);
        }
        else
        {
            Debug.Log("Go in the start zone");
            
            
            // boule immobile
            
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;

            rb.isKinematic = true;

            // son erreur
            audioSource.clip = errorClip;
            audioSource.Play();
            
        }

    }
}
