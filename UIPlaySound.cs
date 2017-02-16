using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Play Sound")]
public class UIPlaySound : MonoBehaviour
{
    public AudioClip audioClip;
    private bool mIsOver;
    [Range(0f, 2f)]
    public float pitch = 1f;
    public Trigger trigger;
    [Range(0f, 1f)]
    public float volume = 1f;

    private void OnClick()
    {
        if (this.canPlay && (this.trigger == Trigger.OnClick))
        {
            NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
        }
    }

    private void OnDisable()
    {
        if (this.trigger == Trigger.OnDisable)
        {
            NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
        }
    }

    private void OnEnable()
    {
        if (this.trigger == Trigger.OnEnable)
        {
            NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
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
        if (this.canPlay && ((isOver && (this.trigger == Trigger.OnMouseOver)) || (!isOver && (this.trigger == Trigger.OnMouseOut))))
        {
            NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
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
        if (this.canPlay && ((isPressed && (this.trigger == Trigger.OnPress)) || (!isPressed && (this.trigger == Trigger.OnRelease))))
        {
            NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
        }
    }

    private void OnSelect(bool isSelected)
    {
        if (this.canPlay && (!isSelected || (UICamera.currentScheme == UICamera.ControlScheme.Controller)))
        {
            this.OnHover(isSelected);
        }
    }

    public void Play()
    {
        NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
    }

    private bool canPlay
    {
        get
        {
            if (!base.enabled)
            {
                return false;
            }
            UIButton component = base.GetComponent<UIButton>();
            return ((component == null) || component.isEnabled);
        }
    }

    public enum Trigger
    {
        OnClick,
        OnMouseOver,
        OnMouseOut,
        OnPress,
        OnRelease,
        Custom,
        OnEnable,
        OnDisable
    }
}

