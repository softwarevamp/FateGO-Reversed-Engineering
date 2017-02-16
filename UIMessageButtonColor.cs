using System;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/Interaction/Message Button Color")]
public class UIMessageButtonColor : UIWidgetContainer
{
    public float duration = 0.2f;
    public Color hover = new Color(0.8823529f, 0.7843137f, 0.5882353f, 1f);
    protected Color mColor;
    protected bool mStarted;
    protected UIWidget mWidget;
    public Color normalColor = Color.white;
    public Color pressed = new Color(0.7176471f, 0.6392157f, 0.4823529f, 1f);
    public GameObject tweenTarget;

    protected void Awake()
    {
        if (!this.mStarted)
        {
            this.mStarted = true;
            this.Init();
        }
    }

    protected void ChangeTweenColor(float duration, Color c)
    {
        if (this.tweenTarget != null)
        {
            Transform transform = this.tweenTarget.transform;
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                TweenColor.Begin(transform.GetChild(i).gameObject, duration, c);
            }
        }
    }

    protected void Init()
    {
        if (this.tweenTarget == null)
        {
            this.tweenTarget = base.gameObject;
        }
        this.mColor = this.normalColor;
    }

    protected virtual void OnDisable()
    {
        if (this.mStarted && (this.tweenTarget != null))
        {
            Transform transform = this.tweenTarget.transform;
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                TweenColor component = transform.GetChild(i).gameObject.GetComponent<TweenColor>();
                if (component != null)
                {
                    component.value = this.mColor;
                    component.enabled = false;
                }
            }
        }
    }

    protected virtual void OnDragOut()
    {
        if (base.enabled)
        {
            if (!this.mStarted)
            {
                this.Awake();
            }
            this.ChangeTweenColor(this.duration, this.mColor);
        }
    }

    protected virtual void OnDragOver()
    {
        if (base.enabled)
        {
            if (!this.mStarted)
            {
                this.Awake();
            }
            this.ChangeTweenColor(this.duration, this.pressed);
        }
    }

    protected virtual void OnEnable()
    {
        if (this.mStarted)
        {
            this.OnHover(UICamera.IsHighlighted(base.gameObject));
        }
        if (UICamera.currentTouch != null)
        {
            if (UICamera.currentTouch.pressed == base.gameObject)
            {
                this.OnPress(true);
            }
            else if (UICamera.currentTouch.current == base.gameObject)
            {
                this.OnHover(true);
            }
        }
    }

    protected virtual void OnHover(bool isOver)
    {
        if (base.enabled)
        {
            if (!this.mStarted)
            {
                this.Awake();
            }
            this.ChangeTweenColor(this.duration, !isOver ? this.mColor : this.hover);
        }
    }

    protected virtual void OnPress(bool isPressed)
    {
        if (base.enabled && (UICamera.currentTouch != null))
        {
            if (!this.mStarted)
            {
                this.Awake();
            }
            if (this.tweenTarget != null)
            {
                if (isPressed)
                {
                    this.ChangeTweenColor(this.duration, this.pressed);
                }
                else if ((UICamera.currentTouch.current == base.gameObject) && (UICamera.currentScheme == UICamera.ControlScheme.Controller))
                {
                    this.ChangeTweenColor(this.duration, this.hover);
                }
                else
                {
                    this.ChangeTweenColor(this.duration, this.mColor);
                }
            }
        }
    }

    protected virtual void OnSelect(bool isSelected)
    {
        if ((base.enabled && (!isSelected || (UICamera.currentScheme == UICamera.ControlScheme.Controller))) && (this.tweenTarget != null))
        {
            this.OnHover(isSelected);
        }
    }

    public Color defaultColor
    {
        get
        {
            this.Awake();
            return this.mColor;
        }
        set
        {
            this.Awake();
            this.mColor = value;
        }
    }
}

