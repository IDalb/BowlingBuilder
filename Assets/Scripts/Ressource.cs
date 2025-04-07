using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Ressource : MonoBehaviour
{
    public int grabCount = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<XRGrabInteractable>().selectEntered.AddListener(Grab);

    }
    private void Grab(SelectEnterEventArgs arg0)
    {
        grabCount++;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
