using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Scale")]
public class TweenScale : UITweener
{
    public Vector3 from = Vector3.one;
    private UITable mTable;
    private Transform mTrans;
    public Vector3 to = Vector3.one;
    public bool updateTable;

    public static TweenScale Begin(GameObject go, float duration, Vector3 scale)
    {
        TweenScale scale2 = UITweener.Begin<TweenScale>(go, duration);
        scale2.from = scale2.value;
        scale2.to = scale;
        if (duration <= 0f)
        {
            scale2.Sample(1f, true);
            scale2.enabled = false;
        }
        return scale2;
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        this.value = (Vector3) ((this.from * (1f - factor)) + (this.to * factor));
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

    public Transform cachedTransform
    {
        get
        {
            if (this.mTrans == null)
            {
                this.mTrans = base.transform;
            }
            return this.mTrans;
        }
    }

    [Obsolete("Use 'value' instead")]
    public Vector3 scale
    {
        get => 
            this.value;
        set
        {
            this.value = value;
        }
    }

    public Vector3 value
    {
        get => 
            this.cachedTransform.localScale;
        set
        {
            this.cachedTransform.localScale = value;
        }
    }
}

