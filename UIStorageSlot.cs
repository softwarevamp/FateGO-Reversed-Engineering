using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/UI Storage Slot")]
public class UIStorageSlot : UIItemSlot
{
    public int slot;
    public UIItemStorage storage;

    protected override InvGameItem Replace(InvGameItem item) => 
        ((this.storage == null) ? item : this.storage.Replace(this.slot, item));

    protected override InvGameItem observedItem =>
        this.storage?.GetItem(this.slot);
}

