using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Orthographic Size"), RequireComponent(typeof(Camera))]
public class TweenOrthoSize : UITweener
{
    public float from = 1f;
    private Camera mCam;
    public float to = 1f;

    public static TweenOrthoSize Begin(GameObject go, float duration, float to)
    {
        TweenOrthoSize size = UITweener.Begin<TweenOrthoSize>(go, duration);
        size.from = size.value;
        size.to = to;
        if (duration <= 0f)
        {
            size.Sample(1f, true);
            size.enabled = false;
        }
        return size;
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

    public Camera cachedCamera
    {
        get
        {
            if (this.mCam == null)
            {
                this.mCam = base.GetComponent<Camera>();
            }
            return this.mCam;
        }
    }

    [Obsolete("Use 'value' instead")]
    public float orthoSize
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
        get => 
            this.cachedCamera.orthographicSize;
        set
        {
            this.cachedCamera.orthographicSize = value;
        }
    }
}

