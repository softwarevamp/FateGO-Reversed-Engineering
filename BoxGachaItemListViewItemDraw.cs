using System;
using UnityEngine;

public class BoxGachaItemListViewItemDraw : MonoBehaviour
{
    protected UIAtlas baseAtlas;
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UISprite baseSprite;
    protected string baseSpriteName;
    [SerializeField]
    protected UILabel currentNumLabel;
    [SerializeField]
    protected ItemIconComponent itemIcon;
    [SerializeField]
    protected UILabel maxNumLabel;
    [SerializeField]
    protected UILabel msgTextLabel;
    [SerializeField]
    protected UICrossNarrowLabel nameTextLabel;
    [SerializeField]
    protected GameObject rareDispImgObj;
    [SerializeField]
    protected UISprite rareDispSprite;

    protected void Awake()
    {
        if (this.baseSprite != null)
        {
            this.baseAtlas = this.baseSprite.atlas;
            this.baseSpriteName = this.baseSprite.spriteName;
        }
    }

    public void SetItem(BoxGachaItemListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            mode = DispMode.INVISIBLE;
        }
        if (mode != DispMode.INVISIBLE)
        {
            this.rareDispImgObj.SetActive(false);
            if ((this.baseSprite != null) && (item.GetEventId > 0))
            {
                EventGachaRootComponent.SetBanner(this.baseSprite, "event_rewardgacha_" + item.GetEventId);
            }
            if (item.GachaBaseType == RewardType.Type.GIFT)
            {
                this.itemIcon.SetGift(item.GiftType, item.TargetObjectId, -1);
            }
            if ((item.GachaBaseType == RewardType.Type.EXTRA) || (item.GachaBaseType == RewardType.Type.SET))
            {
                this.itemIcon.SetItemImage((ImageItem.Id) item.TargetImgId);
            }
            this.nameTextLabel.SetCrossNarrowText(item.NameTxt);
            this.msgTextLabel.text = item.DetailTxt;
            this.currentNumLabel.text = item.CurrentNum.ToString();
            this.currentNumLabel.color = Color.white;
            if (item.IsDraw)
            {
                this.currentNumLabel.color = Color.red;
            }
            this.maxNumLabel.text = item.MaxNum.ToString();
            if (this.baseButton != null)
            {
                if (item.GetIconId > 0)
                {
                    this.baseButton.SetColliderEnable(true, true);
                    this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
                    EventGachaRootComponent.setRewardInfoImg(this.rareDispSprite, "icon_event_" + item.GetIconId);
                    this.rareDispImgObj.SetActive(true);
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

