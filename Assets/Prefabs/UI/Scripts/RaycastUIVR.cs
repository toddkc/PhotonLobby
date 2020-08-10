using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaycastUIVR : MonoBehaviour
{
    [SerializeField] private LayerMask uiLayers = default;
    [SerializeField] private float interactDistance = 10f;
    [SerializeField] private Transform offsetTransform = default;
    [SerializeField] private OVRInput.Controller controller = default;

    private ControllerHaptic haptic = default;
    private Transform thisTransform;
    private ICustomUIElement currentSelected;
    private WaitForSeconds clickDelay;
    private bool canClick = true;
    private LineRenderer rend;
    private bool isGameScene;
    private bool isMenuOpen;
    private EquipItemHand handItem;

    private void Awake()
    {
        currentSelected = null;
        thisTransform = transform;
        rend = GetComponent<LineRenderer>();
        haptic = GetComponent<ControllerHaptic>();
        handItem = GetComponentInChildren<EquipItemHand>();
    }

    private void Start()
    {
        clickDelay = new WaitForSeconds(0.2f);
        isGameScene = SceneManager.GetActiveScene().buildIndex > 1;
        isMenuOpen = false;
    }

    private void OnEnable()
    {
        currentSelected = null;
        SceneManager.activeSceneChanged += ResetMenuState;
    }
    private void OnDisable()
    {
        currentSelected = null;
        SceneManager.activeSceneChanged -= ResetMenuState;
    }

    private void ResetMenuState(Scene oldscene, Scene newscene)
    {
        isMenuOpen = false;
        isGameScene = newscene.buildIndex > 1;
    }

    public void ToggleMenuOpen()
    {
        isMenuOpen = !isMenuOpen;
    }

    // check if ui interaction is happening
    private void Update()
    {
        if (handItem.IsEquipped)
        {
            rend.enabled = false;
            return;
        }
        CheckForUI();

        // click ui if possible
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, controller) && canClick && currentSelected != null)
        {
            canClick = false;
            currentSelected.OnClick();
            currentSelected.OnExit();
            currentSelected = null;
            StartCoroutine(ResetClick());
        }
    }

    // handle hover/exit of ui objects
    private void CheckForUI()
    {
        var _heading = thisTransform.position - offsetTransform.position;
        var _target = offsetTransform.position + _heading.normalized * interactDistance;
        if (!Physics.Raycast(offsetTransform.position, _heading.normalized, out RaycastHit hit, interactDistance, uiLayers))
        {
            if (currentSelected != null)
            {
                currentSelected.OnExit();
                currentSelected = null;
            }

            // if touch show ray
            if (OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger, controller))
            {
                if((!isGameScene || isMenuOpen))
                {
                    rend.enabled = true;
                    rend.SetPosition(0, thisTransform.position);
                    rend.SetPosition(1, _target);
                }
            }
            else
            {
                if (rend.enabled) rend.enabled = false;
            }
        }
        else
        {
            var uiobj = hit.transform.GetComponent<ICustomUIElement>();
            if (uiobj != null && uiobj != currentSelected)
            {
                haptic.Pulse(controller);
                currentSelected = uiobj;
                uiobj.OnHover();
            }

            // always show ray when item is hovered
            rend.enabled = true;
            rend.SetPosition(0, thisTransform.position);
            rend.SetPosition(1, hit.point);
        }
    }

    // delay before user can click again
    private IEnumerator ResetClick()
    {
        yield return clickDelay;
        canClick = true;
    }
}