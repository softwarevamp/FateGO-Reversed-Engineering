using AnimationOrTween;
using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/Interaction/Play Tween")]
public class UIPlayTween : MonoBehaviour
{
    [HideInInspector, SerializeField]
    private string callWhenFinished;
    public static UIPlayTween current;
    public DisableCondition disableWhenFinished;
    [SerializeField, HideInInspector]
    private GameObject eventReceiver;
    public EnableCondition ifDisabledOnPlay;
    public bool includeChildren;
    private bool mActivated;
    private int mActive;
    private bool mStarted;
    private UITweener[] mTweens;
    public List<EventDelegate> onFinished = new List<EventDelegate>();
    public AnimationOrTween.Direction playDirection = AnimationOrTween.Direction.Forward;
    public bool resetIfDisabled;
    public bool resetOnPlay;
    public AnimationOrTween.Trigger trigger;
    public int tweenGroup;
    public GameObject tweenTarget;

    private void Awake()
    {
        if ((this.eventReceiver != null) && EventDelegate.IsValid(this.onFinished))
        {
            this.eventReceiver = null;
            this.callWhenFinished = null;
        }
    }

    private void OnClick()
    {
        if (base.enabled && (this.trigger == AnimationOrTween.Trigger.OnClick))
        {
            this.Play(true);
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
        if (base.enabled && (this.trigger == AnimationOrTween.Trigger.OnDoubleClick))
        {
            this.Play(true);
        }
    }

    private void OnDragOut()
    {
        if (base.enabled && this.mActivated)
        {
            this.mActivated = false;
            this.Play(false);
        }
    }

    private void OnDragOver()
    {
        if (this.trigger == AnimationOrTween.Trigger.OnHover)
        {
            this.OnHover(true);
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
        if ((--this.mActive == 0) && (current == null))
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
            this.mActivated = isOver && (this.trigger == AnimationOrTween.Trigger.OnHover);
            this.Play(isOver);
        }
    }

    private void OnPress(bool isPressed)
    {
        if (base.enabled && (((this.trigger == AnimationOrTween.Trigger.OnPress) || ((this.trigger == AnimationOrTween.Trigger.OnPressTrue) && isPressed)) || ((this.trigger == AnimationOrTween.Trigger.OnPressFalse) && !isPressed)))
        {
            this.mActivated = isPressed && (this.trigger == AnimationOrTween.Trigger.OnPress);
            this.Play(isPressed);
        }
    }

    private void OnSelect(bool isSelected)
    {
        if (base.enabled && (((this.trigger == AnimationOrTween.Trigger.OnSelect) || ((this.trigger == AnimationOrTween.Trigger.OnSelectTrue) && isSelected)) || ((this.trigger == AnimationOrTween.Trigger.OnSelectFalse) && !isSelected)))
        {
            this.mActivated = isSelected && (this.trigger == AnimationOrTween.Trigger.OnSelect);
            this.Play(isSelected);
        }
    }

    private void OnToggle()
    {
        if ((base.enabled && (UIToggle.current != null)) && (((this.trigger == AnimationOrTween.Trigger.OnActivate) || ((this.trigger == AnimationOrTween.Trigger.OnActivateTrue) && UIToggle.current.value)) || ((this.trigger == AnimationOrTween.Trigger.OnActivateFalse) && !UIToggle.current.value)))
        {
            this.Play(UIToggle.current.value);
        }
    }

    public void Play(bool forward)
    {
        this.mActive = 0;
        GameObject go = (this.tweenTarget != null) ? this.tweenTarget : base.gameObject;
        if (!NGUITools.GetActive(go))
        {
            if (this.ifDisabledOnPlay != EnableCondition.EnableThenPlay)
            {
                return;
            }
            NGUITools.SetActive(go, true);
        }
        this.mTweens = !this.includeChildren ? go.GetComponents<UITweener>() : go.GetComponentsInChildren<UITweener>();
        if (this.mTweens.Length == 0)
        {
            if (this.disableWhenFinished != DisableCondition.DoNotDisable)
            {
                NGUITools.SetActive(this.tweenTarget, false);
            }
        }
        else
        {
            bool flag = false;
            if (this.playDirection == AnimationOrTween.Direction.Reverse)
            {
                forward = !forward;
            }
            int index = 0;
            int length = this.mTweens.Length;
            while (index < length)
            {
                UITweener tweener = this.mTweens[index];
                if (tweener.tweenGroup == this.tweenGroup)
                {
                    if (!flag && !NGUITools.GetActive(go))
                    {
                        flag = true;
                        NGUITools.SetActive(go, true);
                    }
                    this.mActive++;
                    if (this.playDirection == AnimationOrTween.Direction.Toggle)
                    {
                        EventDelegate.Add(tweener.onFinished, new EventDelegate.Callback(this.OnFinished), true);
                        tweener.Toggle();
                    }
                    else
                    {
                        if (this.resetOnPlay || (this.resetIfDisabled && !tweener.enabled))
                        {
                            tweener.Play(forward);
                            tweener.ResetToBeginning();
                        }
                        EventDelegate.Add(tweener.onFinished, new EventDelegate.Callback(this.OnFinished), true);
                        tweener.Play(forward);
                    }
                }
                index++;
            }
        }
    }

    private void Start()
    {
        this.mStarted = true;
        if (this.tweenTarget == null)
        {
            this.tweenTarget = base.gameObject;
        }
    }

    private void Update()
    {
        if ((this.disableWhenFinished != DisableCondition.DoNotDisable) && (this.mTweens != null))
        {
            bool flag = true;
            bool flag2 = true;
            int index = 0;
            int length = this.mTweens.Length;
            while (index < length)
            {
                UITweener tweener = this.mTweens[index];
                if (tweener.tweenGroup == this.tweenGroup)
                {
                    if (tweener.enabled)
                    {
                        flag = false;
                        break;
                    }
                    if (tweener.direction != ((AnimationOrTween.Direction) ((int) this.disableWhenFinished)))
                    {
                        flag2 = false;
                    }
                }
                index++;
            }
            if (flag)
            {
                if (flag2)
                {
                    NGUITools.SetActive(this.tweenTarget, false);
                }
                this.mTweens = null;
            }
        }
    }
}

