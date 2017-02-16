using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Play System Se")]
public class UIPlaySystemSe : MonoBehaviour
{
    public SeManager.SystemSeKind kind;
    private bool mIsOver;
    public Trigger trigger;

    private void OnClick()
    {
        if (base.enabled && (this.trigger == Trigger.OnClick))
        {
            SeManager.PlaySystemSe(this.kind);
        }
    }

    private void OnHover(bool isOver)
    {
        if (this.trigger == Trigger.OnMouseOver)
        {
            if (this.mIsOver == isOver)
            {
                return;
            }
            this.mIsOver = isOver;
        }
        if (base.enabled && ((isOver && (this.trigger == Trigger.OnMouseOver)) || (!isOver && (this.trigger == Trigger.OnMouseOut))))
        {
            SeManager.PlaySystemSe(this.kind);
        }
    }

    private void OnPress(bool isPressed)
    {
        if (this.trigger == Trigger.OnPress)
        {
            if (this.mIsOver == isPressed)
            {
                return;
            }
            this.mIsOver = isPressed;
        }
        if (base.enabled && ((isPressed && (this.trigger == Trigger.OnPress)) || (!isPressed && (this.trigger == Trigger.OnRelease))))
        {
            SeManager.PlaySystemSe(this.kind);
        }
    }

    private void OnSelect(bool isSelected)
    {
        if (base.enabled && (!isSelected || (UICamera.currentScheme == UICamera.ControlScheme.Controller)))
        {
            this.OnHover(isSelected);
        }
    }

    public void Play()
    {
        SeManager.PlaySystemSe(this.kind);
    }

    public enum Trigger
    {
        OnClick,
        OnMouseOver,
        OnMouseOut,
        OnPress,
        OnRelease,
        Custom
    }
}

