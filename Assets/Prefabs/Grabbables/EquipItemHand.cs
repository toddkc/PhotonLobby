using System.Collections.Generic;
using UnityEngine;

public class EquipItemHand : MonoBehaviour
{
    [SerializeField] private OVRInput.Controller controller;
    public OVRInput.Controller Controller { get { return controller; } }

    public bool IsEquipped { get; set; }

    private ControllerHaptic haptic;
    [SerializeField] private List<GameObject> itemModels;

    public int Hand { get { return handIndex; } }
    [SerializeField] private int handIndex;

    private void Awake()
    {
        haptic = GetComponentInParent<ControllerHaptic>();
    }

    private void Start()
    {
        IsEquipped = false;
    }

    public void EnterTrigger()
    {
        haptic.Pulse(controller);
    }
}