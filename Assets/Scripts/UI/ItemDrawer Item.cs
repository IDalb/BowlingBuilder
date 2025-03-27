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
        Transform obj = args.interactableObject.transform;
        obj.GetComponent<XRGrabInteractable>().selectEntered.RemoveListener(GrabObject);
        obj.GetComponent<XRGrabInteractable>().selectExited.AddListener(UngrabObject);

        // Rescale the object upon pickup
        const float rescaleFactor = 25;

        LeanTween.value(obj.gameObject, obj.localScale, obj.localScale * rescaleFactor, 0.5f)
        .setEaseOutQuad()
        .setOnUpdate((Vector3 ctx) => { obj.GetComponent<XRGrabInteractable>().SetTargetLocalScale(ctx); });
            
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
