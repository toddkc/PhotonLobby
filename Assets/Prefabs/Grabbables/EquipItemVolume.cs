using UnityEngine;

public class EquipItemVolume : MonoBehaviour
{
    [SerializeField] private int itemIndex = 0;
    private ItemSwitch switcher;
    private EquipItemHand hand;

    private void Awake()
    {
        switcher = GetComponentInParent<ItemSwitch>();
    }

    private void OnTriggerEnter(Collider other)
    {
        hand = other.GetComponent<EquipItemHand>();
        hand.EnterTrigger();
    }

    private void OnTriggerExit(Collider other)
    {
        hand = null;
    }

    private void Update()
    {
        if (hand == null) return;
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, hand.Controller))
        {
            ToggleItem();
        }
    }

    private void ToggleItem()
    {
        if (hand.IsEquipped)
        {
            switcher.SwitchItem(hand.Hand, -1);
        }
        else
        {
            switcher.SwitchItem(hand.Hand, itemIndex);
        }
        hand.IsEquipped = !hand.IsEquipped;
        hand = null;
    }
}