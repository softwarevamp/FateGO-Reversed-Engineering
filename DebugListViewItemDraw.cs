using System;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/DebugListViewItemDraw")]
public class DebugListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UITexture baseImageTexture;
    [SerializeField]
    protected UILabel eventTextLabel;

    public void AddDepth(int v)
    {
        foreach (UIWidget widget in base.GetComponentsInChildren<UIWidget>())
        {
            widget.depth = v;
        }
    }

    public void SetItem(DebugListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            mode = DispMode.INVISIBLE;
        }
        if (mode != DispMode.INVISIBLE)
        {
            if (item.TitleText != null)
            {
                this.eventTextLabel.text = item.TitleText;
            }
            TweenColor component = this.baseImageTexture.gameObject.GetComponent<TweenColor>();
            if (component != null)
            {
                component.enabled = false;
            }
            this.baseImageTexture.color = (mode != DispMode.INVALID) ? Color.white : Color.gray;
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

