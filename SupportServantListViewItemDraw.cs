using System;
using UnityEngine;

public class SupportServantListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UIIconLabel info2IconLabel;
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

    public void SetInput(SupportServantListViewItem item, bool isInput)
    {
        if (item != null)
        {
            bool isBase = item.IsBase;
            if (this.baseButton != null)
            {
                this.baseButton.GetComponent<Collider>().enabled = isInput;
                this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
            }
            if (this.selectObject != null)
            {
                this.selectObject.SetActive(isBase);
            }
        }
    }

    public void SetItem(SupportServantListViewItem item, DispMode mode)
    {
        if ((item != null) && (mode != DispMode.INVISIBLE))
        {
            bool isBase = item.IsBase;
            this.servantFaceIcon.Set(item.UserServant, item.GetEquipList(), item.IconInfo);
            this.info2IconLabel.Set(item.IconInfo2);
            if (isBase && (this.partyIcon != null))
            {
                this.partyIcon.Set(false);
            }
            this.lockSprite.gameObject.SetActive(item.IsLock);
            if (item.IsBase)
            {
                if (isBase)
                {
                    this.maskSprite.gameObject.SetActive(false);
                    this.warningLabel.text = string.Empty;
                }
                else
                {
                    this.maskSprite.gameObject.SetActive(true);
                    this.warningLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_SERVANT_REMOVE_LEADER");
                }
            }
            else if (item.IsSame)
            {
                this.maskSprite.gameObject.SetActive(true);
                this.warningLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_SERVANT_SAME_SERVANT");
            }
            else if (item.IsUseServant)
            {
                this.maskSprite.gameObject.SetActive(true);
                this.warningLabel.text = LocalizationManager.Get("SUPPORT_SELECT_USE_SUPPORT");
            }
            else
            {
                this.maskSprite.gameObject.SetActive(false);
                this.warningLabel.text = string.Empty;
            }
            if (this.selectObject != null)
            {
                this.selectObject.SetActive(isBase);
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

