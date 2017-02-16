using System;
using UnityEngine;

public class CombineListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UIButton baseButton;
    [SerializeField]
    protected UISprite baseSprite;

    public void AddDepth(int v)
    {
        foreach (UIWidget widget in base.GetComponentsInChildren<UIWidget>())
        {
            widget.depth = v;
        }
    }

    public void SetItem(CombineListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            mode = DispMode.INVISIBLE;
        }
        if (mode != DispMode.INVISIBLE)
        {
            if (this.baseSprite != null)
            {
                this.baseSprite.spriteName = item.SpriteName;
            }
            if (this.baseButton != null)
            {
                bool enabled = this.baseButton.GetComponent<Collider>().enabled;
                this.baseButton.GetComponent<Collider>().enabled = true;
                this.baseButton.SetState(UIButtonColor.State.Normal, true);
                this.baseButton.GetComponent<Collider>().enabled = enabled;
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

