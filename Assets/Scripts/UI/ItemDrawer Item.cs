using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using static UnityEngine.Rendering.GPUSort;

public class ItemDrawerItem : MonoBehaviour
{
    public Transform objectParent;
    [SerializeField] private TextMeshProUGUI quantityText;

    [HideInInspector] public ItemDrawer.Item item;

    private void OnEnable()
    {
        // Floating animation
        objectParent.localPosition = Vector3.zero;
        LeanTween.moveLocalZ(objectParent.gameObject, 0.35f, 1f).setEaseInOutSine().setLoopPingPong();
    }

    public void InstantiateObject()
    {
        quantityText.text = item.amount.ToString() + (item.amount <= 1 ? " restant" : " restants");

        if (item.amount <= 0) return;

        var obj = Instantiate(item.prefab, objectParent);
        if (obj.GetComponent<Rigidbody>() != null)
        {
            obj.GetComponent<Rigidbody>().isKinematic = true;
        }
        obj.transform.localPosition = item.offset;

        if (obj.GetComponent<XRGrabInteractable>())
        {
            obj.GetComponent<XRGrabInteractable>().selectEntered.AddListener(GrabObject);
        }
    }

    public void GrabObject(SelectEnterEventArgs args)
    {
        args.interactableObject.transform.GetComponent<XRGrabInteractable>().selectEntered.RemoveListener(GrabObject);
        args.interactableObject.transform.GetComponent<XRGrabInteractable>().selectExited.AddListener(UngrabObject);

        item.amount --;
        InstantiateObject();
    }

    public void UngrabObject(SelectExitEventArgs args)
    {
        //args.interactableObject.transform.GetComponent<XRGrabInteractable>().selectExited.RemoveListener(UngrabObject);

        args.interactableObject.transform.gameObject.transform.parent = null;
        args.interactableObject.transform.gameObject.transform.GetComponent<Rigidbody>().isKinematic = false;
    }
}
