using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[AddComponentMenu("FGO/Common/UICommonButton")]
public class UICommonButton : UICommonButtonColor
{
    public static UICommonButton current;
    public static readonly bool dragHighlight;
    public List<EventDelegate> onClick = new List<EventDelegate>();

    protected virtual void OnClick()
    {
        if (current == null)
        {
            if (this.isEnabled)
            {
                current = this;
                EventDelegate.Execute(this.onClick);
                current = null;
            }
            else
            {
                Debug.Log(string.Concat(new object[] { "UICommonButton OnClick disnable [", base.gameObject.name, "]", base.gameObject.activeInHierarchy, " ", base.gameObject.activeSelf }), base.gameObject);
                if (base.gameObject.activeInHierarchy)
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                }
            }
        }
    }

    protected override void OnDragOut()
    {
        if (this.isEnabled && (dragHighlight || (UICamera.currentTouch.pressed == base.gameObject)))
        {
            base.OnDragOut();
        }
    }

    protected override void OnDragOver()
    {
        if (this.isEnabled && (dragHighlight || (UICamera.currentTouch.pressed == base.gameObject)))
        {
            base.OnDragOver();
        }
    }

    protected override void OnEnable()
    {
        if (this.isEnabled)
        {
            if (base.mInitDone)
            {
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
                    this.SetState(UICommonButtonColor.State.Normal, false);
                }
            }
        }
        else
        {
            this.SetState(UICommonButtonColor.State.Disabled, true);
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
    }

    public void SetButtonEnable(bool isEnable, bool immediate = true)
    {
        Collider component = base.GetComponent<Collider>();
        if (component != null)
        {
            component.enabled = true;
        }
        base.enabled = true;
        this.SetState(!isEnable ? UICommonButtonColor.State.Disabled : UICommonButtonColor.State.Normal, immediate);
        UITouchPress press = base.GetComponent<UITouchPress>();
        if (press != null)
        {
            press.IsEnabled = isEnable;
        }
    }

    public void SetColliderEnable(bool isEnable, bool immediate = true)
    {
        this.SetState(UICommonButtonColor.State.Normal, immediate);
        base.enabled = isEnable;
        UITouchPress component = base.GetComponent<UITouchPress>();
        if (component != null)
        {
            component.IsEnabled = isEnable;
        }
        Collider collider = base.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = isEnable;
        }
    }

    public void SetEnable(bool value)
    {
        this.SetState(!value ? UICommonButtonColor.State.Disabled : UICommonButtonColor.State.Normal, true);
    }

    public override void SetState(UICommonButtonColor.State state, bool immediate)
    {
        base.SetState(state, immediate);
    }

    public override bool isEnabled
    {
        get
        {
            if (!base.enabled)
            {
                return false;
            }
            return (base.mState != UICommonButtonColor.State.Disabled);
        }
        set
        {
            if (this.isEnabled != value)
            {
                this.SetState(!value ? UICommonButtonColor.State.Disabled : UICommonButtonColor.State.Normal, false);
            }
        }
    }
}

