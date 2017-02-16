using System;
using UnityEngine;

public class ShopEventListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UISprite addRangeSprite;
    protected UIAtlas baseAtlas;
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UISprite baseSprite;
    protected string baseSpriteName;
    [SerializeField]
    protected UILabel changePeriodLabel;
    [SerializeField]
    protected UILabel eventPeriodLabel;
    [SerializeField]
    protected GameObject infoBase;
    [SerializeField]
    protected UILabel messageTextLabel;
    [SerializeField]
    protected UILabel nameTextLabel;
    [SerializeField]
    protected UISprite rangeSprite;

    protected void Awake()
    {
        if (this.baseSprite != null)
        {
            this.baseAtlas = this.baseSprite.atlas;
            this.baseSpriteName = this.baseSprite.spriteName;
        }
    }

    public void SetItem(ShopEventListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            if (this.rangeSprite != null)
            {
                this.rangeSprite.gameObject.SetActive(false);
            }
            if (this.addRangeSprite != null)
            {
                this.addRangeSprite.gameObject.SetActive(false);
            }
        }
        else
        {
            if (this.rangeSprite != null)
            {
                this.rangeSprite.gameObject.SetActive(mode == DispMode.INVISIBLE);
            }
            if (this.addRangeSprite != null)
            {
                this.addRangeSprite.gameObject.SetActive(item.IsTerminationSpace);
            }
            if (mode != DispMode.INVISIBLE)
            {
                bool flag = false;
                if (this.baseSprite != null)
                {
                    if (AtlasManager.SetShopBanner(this.baseSprite, item.EventId))
                    {
                        flag = true;
                    }
                    else if (this.baseAtlas != null)
                    {
                        this.baseSprite.atlas = this.baseAtlas;
                        this.baseSprite.spriteName = this.baseSpriteName;
                    }
                }
                this.infoBase.SetActive(!flag);
                this.nameTextLabel.text = item.NameText;
                this.messageTextLabel.text = "[000000]" + item.DetailText;
                this.eventPeriodLabel.text = LocalizationManager.GetPeriod(item.EventStartedAt, item.EventEndedAt, false, false);
                this.changePeriodLabel.text = LocalizationManager.GetPeriod(item.ChangeStartedAt, item.ChangeEndedAt, false, true);
                if (this.baseButton != null)
                {
                    if (item.IsFinished)
                    {
                        this.baseButton.SetColliderEnable(true, true);
                        this.baseButton.isEnabled = false;
                        this.baseButton.SetState(UICommonButtonColor.State.Disabled, true);
                    }
                    else
                    {
                        this.baseButton.isEnabled = true;
                        this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
                        this.baseButton.SetColliderEnable(mode == DispMode.INPUT, true);
                    }
                }
                else if (this.baseSprite != null)
                {
                    this.baseSprite.color = !item.IsFinished ? UICommonButtonColor.normal : UICommonButtonColor.disabledColor;
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

