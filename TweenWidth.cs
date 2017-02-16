using System;
using UnityEngine;

[RequireComponent(typeof(UIWidget)), AddComponentMenu("NGUI/Tween/Tween Width")]
public class TweenWidth : UITweener
{
    public int from = 100;
    private UITable mTable;
    private UIWidget mWidget;
    public int to = 100;
    public bool updateTable;

    public static TweenWidth Begin(UIWidget widget, float duration, int width)
    {
        TweenWidth width2 = UITweener.Begin<TweenWidth>(widget.gameObject, duration);
        width2.from = widget.width;
        width2.to = width;
        if (duration <= 0f)
        {
            width2.Sample(1f, true);
            width2.enabled = false;
        }
        return width2;
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        this.value = Mathf.RoundToInt((this.from * (1f - factor)) + (this.to * factor));
        if (this.updateTable)
        {
            if (this.mTable == null)
            {
                this.mTable = NGUITools.FindInParents<UITable>(base.gameObject);
                if (this.mTable == null)
                {
                    this.updateTable = false;
                    return;
                }
            }
            this.mTable.repositionNow = true;
        }
    }

    [ContextMenu("Assume value of 'To'")]
    private void SetCurrentValueToEnd()
    {
        this.value = this.to;
    }

    [ContextMenu("Assume value of 'From'")]
    private void SetCurrentValueToStart()
    {
        this.value = this.from;
    }

    [ContextMenu("Set 'To' to current value")]
    public override void SetEndToCurrentValue()
    {
        this.to = this.value;
    }

    [ContextMenu("Set 'From' to current value")]
    public override void SetStartToCurrentValue()
    {
        this.from = this.value;
    }

    public UIWidget cachedWidget
    {
        get
        {
            if (this.mWidget == null)
            {
                this.mWidget = base.GetComponent<UIWidget>();
            }
            return this.mWidget;
        }
    }

    public int value
    {
        get => 
            this.cachedWidget.width;
        set
        {
            this.cachedWidget.width = value;
        }
    }

    [Obsolete("Use 'value' instead")]
    public int width
    {
        get => 
            this.value;
        set
        {
            this.value = value;
        }
    }
}

