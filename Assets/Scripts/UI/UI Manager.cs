using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class UIManager : MonoBehaviour
{
    public enum Hand { Left, Right };
    public Hand dominentHand { private get; set; }
    public Hand GetDominentHand() { return dominentHand; }
    public Hand GetNonDominentHand() { return dominentHand == Hand.Left ? Hand.Right : Hand.Left; }

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

    private void Start()
    {
        if (settingsMenu.GetComponent<SettingsUI>() != null)
            settingsMenu.GetComponent<SettingsUI>().uiManager = this;

        dominentHand = Hand.Right;
        ReparentUI(GetNonDominentHand());
    }

    private void OnEnable()
    {
        inputAction.XRILeftInteraction.OpenDrawer.performed += ctx => ToggleItemDrawer(Hand.Left);
        inputAction.XRILeftInteraction.OpenSettings.performed += ctx => ToggleSettingsMenu(Hand.Left);

        inputAction.XRIRightInteraction.OpenDrawer.performed += ctx => ToggleItemDrawer(Hand.Right);
        inputAction.XRIRightInteraction.OpenSettings.performed += ctx => ToggleSettingsMenu(Hand.Right);

        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.XRILeftInteraction.OpenDrawer.performed -= ctx => ToggleItemDrawer(Hand.Left);
        inputAction.XRILeftInteraction.OpenSettings.performed -= ctx => ToggleSettingsMenu(Hand.Left);

        inputAction.XRIRightInteraction.OpenDrawer.performed -= ctx => ToggleItemDrawer(Hand.Right);
        inputAction.XRIRightInteraction.OpenSettings.performed -= ctx => ToggleSettingsMenu(Hand.Right);

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

    void ToggleItemDrawer(Hand hand)
    {
        if (dominentHand == hand) return;

        if (settingsMenu.activeInHierarchy)
            SetMenuActive(settingsMenu, false, hand);

        SetMenuActive(itemDrawer, !itemDrawer.activeInHierarchy, hand);
    }

    void ToggleSettingsMenu(Hand hand)
    {
        if (dominentHand == hand) return;

        if (itemDrawer.activeInHierarchy)
            SetMenuActive(itemDrawer, false, hand);

        SetMenuActive(settingsMenu, !settingsMenu.activeInHierarchy, hand);
    }


    public void SwapDominentHand()
    {
        SetMenuActive(settingsMenu, false, GetNonDominentHand());
        
        dominentHand = dominentHand == Hand.Left ? Hand.Right : Hand.Left;

        // Parents the UI to the NON-dominent hand
        ReparentUI(GetNonDominentHand());
        SetMenuActive(settingsMenu, true, GetNonDominentHand());

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
