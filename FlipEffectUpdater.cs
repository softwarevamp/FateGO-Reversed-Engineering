using System;
using UnityEngine;

public class FlipEffectUpdater : MonoBehaviour
{
    public Transform ConnectTarget;
    private Transform mTrans;

    private void LateUpdate()
    {
        this.OnLateUpdate();
    }

    public void OnLateUpdate()
    {
        if (this.ConnectTarget != null)
        {
            if (this.mTrans == null)
            {
                this.mTrans = base.transform;
            }
            Transform parent = this.mTrans.parent;
            this.mTrans.parent = this.ConnectTarget;
            this.mTrans.localPosition = Vector3.zero;
            this.mTrans.localEulerAngles = Vector3.zero;
            this.mTrans.localScale = Vector3.one;
            this.mTrans.parent = parent;
        }
    }

    private void Start()
    {
        this.mTrans = base.transform;
    }
}

