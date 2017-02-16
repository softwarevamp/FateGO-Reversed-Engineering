using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Position")]
public class TweenPosition : UITweener
{
    public Vector3 from;
    private UIRect mRect;
    private Transform mTrans;
    public Vector3 to;
    [HideInInspector]
    public bool worldSpace;

    private void Awake()
    {
        this.mRect = base.GetComponent<UIRect>();
    }

    public static TweenPosition Begin(GameObject go, float duration, Vector3 pos)
    {
        TweenPosition position = UITweener.Begin<TweenPosition>(go, duration);
        position.from = position.value;
        position.to = pos;
        if (duration <= 0f)
        {
            position.Sample(1f, true);
            position.enabled = false;
        }
        return position;
    }

    public static TweenPosition Begin(GameObject go, float duration, Vector3 pos, bool worldSpace)
    {
        TweenPosition position = UITweener.Begin<TweenPosition>(go, duration);
        position.worldSpace = worldSpace;
        position.from = position.value;
        position.to = pos;
        if (duration <= 0f)
        {
            position.Sample(1f, true);
            position.enabled = false;
        }
        return position;
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        this.value = (Vector3) ((this.from * (1f - factor)) + (this.to * factor));
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
    public Vector3 position
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
            (!this.worldSpace ? this.cachedTransform.localPosition : this.cachedTransform.position);
        set
        {
            if (((this.mRect == null) || !this.mRect.isAnchored) || this.worldSpace)
            {
                if (this.worldSpace)
                {
                    this.cachedTransform.position = value;
                }
                else
                {
                    this.cachedTransform.localPosition = value;
                }
            }
            else
            {
                value -= this.cachedTransform.localPosition;
                NGUIMath.MoveRect(this.mRect, value.x, value.y);
            }
        }
    }
}

