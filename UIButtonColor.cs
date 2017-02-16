using System;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/Interaction/Button Color")]
public class UIButtonColor : UIWidgetContainer
{
    public Color disabledColor = Color.grey;
    public float duration = 0.2f;
    public Color hover = new Color(0.8823529f, 0.7843137f, 0.5882353f, 1f);
    [NonSerialized]
    protected Color mDefaultColor;
    [NonSerialized]
    protected bool mInitDone;
    [NonSerialized]
    protected Color mStartingColor;
    [NonSerialized]
    protected State mState;
    [NonSerialized]
    protected UIWidget mWidget;
    public Color pressed = new Color(0.7176471f, 0.6392157f, 0.4823529f, 1f);
    public GameObject tweenTarget;

    protected virtual void OnDisable()
    {
        if (this.mInitDone && (this.tweenTarget != null))
        {
            this.SetState(State.Normal, true);
            TweenColor component = this.tweenTarget.GetComponent<TweenColor>();
            if (component != null)
            {
                component.value = this.mDefaultColor;
                component.enabled = false;
            }
        }
    }

    protected virtual void OnDragOut()
    {
        if (this.isEnabled)
        {
            if (!this.mInitDone)
            {
                this.OnInit();
            }
            if (this.tweenTarget != null)
            {
                this.SetState(State.Normal, false);
            }
        }
    }

    protected virtual void OnDragOver()
    {
        if (this.isEnabled)
        {
            if (!this.mInitDone)
            {
                this.OnInit();
            }
            if (this.tweenTarget != null)
            {
                this.SetState(State.Pressed, false);
            }
        }
    }

    protected virtual void OnEnable()
    {
        if (this.mInitDone)
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
        if (this.isEnabled)
        {
            if (!this.mInitDone)
            {
                this.OnInit();
            }
            if (this.tweenTarget != null)
            {
                this.SetState(!isOver ? State.Normal : State.Hover, false);
            }
        }
    }

    protected virtual void OnInit()
    {
        this.mInitDone = true;
        if (this.tweenTarget == null)
        {
            this.tweenTarget = base.gameObject;
        }
        if (this.tweenTarget != null)
        {
            this.mWidget = this.tweenTarget.GetComponent<UIWidget>();
        }
        if (this.mWidget != null)
        {
            this.mDefaultColor = this.mWidget.color;
            this.mStartingColor = this.mDefaultColor;
        }
        else if (this.tweenTarget != null)
        {
            Renderer component = this.tweenTarget.GetComponent<Renderer>();
            if (component != null)
            {
                this.mDefaultColor = !Application.isPlaying ? component.sharedMaterial.color : component.material.color;
                this.mStartingColor = this.mDefaultColor;
            }
            else
            {
                Light light = this.tweenTarget.GetComponent<Light>();
                if (light != null)
                {
                    this.mDefaultColor = light.color;
                    this.mStartingColor = this.mDefaultColor;
                }
                else
                {
                    this.tweenTarget = null;
                    this.mInitDone = false;
                }
            }
        }
    }

    protected virtual void OnPress(bool isPressed)
    {
        if (this.isEnabled && (UICamera.currentTouch != null))
        {
            if (!this.mInitDone)
            {
                this.OnInit();
            }
            if (this.tweenTarget != null)
            {
                if (isPressed)
                {
                    this.SetState(State.Pressed, false);
                }
                else if (UICamera.currentTouch.current == base.gameObject)
                {
                    if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
                    {
                        this.SetState(State.Hover, false);
                    }
                    else if ((UICamera.currentScheme == UICamera.ControlScheme.Mouse) && (UICamera.hoveredObject == base.gameObject))
                    {
                        this.SetState(State.Hover, false);
                    }
                    else
                    {
                        this.SetState(State.Normal, false);
                    }
                }
                else
                {
                    this.SetState(State.Normal, false);
                }
            }
        }
    }

    protected virtual void OnSelect(bool isSelected)
    {
        if (this.isEnabled && (this.tweenTarget != null))
        {
            if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
            {
                this.OnHover(isSelected);
            }
            else if (!isSelected && (UICamera.touchCount < 2))
            {
                this.OnHover(isSelected);
            }
        }
    }

    public void ResetDefaultColor()
    {
        this.defaultColor = this.mStartingColor;
    }

    public virtual void SetState(State state, bool instant)
    {
        if (!this.mInitDone)
        {
            this.mInitDone = true;
            this.OnInit();
        }
        if (this.mState != state)
        {
            this.mState = state;
            this.UpdateColor(instant);
        }
    }

    private void Start()
    {
        if (!this.mInitDone)
        {
            this.OnInit();
        }
        if (!this.isEnabled)
        {
            this.SetState(State.Disabled, true);
        }
    }

    public void UpdateColor(bool instant)
    {
        if (this.tweenTarget != null)
        {
            TweenColor color;
            switch (this.mState)
            {
                case State.Hover:
                    color = TweenColor.Begin(this.tweenTarget, this.duration, this.hover);
                    break;

                case State.Pressed:
                    color = TweenColor.Begin(this.tweenTarget, this.duration, this.pressed);
                    break;

                case State.Disabled:
                    color = TweenColor.Begin(this.tweenTarget, this.duration, this.disabledColor);
                    break;

                default:
                    color = TweenColor.Begin(this.tweenTarget, this.duration, this.mDefaultColor);
                    break;
            }
            if (instant && (color != null))
            {
                color.value = color.to;
                color.enabled = false;
            }
        }
    }

    public Color defaultColor
    {
        get
        {
            if (!this.mInitDone)
            {
                this.OnInit();
            }
            return this.mDefaultColor;
        }
        set
        {
            if (!this.mInitDone)
            {
                this.OnInit();
            }
            this.mDefaultColor = value;
            State mState = this.mState;
            this.mState = State.Disabled;
            this.SetState(mState, false);
        }
    }

    public virtual bool isEnabled
    {
        get => 
            base.enabled;
        set
        {
            base.enabled = value;
        }
    }

    public State state
    {
        get => 
            this.mState;
        set
        {
            this.SetState(value, false);
        }
    }

    public enum State
    {
        Normal,
        Hover,
        Pressed,
        Disabled
    }
}

