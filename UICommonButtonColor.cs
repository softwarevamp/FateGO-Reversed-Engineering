using System;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("FGO/Common/UICommonButtonColor")]
public class UICommonButtonColor : UIWidgetContainer
{
    public static readonly Color disabledColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    public static readonly float duration = 0.2f;
    public static readonly Color hover = new Color(0.78f, 0.78f, 0.78f, 1f);
    [NonSerialized]
    protected bool mInitDone;
    [NonSerialized]
    protected State mState;
    public static readonly Color normal = new Color(1f, 1f, 1f, 1f);
    public static readonly Color pressed = new Color(0.64f, 0.64f, 0.64f, 1f);
    public GameObject[] tweenTargets = new GameObject[1];

    protected virtual void OnDisable()
    {
        if (this.mInitDone && (this.tweenTargets != null))
        {
            this.SetState(State.Normal, true);
            for (int i = 0; i < this.tweenTargets.Length; i++)
            {
                if (this.tweenTargets[i] != null)
                {
                    TweenColor component = this.tweenTargets[i].GetComponent<TweenColor>();
                    if (component != null)
                    {
                        component.value = normal;
                        component.enabled = false;
                    }
                }
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
            if (this.tweenTargets != null)
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
            if (this.tweenTargets != null)
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
            if (this.tweenTargets != null)
            {
                this.SetState(!isOver ? State.Normal : State.Hover, false);
            }
        }
    }

    protected virtual void OnInit()
    {
        this.mInitDone = true;
        if (this.tweenTargets == null)
        {
            this.tweenTargets = new GameObject[] { base.gameObject };
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
            if (this.tweenTargets != null)
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
        if (this.isEnabled && (this.tweenTargets != null))
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

    public virtual void SetState(State state, bool instant)
    {
        if (!this.mInitDone)
        {
            this.mInitDone = true;
            this.OnInit();
        }
        if (instant || (this.mState != state))
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
        if (this.tweenTargets != null)
        {
            Color hover;
            switch (this.mState)
            {
                case State.Hover:
                    hover = UICommonButtonColor.hover;
                    break;

                case State.Pressed:
                    hover = pressed;
                    break;

                case State.Disabled:
                    hover = disabledColor;
                    break;

                default:
                    hover = normal;
                    break;
            }
            if (instant)
            {
                for (int i = 0; i < this.tweenTargets.Length; i++)
                {
                    TweenColor color2 = TweenColor.Begin(this.tweenTargets[i], duration, hover);
                    if (color2 != null)
                    {
                        color2.value = color2.to;
                        color2.enabled = false;
                    }
                }
            }
            else
            {
                for (int j = 0; j < this.tweenTargets.Length; j++)
                {
                    TweenColor.Begin(this.tweenTargets[j], duration, hover);
                }
            }
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

