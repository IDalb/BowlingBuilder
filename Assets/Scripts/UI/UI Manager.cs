using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class UIManager : MonoBehaviour
{
    public enum Hand { Left, Right };
    private Hand? currentHand = null;
    
    [Header("UI Elements")]
    [SerializeField] GameObject itemDrawer;
    [SerializeField] GameObject settingsMenu;

    [Header("Hands")]
    [SerializeField] Transform leftHand;
    [SerializeField] XRBaseInteractor[] leftHandInteractors;
    [SerializeField] Transform rightHand;
    [SerializeField] XRBaseInteractor[] rightHandInteractor;

    XRIDefaultInputActions inputAction;

    [Space]
    public Transform throwAreaTransform;

    private void Awake()
    {
        inputAction = new XRIDefaultInputActions();
    }

    private void OnEnable()
    {
        inputAction.XRILeftInteraction.OpenDrawer.performed += ctx => ToggleItemDrawer(Hand.Left, true);
        inputAction.XRILeftInteraction.OpenDrawer.canceled += ctx => ToggleItemDrawer(Hand.Left, false);
        inputAction.XRILeftInteraction.OpenSettings.performed += ctx => ToggleSettingsMenu(Hand.Left, true);
        inputAction.XRILeftInteraction.OpenSettings.canceled += ctx => ToggleSettingsMenu(Hand.Left, false);

        inputAction.XRIRightInteraction.OpenDrawer.performed += ctx => ToggleItemDrawer(Hand.Right, true);
        inputAction.XRIRightInteraction.OpenDrawer.canceled += ctx => ToggleItemDrawer(Hand.Right, false);
        inputAction.XRIRightInteraction.OpenSettings.performed += ctx => ToggleSettingsMenu(Hand.Right, true);
        inputAction.XRIRightInteraction.OpenSettings.canceled += ctx => ToggleSettingsMenu(Hand.Right, false);

        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.XRILeftInteraction.OpenDrawer.performed -= ctx => ToggleItemDrawer(Hand.Left, true);
        inputAction.XRILeftInteraction.OpenDrawer.canceled -= ctx => ToggleItemDrawer(Hand.Left, false);
        inputAction.XRILeftInteraction.OpenSettings.performed -= ctx => ToggleSettingsMenu(Hand.Left, true);
        inputAction.XRILeftInteraction.OpenSettings.canceled -= ctx => ToggleSettingsMenu(Hand.Left, false);

        inputAction.XRIRightInteraction.OpenDrawer.performed -= ctx => ToggleItemDrawer(Hand.Right, true);
        inputAction.XRIRightInteraction.OpenDrawer.canceled -= ctx => ToggleItemDrawer(Hand.Right, false);
        inputAction.XRIRightInteraction.OpenSettings.performed -= ctx => ToggleSettingsMenu(Hand.Right, true);
        inputAction.XRIRightInteraction.OpenSettings.canceled -= ctx => ToggleSettingsMenu(Hand.Right, false);

        inputAction.Disable();
    }

    void SetMenuActive(GameObject menu, bool state, Hand hand) {
        menu.SetActive(state);
        if (hand == Hand.Left)
            foreach (var interactor in leftHandInteractors)
                interactor.gameObject.SetActive(!state);
        else if (hand == Hand.Right)
            foreach (var interactor in rightHandInteractor)
                interactor.gameObject.SetActive(!state);
    }

    void ToggleItemDrawer(Hand hand, bool state)
    {
        if (currentHand != null && currentHand != hand) return;
        if (settingsMenu.activeInHierarchy) return;
        currentHand = state ? hand : null;
        ReparentUI(hand);

        SetMenuActive(itemDrawer, state, hand);
    }

    void ToggleSettingsMenu(Hand hand, bool state)
    {
        if (currentHand != null && currentHand != hand) return;
        if (itemDrawer.activeInHierarchy) return;
        currentHand = state ? hand : null;
        ReparentUI(hand);

        SetMenuActive(settingsMenu, state, hand);

#if UNITY_EDITOR
        GameObject telemetry = GameObject.FindGameObjectWithTag("Telemetry");
        if (telemetry)
        {
            telemetry.GetComponent<Telemetry>().SetItemDrawerOpen();
        }
#endif
    }

    private void ReparentUI(Hand hand)
    {
        itemDrawer.transform.SetParent(hand == Hand.Right ? rightHand : leftHand);
        itemDrawer.transform.localPosition = new Vector3(0, 0, 0.05f);
        itemDrawer.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        settingsMenu.transform.SetParent(hand == Hand.Right ? rightHand : leftHand);
        settingsMenu.transform.localPosition = new Vector3(0, 0, 0.05f);
        settingsMenu.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
    }
}
