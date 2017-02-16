using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Lag Rotation")]
public class LagRotation : MonoBehaviour
{
    public bool ignoreTimeScale;
    private Quaternion mAbsolute;
    private Quaternion mRelative;
    private Transform mTrans;
    public float speed = 10f;

    private void Interpolate(float delta)
    {
        if (this.mTrans != null)
        {
            Transform parent = this.mTrans.parent;
            if (parent != null)
            {
                this.mAbsolute = Quaternion.Slerp(this.mAbsolute, parent.rotation * this.mRelative, delta * this.speed);
                this.mTrans.rotation = this.mAbsolute;
            }
        }
    }

    public void OnRepositionEnd()
    {
        this.Interpolate(1000f);
    }

    private void Start()
    {
        this.mTrans = base.transform;
        this.mRelative = this.mTrans.localRotation;
        this.mAbsolute = this.mTrans.rotation;
    }

    private void Update()
    {
        this.Interpolate(!this.ignoreTimeScale ? Time.deltaTime : RealTime.deltaTime);
    }
}

