using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ScrollArrowComponent : MonoBehaviour
{
    protected static List<ScrollArrowComponent> _arrowList = new List<ScrollArrowComponent>();
    [SerializeField]
    private DirectionType directionType;
    public static readonly int MOVE_RANGE = 20;
    public static readonly float MOVE_TIME = 2.5f;
    [SerializeField]
    private int moveRange = MOVE_RANGE;
    [SerializeField]
    private float moveTime = MOVE_TIME;
    private float mTgtAlp;
    private UIWidget mWidget;
    public static readonly float TGT_ALP_SPD_RATE = 0.25f;

    private void Awake()
    {
        if (this.mWidget == null)
        {
            this.mWidget = base.gameObject.GetComponent<UIWidget>();
            if (this.mWidget != null)
            {
                this.mTgtAlp = this.mWidget.alpha;
            }
        }
        if (!_arrowList.Contains(this))
        {
            _arrowList.Add(this);
        }
        TweenPosition component = base.gameObject.GetComponent<TweenPosition>();
        if (component != null)
        {
            Vector3 zero = Vector3.zero;
            switch (this.directionType)
            {
                case DirectionType.Left:
                    zero.x = -this.moveRange;
                    break;

                case DirectionType.Right:
                    zero.x = this.moveRange;
                    break;
            }
            component.duration = this.moveTime;
            Vector3 localPosition = base.gameObject.GetLocalPosition();
            component.from = localPosition;
            component.to = localPosition + zero;
        }
    }

    protected ScrollArrowComponent FetchActiveArrow()
    {
        foreach (ScrollArrowComponent component in _arrowList)
        {
            if (((component != null) && component.enabled) && (component != this))
            {
                return component;
            }
        }
        return null;
    }

    private void OnDestroy()
    {
        _arrowList.Remove(this);
    }

    private void OnEnable()
    {
        this.SyncAnimation();
    }

    public void SetDisp(bool is_disp, bool is_force = false)
    {
        this.mTgtAlp = !is_disp ? ((float) 0) : ((float) 1);
        if (is_force)
        {
            this.mWidget.alpha = this.mTgtAlp;
        }
    }

    protected void SyncAnimation()
    {
        ScrollArrowComponent component = this.FetchActiveArrow();
        if (component != null)
        {
            base.gameObject.GetComponent<TweenPosition>().tweenFactor = component.GetComponent<TweenPosition>().tweenFactor;
        }
    }

    private void Update()
    {
        if (this.mWidget != null)
        {
            float alpha = this.mWidget.alpha;
            alpha += (this.mTgtAlp - alpha) * TGT_ALP_SPD_RATE;
            this.mWidget.alpha = alpha;
        }
    }

    public enum DirectionType
    {
        Left,
        Right
    }
}

