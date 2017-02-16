using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Rotation")]
public class TweenRotation : UITweener
{
    public Vector3 from;
    private Transform mTrans;
    public bool quaternionLerp;
    public Vector3 to;

    public static TweenRotation Begin(GameObject go, float duration, Quaternion rot)
    {
        TweenRotation rotation = UITweener.Begin<TweenRotation>(go, duration);
        rotation.from = rotation.value.eulerAngles;
        rotation.to = rot.eulerAngles;
        if (duration <= 0f)
        {
            rotation.Sample(1f, true);
            rotation.enabled = false;
        }
        return rotation;
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        this.value = !this.quaternionLerp ? Quaternion.Euler(new Vector3(Mathf.Lerp(this.from.x, this.to.x, factor), Mathf.Lerp(this.from.y, this.to.y, factor), Mathf.Lerp(this.from.z, this.to.z, factor))) : Quaternion.Slerp(Quaternion.Euler(this.from), Quaternion.Euler(this.to), factor);
    }

    [ContextMenu("Assume value of 'To'")]
    private void SetCurrentValueToEnd()
    {
        this.value = Quaternion.Euler(this.to);
    }

    [ContextMenu("Assume value of 'From'")]
    private void SetCurrentValueToStart()
    {
        this.value = Quaternion.Euler(this.from);
    }

    [ContextMenu("Set 'To' to current value")]
    public override void SetEndToCurrentValue()
    {
        this.to = this.value.eulerAngles;
    }

    [ContextMenu("Set 'From' to current value")]
    public override void SetStartToCurrentValue()
    {
        this.from = this.value.eulerAngles;
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
    public Quaternion rotation
    {
        get => 
            this.value;
        set
        {
            this.value = value;
        }
    }

    public Quaternion value
    {
        get => 
            this.cachedTransform.localRotation;
        set
        {
            this.cachedTransform.localRotation = value;
        }
    }
}

