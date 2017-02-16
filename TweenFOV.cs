using System;
using UnityEngine;

[RequireComponent(typeof(Camera)), AddComponentMenu("NGUI/Tween/Tween Field of View")]
public class TweenFOV : UITweener
{
    public float from = 45f;
    private Camera mCam;
    public float to = 45f;

    public static TweenFOV Begin(GameObject go, float duration, float to)
    {
        TweenFOV nfov = UITweener.Begin<TweenFOV>(go, duration);
        nfov.from = nfov.value;
        nfov.to = to;
        if (duration <= 0f)
        {
            nfov.Sample(1f, true);
            nfov.enabled = false;
        }
        return nfov;
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        this.value = (this.from * (1f - factor)) + (this.to * factor);
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
    public float fov
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
            this.cachedCamera.fieldOfView;
        set
        {
            this.cachedCamera.fieldOfView = value;
        }
    }
}

