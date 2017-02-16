using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Keys (Legacy)"), ExecuteInEditMode]
public class UIButtonKeys : UIKeyNavigation
{
    public UIButtonKeys selectOnClick;
    public UIButtonKeys selectOnDown;
    public UIButtonKeys selectOnLeft;
    public UIButtonKeys selectOnRight;
    public UIButtonKeys selectOnUp;

    protected override void OnEnable()
    {
        this.Upgrade();
        base.OnEnable();
    }

    public void Upgrade()
    {
        if ((base.onClick == null) && (this.selectOnClick != null))
        {
            base.onClick = this.selectOnClick.gameObject;
            this.selectOnClick = null;
            NGUITools.SetDirty(this);
        }
        if ((base.onLeft == null) && (this.selectOnLeft != null))
        {
            base.onLeft = this.selectOnLeft.gameObject;
            this.selectOnLeft = null;
            NGUITools.SetDirty(this);
        }
        if ((base.onRight == null) && (this.selectOnRight != null))
        {
            base.onRight = this.selectOnRight.gameObject;
            this.selectOnRight = null;
            NGUITools.SetDirty(this);
        }
        if ((base.onUp == null) && (this.selectOnUp != null))
        {
            base.onUp = this.selectOnUp.gameObject;
            this.selectOnUp = null;
            NGUITools.SetDirty(this);
        }
        if ((base.onDown == null) && (this.selectOnDown != null))
        {
            base.onDown = this.selectOnDown.gameObject;
            this.selectOnDown = null;
            NGUITools.SetDirty(this);
        }
    }
}

