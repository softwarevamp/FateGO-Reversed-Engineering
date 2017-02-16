using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/NGUI UITweenRenderer"), ExecuteInEditMode]
public class UITweenRenderer : UIWidget
{
    protected float volume;

    public virtual Color GetTweenColor() => 
        base.color;

    public virtual float GetTweenVolume() => 
        this.volume;

    public virtual void SetTweenColor(Color c)
    {
        base.color = c;
    }

    public virtual void SetTweenVolume(float v)
    {
        this.volume = v;
    }
}

