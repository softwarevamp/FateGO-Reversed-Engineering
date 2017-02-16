using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button")]
public class UIButton : UIButtonColor
{
    public static UIButton current;
    public string disabledSprite;
    public UnityEngine.Sprite disabledSprite2D;
    public bool dragHighlight;
    public string hoverSprite;
    public UnityEngine.Sprite hoverSprite2D;
    [NonSerialized]
    private string mNormalSprite;
    [NonSerialized]
    private UnityEngine.Sprite mNormalSprite2D;
    [NonSerialized]
    private UISprite mSprite;
    [NonSerialized]
    private UI2DSprite mSprite2D;
    public List<EventDelegate> onClick = new List<EventDelegate>();
    public bool pixelSnap;
    public string pressedSprite;
    public UnityEngine.Sprite pressedSprite2D;

    protected virtual void OnClick()
    {
        if ((current == null) && this.isEnabled)
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
                    this.SetState(UIButtonColor.State.Normal, false);
                }
            }
        }
        else
        {
            this.SetState(UIButtonColor.State.Disabled, true);
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.mSprite = base.mWidget as UISprite;
        this.mSprite2D = base.mWidget as UI2DSprite;
        if (this.mSprite != null)
        {
            this.mNormalSprite = this.mSprite.spriteName;
        }
        if (this.mSprite2D != null)
        {
            this.mNormalSprite2D = this.mSprite2D.sprite2D;
        }
    }

    protected void SetSprite(string sp)
    {
        if (((this.mSprite != null) && !string.IsNullOrEmpty(sp)) && (this.mSprite.spriteName != sp))
        {
            this.mSprite.spriteName = sp;
            if (this.pixelSnap)
            {
                this.mSprite.MakePixelPerfect();
            }
        }
    }

    protected void SetSprite(UnityEngine.Sprite sp)
    {
        if (((sp != null) && (this.mSprite2D != null)) && (this.mSprite2D.sprite2D != sp))
        {
            this.mSprite2D.sprite2D = sp;
            if (this.pixelSnap)
            {
                this.mSprite2D.MakePixelPerfect();
            }
        }
    }

    public override void SetState(UIButtonColor.State state, bool immediate)
    {
        base.SetState(state, immediate);
        if (this.mSprite != null)
        {
            switch (state)
            {
                case UIButtonColor.State.Normal:
                    this.SetSprite(this.mNormalSprite);
                    break;

                case UIButtonColor.State.Hover:
                    this.SetSprite(!string.IsNullOrEmpty(this.hoverSprite) ? this.hoverSprite : this.mNormalSprite);
                    break;

                case UIButtonColor.State.Pressed:
                    this.SetSprite(this.pressedSprite);
                    break;

                case UIButtonColor.State.Disabled:
                    this.SetSprite(this.disabledSprite);
                    break;
            }
        }
        else if (this.mSprite2D != null)
        {
            switch (state)
            {
                case UIButtonColor.State.Normal:
                    this.SetSprite(this.mNormalSprite2D);
                    break;

                case UIButtonColor.State.Hover:
                    this.SetSprite((this.hoverSprite2D != null) ? this.hoverSprite2D : this.mNormalSprite2D);
                    break;

                case UIButtonColor.State.Pressed:
                    this.SetSprite(this.pressedSprite2D);
                    break;

                case UIButtonColor.State.Disabled:
                    this.SetSprite(this.disabledSprite2D);
                    break;
            }
        }
    }

    public override bool isEnabled
    {
        get
        {
            if (!base.enabled)
            {
                return false;
            }
            Collider component = base.gameObject.GetComponent<Collider>();
            if ((component != null) && component.enabled)
            {
                return true;
            }
            Collider2D colliderd = base.GetComponent<Collider2D>();
            return ((colliderd != null) && colliderd.enabled);
        }
        set
        {
            if (this.isEnabled != value)
            {
                Collider component = base.gameObject.GetComponent<Collider>();
                if (component != null)
                {
                    component.enabled = value;
                    this.SetState(!value ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, false);
                }
                else
                {
                    Collider2D colliderd = base.GetComponent<Collider2D>();
                    if (colliderd != null)
                    {
                        colliderd.enabled = value;
                        this.SetState(!value ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, false);
                    }
                    else
                    {
                        base.enabled = value;
                    }
                }
            }
        }
    }

    public string normalSprite
    {
        get
        {
            if (!base.mInitDone)
            {
                this.OnInit();
            }
            return this.mNormalSprite;
        }
        set
        {
            if (!base.mInitDone)
            {
                this.OnInit();
            }
            if (((this.mSprite != null) && !string.IsNullOrEmpty(this.mNormalSprite)) && (this.mNormalSprite == this.mSprite.spriteName))
            {
                this.mNormalSprite = value;
                this.SetSprite(value);
                NGUITools.SetDirty(this.mSprite);
            }
            else
            {
                this.mNormalSprite = value;
                if (base.mState == UIButtonColor.State.Normal)
                {
                    this.SetSprite(value);
                }
            }
        }
    }

    public UnityEngine.Sprite normalSprite2D
    {
        get
        {
            if (!base.mInitDone)
            {
                this.OnInit();
            }
            return this.mNormalSprite2D;
        }
        set
        {
            if (!base.mInitDone)
            {
                this.OnInit();
            }
            if ((this.mSprite2D != null) && (this.mNormalSprite2D == this.mSprite2D.sprite2D))
            {
                this.mNormalSprite2D = value;
                this.SetSprite(value);
                NGUITools.SetDirty(this.mSprite);
            }
            else
            {
                this.mNormalSprite2D = value;
                if (base.mState == UIButtonColor.State.Normal)
                {
                    this.SetSprite(value);
                }
            }
        }
    }
}

