using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public enum Hand { Left, Right };
    public Hand dominentHand;

    [Header("UI Elements")]
    [SerializeField] GameObject itemDrawer;
    [SerializeField] GameObject settingsMenu;

    [Header("Hands")]
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;

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

        ReparentUI(dominentHand == Hand.Left ? Hand.Right : Hand.Left);
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

    void ToggleItemDrawer(Hand hand)
    {
        if (dominentHand == hand) return;

        if (settingsMenu.activeInHierarchy)
            settingsMenu.SetActive(false);

        itemDrawer.SetActive(!itemDrawer.activeInHierarchy);
    }

    void ToggleSettingsMenu(Hand hand)
    {
        if (dominentHand == hand) return;

        if (itemDrawer.activeInHierarchy)
            itemDrawer.SetActive(false);

        settingsMenu.SetActive(!settingsMenu.activeInHierarchy);
    }


    public void SwapDominentHand()
    {
        // Parents the UI to the NON-dominent hand
        ReparentUI(dominentHand);

        dominentHand = dominentHand == Hand.Left ? Hand.Right : Hand.Left;
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
