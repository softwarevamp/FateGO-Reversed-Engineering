using System;
using UnityEngine;

public class PresentBoxNotificationListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UILabel countTextLabel;
    [SerializeField]
    protected ItemIconComponent itemIcon;
    [SerializeField]
    protected UILabel nameTextLabel;

    public void SetInput(PresentBoxNotificationListViewItem item, bool isInput)
    {
    }

    public void SetItem(PresentBoxNotificationListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            mode = DispMode.INVISIBLE;
        }
        if (mode != DispMode.INVISIBLE)
        {
            this.nameTextLabel.text = item.NameText;
            this.countTextLabel.text = item.CountText;
            this.itemIcon.SetGift(item.GiftType, item.ObjactId, -1);
            this.nameTextLabel.text = item.NameText;
            this.countTextLabel.text = item.CountText;
        }
    }

    public enum DispMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT
    }
}

