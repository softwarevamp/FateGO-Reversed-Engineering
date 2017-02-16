using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/MessageButton")]
public class UIMessageButton : UIMessageButtonColor
{
    public static UIMessageButton current;
    public Color disabledColor = Color.grey;
    public bool dragHighlight;
    public List<EventDelegate> onClick = new List<EventDelegate>();

    public void Fadeout(float d)
    {
        if (base.tweenTarget != null)
        {
            if (!base.mStarted)
            {
                base.mStarted = true;
                base.Init();
            }
            Transform transform = base.tweenTarget.transform;
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                TweenAlpha.Begin(transform.GetChild(i).gameObject, d, 0f);
            }
        }
    }

    protected virtual void OnClick()
    {
        if (this.isEnabled)
        {
            current = this;
            EventDelegate.Execute(this.onClick);
            current = null;
        }
    }

    protected override void OnDragOut()
    {
        if (this.isEnabled && (this.dragHighlight || (UICamera.currentTouch.pressed == base.gameObject)))
        {
            base.OnDragOut();
        }
    }

    protected override void OnDragOver()
    {
        if (this.isEnabled && (this.dragHighlight || (UICamera.currentTouch.pressed == base.gameObject)))
        {
            base.OnDragOver();
        }
    }

    protected override void OnEnable()
    {
        if (this.isEnabled)
        {
            if (base.mStarted)
            {
                base.mColor = base.normalColor;
                if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
                {
                    this.OnHover(UICamera.selectedObject == base.gameObject);
                }
                else if (UICamera.currentScheme == UICamera.ControlScheme.Mouse)
                {
                    this.OnHover(UICamera.hoveredObject == base.gameObject);
                }
                else
                {
                    this.UpdateColor(true, false);
                }
            }
        }
        else
        {
            this.UpdateColor(false, true);
        }
    }

    protected override void OnHover(bool isOver)
    {
        if (this.isEnabled)
        {
            base.OnHover(isOver);
        }
    }

    protected override void OnPress(bool isPressed)
    {
        if (this.isEnabled)
        {
            base.OnPress(isPressed);
        }
    }

    protected override void OnSelect(bool isSelected)
    {
        if (this.isEnabled)
        {
            base.OnSelect(isSelected);
        }
    }

    public void UpdateColor(bool shouldBeEnabled, bool immediate)
    {
        if (base.tweenTarget != null)
        {
            if (!base.mStarted)
            {
                base.mStarted = true;
                base.Init();
            }
            Color color = !shouldBeEnabled ? this.disabledColor : base.defaultColor;
            Transform transform = base.tweenTarget.transform;
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                TweenColor color2 = TweenColor.Begin(transform.GetChild(i).gameObject, base.duration, color);
                if ((color2 != null) && immediate)
                {
                    color2.value = color;
                    color2.enabled = false;
                }
            }
        }
    }

    public virtual bool isEnabled
    {
        get
        {
            if (!base.enabled)
            {
                return false;
            }
            Collider component = base.GetComponent<Collider>();
            return ((component != null) && component.enabled);
        }
        set
        {
            Collider component = base.GetComponent<Collider>();
            if (component != null)
            {
                component.enabled = value;
            }
            else
            {
                base.enabled = value;
            }
            this.UpdateColor(value, false);
        }
    }
}

