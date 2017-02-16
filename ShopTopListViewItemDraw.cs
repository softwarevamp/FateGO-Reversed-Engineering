using System;
using UnityEngine;

public class ShopTopListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UISprite baseImageSprite;
    [SerializeField]
    protected UILabel infoTextLabel;
    [SerializeField]
    protected GameObject periodBase;
    [SerializeField]
    protected UISprite rangeSprite;

    public void SetInput(ShopTopListViewItem item, bool isInput)
    {
        if (item != null)
        {
            if (!item.IsUse)
            {
                if (this.baseButton != null)
                {
                    this.baseButton.SetState(UICommonButtonColor.State.Disabled, true);
                }
            }
            else if (isInput)
            {
                if (this.baseButton != null)
                {
                    this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
                }
            }
            else if (this.baseButton != null)
            {
                this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
            }
        }
    }

    public void SetItem(ShopTopListViewItem item, DispMode mode)
    {
        if (item != null)
        {
            if (this.rangeSprite != null)
            {
                this.rangeSprite.gameObject.SetActive(mode == DispMode.INVISIBLE);
            }
            if (mode != DispMode.INVISIBLE)
            {
                string imageName = item.ImageName;
                if (string.IsNullOrEmpty(imageName))
                {
                    imageName = "img_shop_0";
                    this.infoTextLabel.text = item.InfoText;
                }
                else
                {
                    this.infoTextLabel.text = string.Empty;
                }
                this.baseImageSprite.spriteName = imageName;
                if (this.periodBase != null)
                {
                    this.periodBase.SetActive(item.IsPeriod);
                }
                if (this.baseButton != null)
                {
                    if (!item.IsUse)
                    {
                        this.baseButton.SetState(UICommonButtonColor.State.Disabled, true);
                    }
                    else
                    {
                        this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
                    }
                }
            }
        }
    }

    public enum DispMode
    {
        INVISIBLE,
        INVALID,
        VALID
    }
}

