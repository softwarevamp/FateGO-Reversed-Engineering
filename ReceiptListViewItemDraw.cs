﻿using System;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ReceiptListViewItemDraw")]
public class ReceiptListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UITexture baseImageTexture;
    [SerializeField]
    protected UILabel timeTextLabel;
    [SerializeField]
    protected UILabel titleTextLabel;

    public void AddDepth(int v)
    {
        foreach (UIWidget widget in base.GetComponentsInChildren<UIWidget>())
        {
            widget.depth = v;
        }
    }

    public void SetItem(ReceiptListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            mode = DispMode.INVISIBLE;
        }
        if (mode != DispMode.INVISIBLE)
        {
            if (item.TitleText != null)
            {
                this.titleTextLabel.text = item.TitleText;
            }
            else
            {
                this.titleTextLabel.text = "?";
            }
            this.timeTextLabel.text = item.TimeText;
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

