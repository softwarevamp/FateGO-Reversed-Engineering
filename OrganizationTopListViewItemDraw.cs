using System;
using UnityEngine;

public class OrganizationTopListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UIButton baseButton;
    [SerializeField]
    protected UISprite baseImageSprite;
    [SerializeField]
    protected UILabel infoTextLabel;

    public void AddDepth(int v)
    {
        foreach (UIWidget widget in base.GetComponentsInChildren<UIWidget>())
        {
            widget.depth = v;
        }
    }

    public void SetItem(OrganizationTopListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            mode = DispMode.INVISIBLE;
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
            if (this.baseButton != null)
            {
                this.baseButton.normalSprite = imageName;
            }
            this.baseImageSprite.spriteName = imageName;
            if (this.baseButton != null)
            {
                this.baseButton.SetState(UIButtonColor.State.Normal, true);
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

