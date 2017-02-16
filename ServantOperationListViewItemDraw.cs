using System;
using UnityEngine;

public class ServantOperationListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UISprite lockSprite;
    [SerializeField]
    protected UISprite maskSprite;
    [SerializeField]
    protected UISprite maskTitleSprite;
    [SerializeField]
    protected FlashingIconComponent partyIcon;
    [SerializeField]
    protected GameObject selectObject;
    [SerializeField]
    protected UILabel selectTextLabel;
    [SerializeField]
    protected ServantFaceIconComponent servantFaceIcon;
    [SerializeField]
    protected UISprite useSprite;
    [SerializeField]
    protected UILabel warningLabel;

    public void SetInput(ServantOperationListViewItem item, bool isSelectEnable)
    {
        bool flag = false;
        bool flag2 = false;
        bool isSelect = false;
        if (item != null)
        {
            isSelect = item.IsSelect;
            if (item.IsCanNotSelect)
            {
                flag = true;
            }
            if (item.IsOrganization)
            {
                flag2 = true;
            }
        }
        if (this.baseButton != null)
        {
            this.baseButton.isEnabled = true;
            this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
        }
        if (flag)
        {
            this.maskTitleSprite.gameObject.SetActive(false);
            this.maskSprite.gameObject.SetActive(true);
        }
        else if ((flag2 && !isSelectEnable) && !isSelect)
        {
            this.maskTitleSprite.gameObject.SetActive(false);
            this.maskSprite.gameObject.SetActive(true);
        }
        else
        {
            this.maskTitleSprite.gameObject.SetActive(false);
            this.maskSprite.gameObject.SetActive(false);
        }
        if (this.selectObject != null)
        {
            if (isSelect)
            {
                this.selectObject.SetActive(true);
                this.selectTextLabel.text = string.Empty + ((item.SelectNum + 1)).ToString();
            }
            else
            {
                this.selectObject.SetActive(false);
                this.selectTextLabel.text = string.Empty;
            }
        }
    }

    public void SetItem(ServantOperationListViewItem item, DispMode mode, bool isSelectEnable)
    {
        if ((item != null) && (mode != DispMode.INVISIBLE))
        {
            string str;
            bool isSelect = item.IsSelect;
            this.servantFaceIcon.Set(item.UserServant, item.IconInfo);
            if (this.partyIcon != null)
            {
                this.partyIcon.Set(item.IsParty);
            }
            if (this.useSprite != null)
            {
                this.useSprite.gameObject.SetActive(item.IsUse);
            }
            if (this.lockSprite != null)
            {
                this.lockSprite.gameObject.SetActive(item.IsLock);
            }
            if (this.selectObject != null)
            {
                if (item.IsSelect)
                {
                    this.selectObject.SetActive(true);
                    this.selectTextLabel.text = string.Empty + (item.SelectNum + 1);
                }
                else
                {
                    this.selectObject.SetActive(false);
                    this.selectTextLabel.text = string.Empty;
                }
            }
            if (!item.IsSellEnableServant)
            {
                str = LocalizationManager.Get("SELECT_CANNOT");
            }
            else if (item.IsLock)
            {
                str = LocalizationManager.Get("SELECT_LOCK");
            }
            else if (item.IsFavorite)
            {
                str = LocalizationManager.Get("SELECT_FAVORITE");
            }
            else if (item.IsPartyEquip)
            {
                str = LocalizationManager.Get("SELECT_PARTY_EQUIP");
            }
            else if (item.IsParty)
            {
                str = LocalizationManager.Get("SELECT_PARTY");
            }
            else if (item.IsUseSupportServant)
            {
                str = LocalizationManager.Get("SUPPORT_MEMBER");
            }
            else if (item.IsUseSupportEquip)
            {
                str = LocalizationManager.Get("SUPPORT_EQUIP");
            }
            else if (item.IsUse)
            {
                str = LocalizationManager.Get("COMMON_EQUIP_USED");
            }
            else
            {
                str = string.Empty;
            }
            this.warningLabel.text = str;
            if (this.baseButton != null)
            {
                this.baseButton.isEnabled = true;
                this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
            }
            if (item.IsCanNotSelect)
            {
                this.maskTitleSprite.gameObject.SetActive(false);
                this.maskSprite.gameObject.SetActive(true);
            }
            else if (!isSelectEnable && !isSelect)
            {
                this.maskTitleSprite.gameObject.SetActive(false);
                this.maskSprite.gameObject.SetActive(true);
            }
            else
            {
                this.maskTitleSprite.gameObject.SetActive(false);
                this.maskSprite.gameObject.SetActive(false);
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

