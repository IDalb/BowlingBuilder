using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TwoHandResize : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;  // Reference to XR Grab Interactable
    private Transform firstHandTransform;
    private Transform secondHandTransform;

    private Vector3 initialDistance;
    private Vector3 initialScale;
    private Vector3 initialObjectPosition;

    private void OnEnable()
    {
        // Listen to the grab events
        grabInteractable.selectEntered.AddListener(OnGrabEntered);
        grabInteractable.selectExited.AddListener(OnGrabExited);
    }

    private void OnDisable()
    {
        // Remove listeners when no longer needed
        grabInteractable.selectEntered.RemoveListener(OnGrabEntered);
        grabInteractable.selectExited.RemoveListener(OnGrabExited);
    }

    // When grabbing the object
    private void OnGrabEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject is XRDirectInteractor)
        {
            if (firstHandTransform == null)
            {
                firstHandTransform = args.interactorObject.transform;
            }
            else if (secondHandTransform == null)
            {
                secondHandTransform = args.interactorObject.transform;
            }

            initialScale = transform.localScale;
            initialObjectPosition = transform.position;

            // Store initial distance between hands
            if (firstHandTransform != null && secondHandTransform != null)
            {
                initialDistance = secondHandTransform.position - firstHandTransform.position;
            }
        }
    }

    // When releasing the object
    private void OnGrabExited(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform == firstHandTransform)
        {
            firstHandTransform = null;
        }
        else if (args.interactorObject.transform == secondHandTransform)
        {
            secondHandTransform = null;
        }
    }

    private void Update()
    {
        if (firstHandTransform != null && secondHandTransform != null)
        {
            Vector3 currentDistance = secondHandTransform.position - firstHandTransform.position;

            // Calculate the scale factor based on the change in distance
            float scaleFactor = currentDistance.magnitude / initialDistance.magnitude;

            // Adjust the object’s scale while keeping the midpoint in the same position
            transform.localScale = initialScale * scaleFactor;

            // Position the object to keep the midpoint between hands in the same position
            Vector3 midpoint = (firstHandTransform.position + secondHandTransform.position) / 2;
            transform.position = initialObjectPosition + (midpoint - (firstHandTransform.position + secondHandTransform.position) / 2);
        }
    }
}
