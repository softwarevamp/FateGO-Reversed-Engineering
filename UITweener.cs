using AnimationOrTween;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UITweener : MonoBehaviour
{
    [HideInInspector]
    public AnimationCurve animationCurve;
    [HideInInspector]
    public string callWhenFinished;
    public static UITweener current;
    [HideInInspector]
    public float delay;
    [HideInInspector]
    public float duration;
    [HideInInspector]
    public GameObject eventReceiver;
    [HideInInspector]
    public bool ignoreTimeScale;
    private float mAmountPerDelta;
    private float mDuration;
    [HideInInspector]
    public Method method;
    private float mFactor;
    private bool mStarted;
    private float mStartTime;
    private List<EventDelegate> mTemp;
    [HideInInspector]
    public List<EventDelegate> onFinished;
    [HideInInspector]
    public bool steeperCurves;
    [HideInInspector]
    public Style style;
    [HideInInspector]
    public int tweenGroup;

    protected UITweener()
    {
        Keyframe[] keys = new Keyframe[] { new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f) };
        this.animationCurve = new AnimationCurve(keys);
        this.ignoreTimeScale = true;
        this.duration = 1f;
        this.onFinished = new List<EventDelegate>();
        this.mAmountPerDelta = 1000f;
    }

    public void AddOnFinished(EventDelegate del)
    {
        EventDelegate.Add(this.onFinished, del);
    }

    public void AddOnFinished(EventDelegate.Callback del)
    {
        EventDelegate.Add(this.onFinished, del);
    }

    public static T Begin<T>(GameObject go, float duration) where T: UITweener
    {
        T component = go.GetComponent<T>();
        if ((component != null) && (component.tweenGroup != 0))
        {
            component = null;
            T[] components = go.GetComponents<T>();
            int index = 0;
            int length = components.Length;
            while (index < length)
            {
                component = components[index];
                if ((component != null) && (component.tweenGroup == 0))
                {
                    break;
                }
                component = null;
                index++;
            }
        }
        if (component == null)
        {
            component = go.AddComponent<T>();
            if (component == null)
            {
                Debug.LogError(string.Concat(new object[] { "Unable to add ", typeof(T), " to ", NGUITools.GetHierarchy(go) }), go);
                return null;
            }
        }
        component.mStarted = false;
        component.duration = duration;
        component.mFactor = 0f;
        component.mAmountPerDelta = Mathf.Abs(component.amountPerDelta);
        component.style = Style.Once;
        Keyframe[] keys = new Keyframe[] { new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f) };
        component.animationCurve = new AnimationCurve(keys);
        component.eventReceiver = null;
        component.callWhenFinished = null;
        component.enabled = true;
        return component;
    }

    private float BounceLogic(float val)
    {
        if (val < 0.363636f)
        {
            val = (7.5685f * val) * val;
            return val;
        }
        if (val < 0.727272f)
        {
            val = ((7.5625f * (val -= 0.545454f)) * val) + 0.75f;
            return val;
        }
        if (val < 0.90909f)
        {
            val = ((7.5625f * (val -= 0.818181f)) * val) + 0.9375f;
            return val;
        }
        val = ((7.5625f * (val -= 0.9545454f)) * val) + 0.984375f;
        return val;
    }

    private void OnDisable()
    {
        this.mStarted = false;
    }

    protected abstract void OnUpdate(float factor, bool isFinished);
    [Obsolete("Use PlayForward() instead")]
    public void Play()
    {
        this.Play(true);
    }

    public void Play(bool forward)
    {
        this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
        if (!forward)
        {
            this.mAmountPerDelta = -this.mAmountPerDelta;
        }
        base.enabled = true;
        this.Update();
    }

    public void PlayForward()
    {
        this.Play(true);
    }

    public void PlayReverse()
    {
        this.Play(false);
    }

    public void RemoveOnFinished(EventDelegate del)
    {
        if (this.onFinished != null)
        {
            this.onFinished.Remove(del);
        }
        if (this.mTemp != null)
        {
            this.mTemp.Remove(del);
        }
    }

    private void Reset()
    {
        if (!this.mStarted)
        {
            this.SetStartToCurrentValue();
            this.SetEndToCurrentValue();
        }
    }

    public void ResetToBeginning()
    {
        this.mStarted = false;
        this.mFactor = (this.amountPerDelta >= 0f) ? 0f : 1f;
        this.Sample(this.mFactor, false);
    }

    public void Sample(float factor, bool isFinished)
    {
        float f = Mathf.Clamp01(factor);
        if (this.method == Method.EaseIn)
        {
            f = 1f - Mathf.Sin(1.570796f * (1f - f));
            if (this.steeperCurves)
            {
                f *= f;
            }
        }
        else if (this.method == Method.EaseOut)
        {
            f = Mathf.Sin(1.570796f * f);
            if (this.steeperCurves)
            {
                f = 1f - f;
                f = 1f - (f * f);
            }
        }
        else if (this.method == Method.EaseOutQuad)
        {
            f = 1f - Mathf.Pow(1f - f, 2f);
        }
        else if (this.method == Method.EaseInOut)
        {
            f -= Mathf.Sin(f * 6.283185f) / 6.283185f;
            if (this.steeperCurves)
            {
                f = (f * 2f) - 1f;
                float num3 = Mathf.Sign(f);
                f = 1f - Mathf.Abs(f);
                f = 1f - (f * f);
                f = ((num3 * f) * 0.5f) + 0.5f;
            }
        }
        else if (this.method == Method.BounceIn)
        {
            f = this.BounceLogic(f);
        }
        else if (this.method == Method.BounceOut)
        {
            f = 1f - this.BounceLogic(1f - f);
        }
        this.OnUpdate((this.animationCurve == null) ? f : this.animationCurve.Evaluate(f), isFinished);
    }

    public virtual void SetEndToCurrentValue()
    {
    }

    public void SetOnFinished(EventDelegate del)
    {
        EventDelegate.Set(this.onFinished, del);
    }

    public void SetOnFinished(EventDelegate.Callback del)
    {
        EventDelegate.Set(this.onFinished, del);
    }

    public virtual void SetStartToCurrentValue()
    {
    }

    protected virtual void Start()
    {
        this.Update();
    }

    public virtual bool SynchronizeTween(UITweener tween)
    {
        if ((this.style == tween.style) && (this.duration == tween.duration))
        {
            this.mAmountPerDelta = tween.amountPerDelta;
            this.mFactor = tween.mFactor;
            return true;
        }
        return false;
    }

    public void Toggle()
    {
        if (this.mFactor > 0f)
        {
            this.mAmountPerDelta = -this.amountPerDelta;
        }
        else
        {
            this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
        }
        base.enabled = true;
    }

    private void Update()
    {
        float num = !this.ignoreTimeScale ? Time.deltaTime : RealTime.deltaTime;
        float num2 = !this.ignoreTimeScale ? Time.time : RealTime.time;
        float num3 = 1f / ((float) Application.targetFrameRate);
        if (num > num3)
        {
            num = num3;
        }
        if (!this.mStarted)
        {
            this.mStarted = true;
            this.mStartTime = num2 + this.delay;
        }
        if (num2 >= this.mStartTime)
        {
            this.mFactor += this.amountPerDelta * num;
            if (this.style == Style.Loop)
            {
                if (this.mFactor > 1f)
                {
                    this.mFactor -= Mathf.Floor(this.mFactor);
                }
            }
            else if (this.style == Style.PingPong)
            {
                if (this.mFactor > 1f)
                {
                    this.mFactor = 1f - (this.mFactor - Mathf.Floor(this.mFactor));
                    this.mAmountPerDelta = -this.mAmountPerDelta;
                }
                else if (this.mFactor < 0f)
                {
                    this.mFactor = -this.mFactor;
                    this.mFactor -= Mathf.Floor(this.mFactor);
                    this.mAmountPerDelta = -this.mAmountPerDelta;
                }
            }
            if ((this.style == Style.Once) && (((this.duration == 0f) || (this.mFactor > 1f)) || (this.mFactor < 0f)))
            {
                this.mFactor = Mathf.Clamp01(this.mFactor);
                this.Sample(this.mFactor, true);
                if (((this.duration == 0f) || ((this.mFactor == 1f) && (this.mAmountPerDelta > 0f))) || ((this.mFactor == 0f) && (this.mAmountPerDelta < 0f)))
                {
                    base.enabled = false;
                }
                if (current == null)
                {
                    current = this;
                    if (this.onFinished != null)
                    {
                        this.mTemp = this.onFinished;
                        this.onFinished = new List<EventDelegate>();
                        EventDelegate.Execute(this.mTemp);
                        for (int i = 0; i < this.mTemp.Count; i++)
                        {
                            EventDelegate ev = this.mTemp[i];
                            if ((ev != null) && !ev.oneShot)
                            {
                                EventDelegate.Add(this.onFinished, ev, ev.oneShot);
                            }
                        }
                        this.mTemp = null;
                    }
                    if ((this.eventReceiver != null) && !string.IsNullOrEmpty(this.callWhenFinished))
                    {
                        this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
                    }
                    current = null;
                }
            }
            else
            {
                this.Sample(this.mFactor, false);
            }
        }
    }

    public float amountPerDelta
    {
        get
        {
            if (this.mDuration != this.duration)
            {
                this.mDuration = this.duration;
                this.mAmountPerDelta = Mathf.Abs((this.duration <= 0f) ? 1000f : (1f / this.duration)) * Mathf.Sign(this.mAmountPerDelta);
            }
            return this.mAmountPerDelta;
        }
    }

    public AnimationOrTween.Direction direction =>
        ((this.amountPerDelta >= 0f) ? AnimationOrTween.Direction.Forward : AnimationOrTween.Direction.Reverse);

    public float tweenFactor
    {
        get => 
            this.mFactor;
        set
        {
            this.mFactor = Mathf.Clamp01(value);
        }
    }

    public enum Method
    {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut,
        BounceIn,
        BounceOut,
        EaseOutQuad
    }

    public enum Style
    {
        Once,
        Loop,
        PingPong
    }
}

