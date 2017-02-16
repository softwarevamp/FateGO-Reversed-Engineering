using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Key Binding")]
public class UIKeyBinding : MonoBehaviour
{
    public Action action;
    public KeyCode keyCode;
    private bool mIgnoreUp;
    private bool mIsInput;
    public Modifier modifier;
    private bool mPress;

    protected virtual bool IsModifierActive()
    {
        if (this.modifier == Modifier.None)
        {
            return true;
        }
        if (this.modifier == Modifier.Alt)
        {
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            {
                return true;
            }
        }
        else if (this.modifier == Modifier.Control)
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                return true;
            }
        }
        else if ((this.modifier == Modifier.Shift) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            return true;
        }
        return false;
    }

    protected virtual void OnBindingClick()
    {
        UICamera.Notify(base.gameObject, "OnClick", null);
    }

    protected virtual void OnBindingPress(bool pressed)
    {
        UICamera.Notify(base.gameObject, "OnPress", pressed);
    }

    protected virtual void OnSubmit()
    {
        if ((UICamera.currentKey == this.keyCode) && this.IsModifierActive())
        {
            this.mIgnoreUp = true;
        }
    }

    protected virtual void Start()
    {
        UIInput component = base.GetComponent<UIInput>();
        this.mIsInput = component != null;
        if (component != null)
        {
            EventDelegate.Add(component.onSubmit, new EventDelegate.Callback(this.OnSubmit));
        }
    }

    protected virtual void Update()
    {
        if (!UICamera.inputHasFocus && ((this.keyCode != KeyCode.None) && this.IsModifierActive()))
        {
            bool keyDown = Input.GetKeyDown(this.keyCode);
            bool keyUp = Input.GetKeyUp(this.keyCode);
            if (keyDown)
            {
                this.mPress = true;
            }
            if ((this.action == Action.PressAndClick) || (this.action == Action.All))
            {
                UICamera.currentTouch = UICamera.controller;
                UICamera.currentScheme = UICamera.ControlScheme.Mouse;
                UICamera.currentTouch.current = base.gameObject;
                if (keyDown)
                {
                    this.OnBindingPress(true);
                }
                if (this.mPress && keyUp)
                {
                    this.OnBindingPress(false);
                    this.OnBindingClick();
                }
                UICamera.currentTouch.current = null;
            }
            if (((this.action == Action.Select) || (this.action == Action.All)) && keyUp)
            {
                if (this.mIsInput)
                {
                    if ((!this.mIgnoreUp && !UICamera.inputHasFocus) && this.mPress)
                    {
                        UICamera.selectedObject = base.gameObject;
                    }
                    this.mIgnoreUp = false;
                }
                else if (this.mPress)
                {
                    UICamera.selectedObject = base.gameObject;
                }
            }
            if (keyUp)
            {
                this.mPress = false;
            }
        }
    }

    public enum Action
    {
        PressAndClick,
        Select,
        All
    }

    public enum Modifier
    {
        None,
        Shift,
        Control,
        Alt
    }
}

