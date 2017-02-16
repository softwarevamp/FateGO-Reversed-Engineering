using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(UIPanel)), AddComponentMenu("NGUI/Internal/Spring Panel")]
public class SpringPanel : MonoBehaviour
{
    public static SpringPanel current;
    private UIScrollView mDrag;
    private UIPanel mPanel;
    private Transform mTrans;
    public OnFinished onFinished;
    public float strength = 10f;
    public Vector3 target = Vector3.zero;

    protected virtual void AdvanceTowardsPosition()
    {
        float deltaTime = RealTime.deltaTime;
        bool flag = false;
        Vector3 localPosition = this.mTrans.localPosition;
        Vector3 target = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, deltaTime);
        Vector3 vector5 = target - this.target;
        if (vector5.sqrMagnitude < 0.01f)
        {
            target = this.target;
            base.enabled = false;
            flag = true;
        }
        this.mTrans.localPosition = target;
        Vector3 vector3 = target - localPosition;
        Vector2 clipOffset = this.mPanel.clipOffset;
        clipOffset.x -= vector3.x;
        clipOffset.y -= vector3.y;
        this.mPanel.clipOffset = clipOffset;
        if (this.mDrag != null)
        {
            this.mDrag.UpdateScrollbars(false);
        }
        if (flag && (this.onFinished != null))
        {
            current = this;
            this.onFinished();
            current = null;
        }
    }

    public static SpringPanel Begin(GameObject go, Vector3 pos, float strength)
    {
        SpringPanel component = go.GetComponent<SpringPanel>();
        if (component == null)
        {
            component = go.AddComponent<SpringPanel>();
        }
        component.target = pos;
        component.strength = strength;
        component.onFinished = null;
        component.enabled = true;
        return component;
    }

    private void Start()
    {
        this.mPanel = base.GetComponent<UIPanel>();
        this.mDrag = base.GetComponent<UIScrollView>();
        this.mTrans = base.transform;
    }

    private void Update()
    {
        this.AdvanceTowardsPosition();
    }

    public delegate void OnFinished();
}

