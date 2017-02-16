using System;
using UnityEngine;

public class PartyServantListViewItemDraw : MonoBehaviour
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

    public void SetInput(PartyServantListViewItem item, bool isInput)
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

    public void SetItem(PartyServantListViewItem item, DispMode mode)
    {
        if ((item != null) && (mode != DispMode.INVISIBLE))
        {
            bool isBase = item.IsBase;
            this.servantFaceIcon.Set(item.UserServant, item.GetEquipList(), item.IconInfo);
            this.costIconLabel.Set(IconLabelInfo.IconKind.COST, item.Cost, 0, item.EquipCost, 0L, false, false);
            if (isBase)
            {
                if (this.partyIcon != null)
                {
                    this.partyIcon.Set(false);
                }
            }
            else if (this.partyIcon != null)
            {
                this.partyIcon.Set(item.IsParty);
            }
            this.lockSprite.gameObject.SetActive(item.IsLock);
            if (isBase)
            {
                this.maskSprite.gameObject.SetActive(false);
                this.warningLabel.text = string.Empty;
            }
            else if (item.IsActionEventJoinLeader)
            {
                this.maskSprite.gameObject.SetActive(true);
                if (item.IsSelectLeader)
                {
                    this.warningLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_SERVANT_EVENT_JOIN");
                }
                else
                {
                    this.warningLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_SERVANT_EVENT_JOIN_LEADER");
                }
            }
            else if (item.IsSame)
            {
                this.maskSprite.gameObject.SetActive(true);
                this.warningLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_SERVANT_SAME_SERVANT");
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

