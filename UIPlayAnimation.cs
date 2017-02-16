using AnimationOrTween;
using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/Interaction/Play Animation")]
public class UIPlayAnimation : MonoBehaviour
{
    public Animator animator;
    [HideInInspector, SerializeField]
    private string callWhenFinished;
    public bool clearSelection;
    public string clipName;
    public static UIPlayAnimation current;
    public DisableCondition disableWhenFinished;
    private bool dragHighlight;
    [HideInInspector, SerializeField]
    private GameObject eventReceiver;
    public EnableCondition ifDisabledOnPlay;
    private bool mActivated;
    private bool mStarted;
    public List<EventDelegate> onFinished = new List<EventDelegate>();
    public AnimationOrTween.Direction playDirection = AnimationOrTween.Direction.Forward;
    public bool resetOnPlay;
    public Animation target;
    public AnimationOrTween.Trigger trigger;

    private void Awake()
    {
        UIButton component = base.GetComponent<UIButton>();
        if (component != null)
        {
            this.dragHighlight = component.dragHighlight;
        }
        if ((this.eventReceiver != null) && EventDelegate.IsValid(this.onFinished))
        {
            this.eventReceiver = null;
            this.callWhenFinished = null;
        }
    }

    private void OnClick()
    {
        if ((UICamera.currentTouchID >= -1) && (base.enabled && (this.trigger == AnimationOrTween.Trigger.OnClick)))
        {
            this.Play(true, false);
        }
    }

    private void OnDisable()
    {
        UIToggle component = base.GetComponent<UIToggle>();
        if (component != null)
        {
            EventDelegate.Remove(component.onChange, new EventDelegate.Callback(this.OnToggle));
        }
    }

    private void OnDoubleClick()
    {
        if ((UICamera.currentTouchID >= -1) && (base.enabled && (this.trigger == AnimationOrTween.Trigger.OnDoubleClick)))
        {
            this.Play(true, false);
        }
    }

    private void OnDragOut()
    {
        if ((base.enabled && this.dualState) && (UICamera.hoveredObject != base.gameObject))
        {
            this.Play(false, true);
        }
    }

    private void OnDragOver()
    {
        if (base.enabled && this.dualState)
        {
            if (UICamera.currentTouch.dragged == base.gameObject)
            {
                this.Play(true, true);
            }
            else if (this.dragHighlight && (this.trigger == AnimationOrTween.Trigger.OnPress))
            {
                this.Play(true, true);
            }
        }
    }

    private void OnDrop(GameObject go)
    {
        if ((base.enabled && (this.trigger == AnimationOrTween.Trigger.OnPress)) && (UICamera.currentTouch.dragged != base.gameObject))
        {
            this.Play(false, true);
        }
    }

    private void OnEnable()
    {
        if (this.mStarted)
        {
            this.OnHover(UICamera.IsHighlighted(base.gameObject));
        }
        if (UICamera.currentTouch != null)
        {
            if ((this.trigger == AnimationOrTween.Trigger.OnPress) || (this.trigger == AnimationOrTween.Trigger.OnPressTrue))
            {
                this.mActivated = UICamera.currentTouch.pressed == base.gameObject;
            }
            if ((this.trigger == AnimationOrTween.Trigger.OnHover) || (this.trigger == AnimationOrTween.Trigger.OnHoverTrue))
            {
                this.mActivated = UICamera.currentTouch.current == base.gameObject;
            }
        }
        UIToggle component = base.GetComponent<UIToggle>();
        if (component != null)
        {
            EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnToggle));
        }
    }

    private void OnFinished()
    {
        if (current == null)
        {
            current = this;
            EventDelegate.Execute(this.onFinished);
            if ((this.eventReceiver != null) && !string.IsNullOrEmpty(this.callWhenFinished))
            {
                this.eventReceiver.SendMessage(this.callWhenFinished, SendMessageOptions.DontRequireReceiver);
            }
            this.eventReceiver = null;
            current = null;
        }
    }

    private void OnHover(bool isOver)
    {
        if (base.enabled && (((this.trigger == AnimationOrTween.Trigger.OnHover) || ((this.trigger == AnimationOrTween.Trigger.OnHoverTrue) && isOver)) || ((this.trigger == AnimationOrTween.Trigger.OnHoverFalse) && !isOver)))
        {
            this.Play(isOver, this.dualState);
        }
    }

    private void OnPress(bool isPressed)
    {
        if ((base.enabled && (UICamera.currentTouchID >= -1)) && (((this.trigger == AnimationOrTween.Trigger.OnPress) || ((this.trigger == AnimationOrTween.Trigger.OnPressTrue) && isPressed)) || ((this.trigger == AnimationOrTween.Trigger.OnPressFalse) && !isPressed)))
        {
            this.Play(isPressed, this.dualState);
        }
    }

    private void OnSelect(bool isSelected)
    {
        if (base.enabled && (((this.trigger == AnimationOrTween.Trigger.OnSelect) || ((this.trigger == AnimationOrTween.Trigger.OnSelectTrue) && isSelected)) || ((this.trigger == AnimationOrTween.Trigger.OnSelectFalse) && !isSelected)))
        {
            this.Play(isSelected, this.dualState);
        }
    }

    private void OnToggle()
    {
        if ((base.enabled && (UIToggle.current != null)) && (((this.trigger == AnimationOrTween.Trigger.OnActivate) || ((this.trigger == AnimationOrTween.Trigger.OnActivateTrue) && UIToggle.current.value)) || ((this.trigger == AnimationOrTween.Trigger.OnActivateFalse) && !UIToggle.current.value)))
        {
            this.Play(UIToggle.current.value, this.dualState);
        }
    }

    public void Play(bool forward)
    {
        this.Play(forward, true);
    }

    public void Play(bool forward, bool onlyIfDifferent)
    {
        if ((this.target != null) || (this.animator != null))
        {
            if (onlyIfDifferent)
            {
                if (this.mActivated == forward)
                {
                    return;
                }
                this.mActivated = forward;
            }
            if (this.clearSelection && (UICamera.selectedObject == base.gameObject))
            {
                UICamera.selectedObject = null;
            }
            int num = (int) -this.playDirection;
            AnimationOrTween.Direction playDirection = !forward ? ((AnimationOrTween.Direction) num) : this.playDirection;
            ActiveAnimation animation = (this.target == null) ? ActiveAnimation.Play(this.animator, this.clipName, playDirection, this.ifDisabledOnPlay, this.disableWhenFinished) : ActiveAnimation.Play(this.target, this.clipName, playDirection, this.ifDisabledOnPlay, this.disableWhenFinished);
            if (animation != null)
            {
                if (this.resetOnPlay)
                {
                    animation.Reset();
                }
                for (int i = 0; i < this.onFinished.Count; i++)
                {
                    EventDelegate.Add(animation.onFinished, new EventDelegate.Callback(this.OnFinished), true);
                }
            }
        }
    }

    private void Start()
    {
        this.mStarted = true;
        if ((this.target == null) && (this.animator == null))
        {
            this.animator = base.GetComponentInChildren<Animator>();
        }
        if (this.animator != null)
        {
            if (this.animator.enabled)
            {
                this.animator.enabled = false;
            }
        }
        else
        {
            if (this.target == null)
            {
                this.target = base.GetComponentInChildren<Animation>();
            }
            if ((this.target != null) && this.target.enabled)
            {
                this.target.enabled = false;
            }
        }
    }

    private bool dualState =>
        ((this.trigger == AnimationOrTween.Trigger.OnPress) || (this.trigger == AnimationOrTween.Trigger.OnHover));
}

