using System;
using UnityEngine;

public class UserItemListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UIButton baseButton;
    [SerializeField]
    protected UISprite iconImageSprite;
    [SerializeField]
    protected ItemIconComponent itemIconInfo;
    [SerializeField]
    protected UILabel nameTextLabel;
    [SerializeField]
    protected UILabel numTextLabel;

    public void AddDepth(int v)
    {
        foreach (UIWidget widget in base.GetComponentsInChildren<UIWidget>())
        {
            widget.depth = v;
        }
    }

    public void SetInput(UserItemListViewItem item, bool isInput)
    {
        if (this.baseButton != null)
        {
            this.baseButton.GetComponent<Collider>().enabled = isInput;
            this.baseButton.SetState(UIButtonColor.State.Normal, true);
        }
    }

    public void SetItem(UserItemListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            mode = DispMode.INVISIBLE;
        }
        if (mode != DispMode.INVISIBLE)
        {
            this.itemIconInfo.SetCombineItem(item.ItemId, -1);
            this.nameTextLabel.text = item.ItemName;
            this.numTextLabel.text = LocalizationManager.GetUnitInfo(item.ItemNum);
            TweenColor component = this.iconImageSprite.gameObject.GetComponent<TweenColor>();
            if (component != null)
            {
                component.enabled = false;
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

