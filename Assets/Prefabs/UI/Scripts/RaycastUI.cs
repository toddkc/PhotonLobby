using System.Collections;
using UnityEngine;

/// <summary>
/// used by the player to interact with custom world-space ui elements
/// </summary>

public class RaycastUI : MonoBehaviour
{
    [SerializeField] private LayerMask uiLayers = default;
    [SerializeField] private float interactDistance = 10f;

    private Transform thisTransform;
    private ICustomUIElement currentSelected;
    private WaitForSeconds clickDelay;
    private bool canClick = true;

    private void Awake()
    {
        currentSelected = null;
        thisTransform = transform;
    }

    private void Start()
    {
        clickDelay = new WaitForSeconds(0.2f);
    }

    private void OnEnable()
    {
        currentSelected = null;
    }
    private void OnDisable()
    {
        currentSelected = null;
    }

    private void Update()
    {
        CheckForUI();

        if (InputBridgeBase.instance.Interact && canClick && currentSelected != null)
        {
            canClick = false;
            currentSelected.OnClick();
            currentSelected.OnExit();
            currentSelected = null;
            StartCoroutine(ResetClick());
        }
    }

    private void CheckForUI()
    {
        if (!Physics.Raycast(thisTransform.position, thisTransform.forward, out RaycastHit hit, interactDistance, uiLayers))
        {
            if (currentSelected != null)
            {
                currentSelected.OnExit();
                currentSelected = null;
            }
        }
        else
        {
            var uiobj = hit.transform.GetComponent<ICustomUIElement>();
            if (uiobj != null && uiobj != currentSelected)
            {
                currentSelected = uiobj;
                uiobj.OnHover();
            }
        }
    }

    private IEnumerator ResetClick()
    {
        yield return clickDelay;
        canClick = true;
    }
}
