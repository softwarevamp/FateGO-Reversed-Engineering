using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Alpha")]
public class TweenAlpha : UITweener
{
    [Range(0f, 1f)]
    public float from = 1f;
    private bool mCached;
    private Material mMat;
    private UIRect mRect;
    private SpriteRenderer mSr;
    [Range(0f, 1f)]
    public float to = 1f;

    public static TweenAlpha Begin(GameObject go, float duration, float alpha)
    {
        TweenAlpha alpha2 = UITweener.Begin<TweenAlpha>(go, duration);
        alpha2.from = alpha2.value;
        alpha2.to = alpha;
        if (duration <= 0f)
        {
            alpha2.Sample(1f, true);
            alpha2.enabled = false;
        }
        return alpha2;
    }

    private void Cache()
    {
        this.mCached = true;
        this.mRect = base.GetComponent<UIRect>();
        this.mSr = base.GetComponent<SpriteRenderer>();
        if ((this.mRect == null) && (this.mSr == null))
        {
            Renderer component = base.GetComponent<Renderer>();
            if (component != null)
            {
                this.mMat = component.material;
            }
            if (this.mMat == null)
            {
                this.mRect = base.GetComponentInChildren<UIRect>();
            }
        }
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        this.value = Mathf.Lerp(this.from, this.to, factor);
    }

    public override void SetEndToCurrentValue()
    {
        this.to = this.value;
    }

    public override void SetStartToCurrentValue()
    {
        this.from = this.value;
    }

    [Obsolete("Use 'value' instead")]
    public float alpha
    {
        get => 
            this.value;
        set
        {
            this.value = value;
        }
    }

    public float value
    {
        get
        {
            if (!this.mCached)
            {
                this.Cache();
            }
            if (this.mRect != null)
            {
                return this.mRect.alpha;
            }
            if (this.mSr != null)
            {
                return this.mSr.color.a;
            }
            return ((this.mMat == null) ? 1f : this.mMat.color.a);
        }
        set
        {
            if (!this.mCached)
            {
                this.Cache();
            }
            if (this.mRect != null)
            {
                this.mRect.alpha = value;
            }
            else if (this.mSr != null)
            {
                Color color = this.mSr.color;
                color.a = value;
                this.mSr.color = color;
            }
            else if (this.mMat != null)
            {
                Color color2 = this.mMat.color;
                color2.a = value;
                this.mMat.color = color2;
            }
        }
    }
}

