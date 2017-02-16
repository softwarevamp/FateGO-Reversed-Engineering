using System;
using UnityEngine;

public class ServantListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UIIconLabel info2IconLabel;
    [SerializeField]
    protected UISprite lockSprite;
    [SerializeField]
    protected FlashingIconComponent partyIcon;
    [SerializeField]
    protected ServantFaceIconComponent servantFaceIcon;
    [SerializeField]
    protected UISprite useSprite;

    public void SetInput(ServantListViewItem item, bool isInput)
    {
        if ((item != null) && (this.baseButton != null))
        {
            this.baseButton.GetComponent<Collider>().enabled = isInput;
            this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
        }
    }

    public void SetItem(ServantListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            mode = DispMode.INVISIBLE;
        }
        if (mode != DispMode.INVISIBLE)
        {
            this.servantFaceIcon.Set(item.UserServant, item.IconInfo);
            if (this.partyIcon != null)
            {
                this.partyIcon.Set(item.IsParty);
            }
            if (this.info2IconLabel != null)
            {
                this.info2IconLabel.Set(item.IconInfo2);
            }
            if (this.useSprite != null)
            {
                this.useSprite.gameObject.SetActive(item.IsUse);
            }
            if (this.lockSprite != null)
            {
                this.lockSprite.gameObject.SetActive(item.IsLock);
            }
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

