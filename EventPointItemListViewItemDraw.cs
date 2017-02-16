using System;
using UnityEngine;

public class EventPointItemListViewItemDraw : MonoBehaviour
{
    protected UIAtlas baseAtlas;
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UISprite baseSprite;
    protected string baseSpriteName;
    [SerializeField]
    protected GameObject getRewardImgObj;
    [SerializeField]
    protected ItemIconComponent itemIcon;
    [SerializeField]
    protected UILabel msgTextLabel;
    [SerializeField]
    protected UICrossNarrowLabel nameTextLabel;
    [SerializeField]
    protected UICrossNarrowLabel numTextLabel;

    protected void Awake()
    {
        if (this.baseSprite != null)
        {
            this.baseAtlas = this.baseSprite.atlas;
            this.baseSpriteName = this.baseSprite.spriteName;
        }
    }

    public void SetItem(EventPointItemListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            mode = DispMode.INVISIBLE;
        }
        if (mode != DispMode.INVISIBLE)
        {
            this.getRewardImgObj.SetActive(false);
            if (this.baseSprite != null)
            {
                Debug.Log("!!** !! SetItem EventId: " + item.GetEventId);
                if (item.GetEventId > 0)
                {
                    EventGachaRootComponent.SetBanner(this.baseSprite, "event_rewardpoint_" + item.GetEventId);
                }
            }
            if (item.eventRewardType == RewardType.Type.GIFT)
            {
                this.itemIcon.SetGift(item.Type, item.RewardObjId, -1);
            }
            if ((item.eventRewardType == RewardType.Type.SET) || (item.eventRewardType == RewardType.Type.EXTRA))
            {
                this.itemIcon.SetItemImage((ImageItem.Id) item.SetExtraIconId);
            }
            this.nameTextLabel.SetCrossNarrowText(item.NameText);
            this.numTextLabel.SetCrossNarrowText(item.NumText);
            this.msgTextLabel.text = item.needPointTxt;
            if (this.baseButton != null)
            {
                if (item.IsGetReward)
                {
                    this.baseButton.SetColliderEnable(true, true);
                    this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
                    this.getRewardImgObj.SetActive(true);
                }
                else
                {
                    this.baseButton.SetColliderEnable(true, true);
                    this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
                }
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

