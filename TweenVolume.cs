using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource)), AddComponentMenu("NGUI/Tween/Tween Volume")]
public class TweenVolume : UITweener
{
    [Range(0f, 1f)]
    public float from = 1f;
    private AudioSource mSource;
    [Range(0f, 1f)]
    public float to = 1f;

    public static TweenVolume Begin(GameObject go, float duration, float targetVolume)
    {
        TweenVolume volume = UITweener.Begin<TweenVolume>(go, duration);
        volume.from = volume.value;
        volume.to = targetVolume;
        return volume;
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        this.value = (this.from * (1f - factor)) + (this.to * factor);
        this.mSource.enabled = this.mSource.volume > 0.01f;
    }

    public override void SetEndToCurrentValue()
    {
        this.to = this.value;
    }

    public override void SetStartToCurrentValue()
    {
        this.from = this.value;
    }

    public AudioSource audioSource
    {
        get
        {
            if (this.mSource == null)
            {
                this.mSource = base.GetComponent<AudioSource>();
                if (this.mSource == null)
                {
                    this.mSource = base.GetComponent<AudioSource>();
                    if (this.mSource == null)
                    {
                        Debug.LogError("TweenVolume needs an AudioSource to work with", this);
                        base.enabled = false;
                    }
                }
            }
            return this.mSource;
        }
    }

    public float value
    {
        get => 
            ((this.audioSource == null) ? 0f : this.mSource.volume);
        set
        {
            if (this.audioSource != null)
            {
                this.mSource.volume = value;
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

