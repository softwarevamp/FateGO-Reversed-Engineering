using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/UI Touch Press")]
public class UITouchPress : MonoBehaviour
{
    public float duration = 0.5f;
    protected bool isCancel;
    protected bool isLongPress;
    protected bool isLongPressCheck;
    protected bool isPress;
    public List<EventDelegate> onClick = new List<EventDelegate>();
    public List<EventDelegate> onLongPress = new List<EventDelegate>();
    public List<EventDelegate> onPress = new List<EventDelegate>();
    public float releaseRange = 10f;
    protected Vector2 startPosition;

    protected void Init()
    {
    }

    protected void OnCheckLongPress()
    {
        this.isLongPress = true;
        this.isLongPressCheck = false;
        EventDelegate.Execute(this.onLongPress);
    }

    public void OnClick()
    {
        if (this.IsEnabled && !this.isCancel)
        {
            EventDelegate.Execute(this.onClick);
        }
    }

    protected void OnDrag(Vector2 v)
    {
        if (this.isLongPressCheck)
        {
            float releaseRange = this.releaseRange;
            if (releaseRange >= 0f)
            {
                Vector2 vector = UICamera.lastTouchPosition - this.startPosition;
                if (((vector.x < -releaseRange) || (vector.x > releaseRange)) || ((vector.y < -releaseRange) || (vector.y > releaseRange)))
                {
                    this.isLongPressCheck = false;
                    base.CancelInvoke("OnCheckLongPress");
                }
            }
        }
    }

    protected void OnEnable()
    {
        if (this.IsEnabled)
        {
        }
    }

    public void OnHover(bool isSelect)
    {
        if (this.isPress && !isSelect)
        {
            if (this.isLongPressCheck)
            {
                this.isLongPressCheck = false;
            }
            base.CancelInvoke("OnCheckLongPress");
            this.isPress = false;
            this.isLongPress = false;
        }
    }

    public void OnPress(bool isPressed)
    {
        if (this.IsEnabled)
        {
            if (isPressed)
            {
                this.isCancel = false;
                this.isPress = true;
                this.isLongPress = false;
                this.startPosition = UICamera.lastTouchPosition;
                if (this.IsEnabled)
                {
                    EventDelegate.Execute(this.onPress);
                }
                this.isLongPressCheck = true;
                base.Invoke("OnCheckLongPress", 1f);
            }
            else
            {
                this.isLongPressCheck = false;
                base.CancelInvoke("OnCheckLongPress");
                this.isPress = false;
                this.isLongPress = false;
            }
        }
    }

    protected void OnPressCancel()
    {
        this.isCancel = true;
    }

    public void PressReset()
    {
        this.isCancel = true;
    }

    public bool IsEnabled
    {
        get => 
            base.enabled;
        set
        {
            if (value != base.enabled)
            {
                base.CancelInvoke("OnCheckLongPress");
                this.isCancel = false;
                this.isPress = false;
                this.isLongPress = false;
                this.isLongPressCheck = false;
                base.enabled = value;
            }
        }
    }

    public bool IsLongPress
    {
        get
        {
            if (this.isCancel)
            {
                return false;
            }
            return this.isLongPress;
        }
    }

    public bool IsPress
    {
        get
        {
            if (this.isCancel)
            {
                return false;
            }
            return this.isPress;
        }
    }
}

