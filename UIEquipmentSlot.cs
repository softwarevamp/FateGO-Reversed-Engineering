using UnityEngine;

[AddComponentMenu("NGUI/Examples/UI Equipment Slot")]
public class UIEquipmentSlot : UIItemSlot
{
    public InvEquipment equipment;
    public InvBaseItem.Slot slot;

    protected override InvGameItem Replace(InvGameItem item) => 
        ((this.equipment == null) ? item : this.equipment.Replace(this.slot, item));

    protected override InvGameItem observedItem =>
        this.equipment?.GetItem(this.slot);
}

