using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BowlingBallThrow : MonoBehaviour
{
    public float throwForce = 2;  // Force du lancer
    public float throwTorque = 5;  // Force du spin (rotation)
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    private GameManager gameManager;
    public float headTilt;

    [Space]

    [SerializeField] private Vector2 headTiltThreshold = new Vector2(15f, 35f);  // Angles d'inclinaison min et max de la tête pour lesquels on dévie la boule
    [SerializeField] private float headDeviationForce = 3f; // Force de déviation en inclinant la tête
    [SerializeField] private GameObject windParticles;
    public GameObject windParticlesInstance { get; private set; } = null;
    [Space]
    
    [SerializeField] private AudioClip rollClip;
    [SerializeField] private AudioClip errorClip;

    private AudioSource audioSource;


    public void registerGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    void Start()
    {

        rb = gameObject.GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrabStarted);
        grabInteractable.selectExited.AddListener(OnGrabEnded);
        
        audioSource = this.GetComponent<AudioSource>();

        //if (windParticles != null)
        //    windParticlesInstance = Instantiate(windParticles, transform.position, transform.rotation);
    }

    void FixedUpdate()
    {
        Vector3 velocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        velocity.Normalize();
        velocity *= throwForce;
        rb.linearVelocity.Set(velocity.x, rb.linearVelocity.y, rb.linearVelocity.z);

        // Slightly move the ball left or right by tilting the head
        headTilt = -Camera.main.transform.eulerAngles.z;
        if (headTilt < -180f) headTilt += 360f;
        float directionMultiplier = Mathf.Clamp(
            (Mathf.Abs(headTilt) - headTiltThreshold.x) / (headTiltThreshold.y - headTiltThreshold.x),
            0f,
            1f
        ) * Mathf.Sign(headTilt);

        Vector3 orthogonal = Vector3.Cross(Vector3.up, velocity).normalized;
        rb.AddForce(orthogonal * headDeviationForce * directionMultiplier);

        // Wind particles
        if (windParticlesInstance != null)
        {
            windParticlesInstance.transform.position = transform.position;
            windParticlesInstance.transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
            windParticlesInstance.SetActive(directionMultiplier != 0);
            windParticlesInstance.transform.localScale = new Vector3(directionMultiplier * .5f, .5f, .5f);
        }
        
        
    }

    void OnGrabStarted(SelectEnterEventArgs arg0)
    {
        // Si des quilles sont tombées entre-temps, on les efface (permet aussi de checker une fin de partie)
        gameManager.RemoveFallenPins();

        // Lorsque la boule est attrap�e, d�sactiver la physique (Rigidbody) pour la contr�ler manuellement.
        rb.isKinematic = true;

        gameManager.toggleLevelPhysics(false); // desactiver physique quilles

        audioSource.Stop();
    }

    void OnGrabEnded(SelectExitEventArgs arg0)
    {
        // Lorsque la boule est rel�ch�e, activer la physique pour permettre le lancer avec force.
        rb.isKinematic = false;

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

            // Initialize wind particles
            if (windParticles != null)
                windParticlesInstance = Instantiate(windParticles, transform.position, transform.rotation);

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
