using System;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/Interaction/NGUI Scroll Bar")]
public class UIScrollBar : UISlider
{
    [HideInInspector, SerializeField]
    private Direction mDir = Direction.Upgraded;
    [HideInInspector, SerializeField]
    private float mScroll;
    [SerializeField, HideInInspector]
    protected float mSize = 1f;

    public override void ForceUpdate()
    {
        if (base.mFG != null)
        {
            base.mIsDirty = false;
            float a = Mathf.Clamp01(this.mSize) * 0.5f;
            float num2 = Mathf.Lerp(a, 1f - a, base.value);
            float x = num2 - a;
            float z = num2 + a;
            if (base.isHorizontal)
            {
                base.mFG.drawRegion = !base.isInverted ? new Vector4(x, 0f, z, 1f) : new Vector4(1f - z, 0f, 1f - x, 1f);
            }
            else
            {
                base.mFG.drawRegion = !base.isInverted ? new Vector4(0f, x, 1f, z) : new Vector4(0f, 1f - z, 1f, 1f - x);
            }
            if (base.thumb != null)
            {
                Vector4 drawingDimensions = base.mFG.drawingDimensions;
                float introduced6 = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, 0.5f);
                Vector3 position = new Vector3(introduced6, Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, 0.5f));
                base.SetThumbPosition(base.mFG.cachedTransform.TransformPoint(position));
            }
        }
        else
        {
            base.ForceUpdate();
        }
    }

    protected override float LocalToValue(Vector2 localPos)
    {
        if (base.mFG == null)
        {
            return base.LocalToValue(localPos);
        }
        float num = Mathf.Clamp01(this.mSize) * 0.5f;
        float t = num;
        float num3 = 1f - num;
        Vector3[] localCorners = base.mFG.localCorners;
        if (base.isHorizontal)
        {
            t = Mathf.Lerp(localCorners[0].x, localCorners[2].x, t);
            num3 = Mathf.Lerp(localCorners[0].x, localCorners[2].x, num3);
            float num4 = num3 - t;
            if (num4 == 0f)
            {
                return base.value;
            }
            return (!base.isInverted ? ((localPos.x - t) / num4) : ((num3 - localPos.x) / num4));
        }
        t = Mathf.Lerp(localCorners[0].y, localCorners[1].y, t);
        num3 = Mathf.Lerp(localCorners[3].y, localCorners[2].y, num3);
        float num5 = num3 - t;
        if (num5 == 0f)
        {
            return base.value;
        }
        return (!base.isInverted ? ((localPos.y - t) / num5) : ((num3 - localPos.y) / num5));
    }

    protected override void OnStart()
    {
        base.OnStart();
        if (((base.mFG != null) && (base.mFG.gameObject != base.gameObject)) && ((base.mFG.GetComponent<Collider>() != null) || (base.mFG.GetComponent<Collider2D>() != null)))
        {
            UIEventListener listener = UIEventListener.Get(base.mFG.gameObject);
            listener.onPress = (UIEventListener.BoolDelegate) Delegate.Combine(listener.onPress, new UIEventListener.BoolDelegate(this.OnPressForeground));
            listener.onDrag = (UIEventListener.VectorDelegate) Delegate.Combine(listener.onDrag, new UIEventListener.VectorDelegate(this.OnDragForeground));
            base.mFG.autoResizeBoxCollider = true;
        }
    }

    protected override void Upgrade()
    {
        if (this.mDir != Direction.Upgraded)
        {
            base.mValue = this.mScroll;
            if (this.mDir == Direction.Horizontal)
            {
                base.mFill = !base.mInverted ? UIProgressBar.FillDirection.LeftToRight : UIProgressBar.FillDirection.RightToLeft;
            }
            else
            {
                base.mFill = !base.mInverted ? UIProgressBar.FillDirection.TopToBottom : UIProgressBar.FillDirection.BottomToTop;
            }
            this.mDir = Direction.Upgraded;
        }
    }

    public float barSize
    {
        get => 
            this.mSize;
        set
        {
            float num = Mathf.Clamp01(value);
            if (this.mSize != num)
            {
                this.mSize = num;
                base.mIsDirty = true;
                if (NGUITools.GetActive(this))
                {
                    if ((UIProgressBar.current == null) && (base.onChange != null))
                    {
                        UIProgressBar.current = this;
                        EventDelegate.Execute(base.onChange);
                        UIProgressBar.current = null;
                    }
                    this.ForceUpdate();
                }
            }
        }
    }

    [Obsolete("Use 'value' instead")]
    public float scrollValue
    {
        get => 
            base.value;
        set
        {
            base.value = value;
        }
    }

    private enum Direction
    {
        Horizontal,
        Vertical,
        Upgraded
    }
}

