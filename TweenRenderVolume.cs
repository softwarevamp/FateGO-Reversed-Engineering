using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Render Volume")]
public class TweenRenderVolume : UITweener
{
    [Range(0f, 1f)]
    public float from = 1f;
    private bool mCached;
    private UITweenRenderer mTweenRenderer;
    [Range(0f, 1f)]
    public float to = 1f;

    public static TweenRenderVolume Begin(GameObject go, float duration, float targetVolume)
    {
        TweenRenderVolume volume = UITweener.Begin<TweenRenderVolume>(go, duration);
        volume.from = volume.value;
        volume.to = targetVolume;
        return volume;
    }

    private void Cache()
    {
        this.mCached = true;
        this.mTweenRenderer = base.GetComponent<UITweenRenderer>();
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        this.value = (this.from * (1f - factor)) + (this.to * factor);
    }

    public override void SetEndToCurrentValue()
    {
        this.to = this.value;
    }

    public override void SetStartToCurrentValue()
    {
        this.from = this.value;
    }

    public float value
    {
        get
        {
            if (!this.mCached)
            {
                this.Cache();
            }
            if (this.mTweenRenderer != null)
            {
                return this.mTweenRenderer.GetTweenVolume();
            }
            return 0f;
        }
        set
        {
            if (!this.mCached)
            {
                this.Cache();
            }
            if (this.mTweenRenderer != null)
            {
                this.mTweenRenderer.SetTweenVolume(value);
            }
        }
    }

    [Obsolete("Use 'value' instead")]
    public float volume
    {
        get => 
            this.value;
        set
        {
            this.value = value;
        }
    }
}

