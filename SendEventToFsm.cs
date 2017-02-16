using System;
using UnityEngine;

public class SendEventToFsm : BaseMonoBehaviour
{
    private bool mStarted;
    [HideInInspector]
    public string sendEvent = "none";
    public PlayMakerFSM targetFSM;
    public Trigger trigger;

    private void OnClick()
    {
        if (base.enabled && (this.trigger == Trigger.OnClick))
        {
            this.Send();
        }
    }

    private void OnDoubleClick()
    {
        if (base.enabled && (this.trigger == Trigger.OnDoubleClick))
        {
            this.Send();
        }
    }

    private void OnEnable()
    {
        if (this.mStarted)
        {
            this.OnHover(UICamera.IsHighlighted(base.gameObject));
        }
    }

    private void OnHover(bool isOver)
    {
        if (base.enabled && ((isOver && (this.trigger == Trigger.OnMouseOver)) || (!isOver && (this.trigger == Trigger.OnMouseOut))))
        {
            this.Send();
        }
    }

    private void OnPress(bool isPressed)
    {
        if (base.enabled && ((isPressed && (this.trigger == Trigger.OnPress)) || (!isPressed && (this.trigger == Trigger.OnRelease))))
        {
            this.Send();
        }
    }

    private void OnSelect(bool isSelected)
    {
        if (base.enabled && (!isSelected || (UICamera.currentScheme == UICamera.ControlScheme.Controller)))
        {
            this.OnHover(isSelected);
        }
    }

    private void Send()
    {
        if (this.targetFSM != null)
        {
            this.targetFSM.SendEvent(this.sendEvent);
        }
    }

    private void Start()
    {
        this.mStarted = true;
    }

    public enum Trigger
    {
        OnClick,
        OnMouseOver,
        OnMouseOut,
        OnPress,
        OnRelease,
        OnDoubleClick
    }
}

