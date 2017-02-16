using System;
using UnityEngine;

public class EquipGraphListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UIIconLabel costIconLabel;
    [SerializeField]
    protected UISprite lockSprite;
    [SerializeField]
    protected UISprite maskSprite;
    [SerializeField]
    protected FlashingIconComponent partyIcon;
    [SerializeField]
    protected GameObject selectObject;
    [SerializeField]
    protected ServantFaceIconComponent servantFaceIcon;
    [SerializeField]
    protected UILabel warningLabel;

    public void SetInput(EquipGraphListViewItem item, bool isInput)
    {
        if (item != null)
        {
            if (this.baseButton != null)
            {
                this.baseButton.GetComponent<Collider>().enabled = isInput;
                this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
            }
            if (this.selectObject != null)
            {
                this.selectObject.SetActive(item.IsBase);
            }
        }
    }

    public void SetItem(EquipGraphListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            mode = DispMode.INVISIBLE;
        }
        if (mode != DispMode.INVISIBLE)
        {
            this.servantFaceIcon.Set(item.UserServant, item.IconInfo);
            this.costIconLabel.Set(IconLabelInfo.IconKind.COST, item.Cost, 0, 0, 0L, false, false);
            if (this.partyIcon != null)
            {
                this.partyIcon.Set(item.IsUse);
            }
            if (this.lockSprite != null)
            {
                this.lockSprite.gameObject.SetActive(item.IsLock);
            }
            if (item.IsBase)
            {
                this.maskSprite.gameObject.SetActive(false);
                this.warningLabel.text = string.Empty;
            }
            else if (item.IsUse)
            {
                this.maskSprite.gameObject.SetActive(true);
                this.warningLabel.text = LocalizationManager.Get("EQUIP_GRAPH_USE");
            }
            else
            {
                this.maskSprite.gameObject.SetActive(false);
                this.warningLabel.text = string.Empty;
            }
            if (this.selectObject != null)
            {
                this.selectObject.SetActive(item.IsBase);
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

