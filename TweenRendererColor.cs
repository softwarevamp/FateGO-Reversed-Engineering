using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Renderer Color")]
public class TweenRendererColor : UITweener
{
    public Color from = Color.white;
    private bool mCached;
    private UITweenRenderer mTweenRenderer;
    public Color to = Color.white;

    public static TweenRendererColor Begin(GameObject go, float duration, Color color)
    {
        TweenRendererColor color2 = UITweener.Begin<TweenRendererColor>(go, duration);
        color2.from = color2.value;
        color2.to = color;
        if (duration <= 0f)
        {
            color2.Sample(1f, true);
            color2.enabled = false;
        }
        return color2;
    }

    private void Cache()
    {
        this.mCached = true;
        this.mTweenRenderer = base.GetComponent<UITweenRenderer>();
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        this.value = Color.Lerp(this.from, this.to, factor);
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

    [Obsolete("Use 'value' instead")]
    public Color color
    {
        get => 
            this.value;
        set
        {
            this.value = value;
        }
    }

    public Color value
    {
        get
        {
            if (!this.mCached)
            {
                this.Cache();
            }
            if (this.mTweenRenderer != null)
            {
                return this.mTweenRenderer.GetTweenColor();
            }
            return Color.black;
        }
        set
        {
            if (!this.mCached)
            {
                this.Cache();
            }
            if (this.mTweenRenderer != null)
            {
                this.mTweenRenderer.SetTweenColor(value);
            }
        }
    }
}

