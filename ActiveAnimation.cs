using AnimationOrTween;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Active Animation")]
public class ActiveAnimation : MonoBehaviour
{
    [HideInInspector]
    public string callWhenFinished;
    public static ActiveAnimation current;
    [HideInInspector]
    public GameObject eventReceiver;
    private Animation mAnim;
    private Animator mAnimator;
    private string mClip = string.Empty;
    private AnimationOrTween.Direction mDisableDirection;
    private AnimationOrTween.Direction mLastDirection;
    private bool mNotify;
    public List<EventDelegate> onFinished = new List<EventDelegate>();

    public void Finish()
    {
        if (this.mAnim != null)
        {
            IEnumerator enumerator = this.mAnim.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AnimationState current = (AnimationState) enumerator.Current;
                    if (this.mLastDirection == AnimationOrTween.Direction.Forward)
                    {
                        current.time = current.length;
                    }
                    else if (this.mLastDirection == AnimationOrTween.Direction.Reverse)
                    {
                        current.time = 0f;
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
            this.mAnim.Sample();
        }
        else if (this.mAnimator != null)
        {
            this.mAnimator.Play(this.mClip, 0, (this.mLastDirection != AnimationOrTween.Direction.Forward) ? 0f : 1f);
        }
    }

    private void Play(string clipName, AnimationOrTween.Direction playDirection)
    {
        if (playDirection == AnimationOrTween.Direction.Toggle)
        {
            playDirection = (this.mLastDirection == AnimationOrTween.Direction.Forward) ? AnimationOrTween.Direction.Reverse : AnimationOrTween.Direction.Forward;
        }
        if (this.mAnim != null)
        {
            base.enabled = true;
            this.mAnim.enabled = false;
            if (string.IsNullOrEmpty(clipName))
            {
                if (!this.mAnim.isPlaying)
                {
                    this.mAnim.Play();
                }
            }
            else if (!this.mAnim.IsPlaying(clipName))
            {
                this.mAnim.Play(clipName);
            }
            IEnumerator enumerator = this.mAnim.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AnimationState current = (AnimationState) enumerator.Current;
                    if (string.IsNullOrEmpty(clipName) || (current.name == clipName))
                    {
                        float num = Mathf.Abs(current.speed);
                        current.speed = num * ((float) playDirection);
                        if ((playDirection == AnimationOrTween.Direction.Reverse) && (current.time == 0f))
                        {
                            current.time = current.length;
                        }
                        else if ((playDirection == AnimationOrTween.Direction.Forward) && (current.time == current.length))
                        {
                            current.time = 0f;
                        }
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
            this.mLastDirection = playDirection;
            this.mNotify = true;
            this.mAnim.Sample();
        }
        else if (this.mAnimator != null)
        {
            if ((base.enabled && this.isPlaying) && (this.mClip == clipName))
            {
                this.mLastDirection = playDirection;
            }
            else
            {
                base.enabled = true;
                this.mNotify = true;
                this.mLastDirection = playDirection;
                this.mClip = clipName;
                this.mAnimator.Play(this.mClip, 0, (playDirection != AnimationOrTween.Direction.Forward) ? 1f : 0f);
            }
        }
    }

    public static ActiveAnimation Play(Animation anim, AnimationOrTween.Direction playDirection) => 
        Play(anim, null, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);

    public static ActiveAnimation Play(Animation anim, string clipName, AnimationOrTween.Direction playDirection) => 
        Play(anim, clipName, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);

    public static ActiveAnimation Play(Animation anim, string clipName, AnimationOrTween.Direction playDirection, EnableCondition enableBeforePlay, DisableCondition disableCondition)
    {
        if (!NGUITools.GetActive(anim.gameObject))
        {
            if (enableBeforePlay != EnableCondition.EnableThenPlay)
            {
                return null;
            }
            NGUITools.SetActive(anim.gameObject, true);
            UIPanel[] componentsInChildren = anim.gameObject.GetComponentsInChildren<UIPanel>();
            int index = 0;
            int length = componentsInChildren.Length;
            while (index < length)
            {
                componentsInChildren[index].Refresh();
                index++;
            }
        }
        ActiveAnimation component = anim.GetComponent<ActiveAnimation>();
        if (component == null)
        {
            component = anim.gameObject.AddComponent<ActiveAnimation>();
        }
        component.mAnim = anim;
        component.mDisableDirection = (AnimationOrTween.Direction) disableCondition;
        component.onFinished.Clear();
        component.Play(clipName, playDirection);
        if (component.mAnim != null)
        {
            component.mAnim.Sample();
            return component;
        }
        if (component.mAnimator != null)
        {
            component.mAnimator.Update(0f);
        }
        return component;
    }

    public static ActiveAnimation Play(Animator anim, string clipName, AnimationOrTween.Direction playDirection, EnableCondition enableBeforePlay, DisableCondition disableCondition)
    {
        if ((enableBeforePlay != EnableCondition.IgnoreDisabledState) && !NGUITools.GetActive(anim.gameObject))
        {
            if (enableBeforePlay != EnableCondition.EnableThenPlay)
            {
                return null;
            }
            NGUITools.SetActive(anim.gameObject, true);
            UIPanel[] componentsInChildren = anim.gameObject.GetComponentsInChildren<UIPanel>();
            int index = 0;
            int length = componentsInChildren.Length;
            while (index < length)
            {
                componentsInChildren[index].Refresh();
                index++;
            }
        }
        ActiveAnimation component = anim.GetComponent<ActiveAnimation>();
        if (component == null)
        {
            component = anim.gameObject.AddComponent<ActiveAnimation>();
        }
        component.mAnimator = anim;
        component.mDisableDirection = (AnimationOrTween.Direction) disableCondition;
        component.onFinished.Clear();
        component.Play(clipName, playDirection);
        if (component.mAnim != null)
        {
            component.mAnim.Sample();
            return component;
        }
        if (component.mAnimator != null)
        {
            component.mAnimator.Update(0f);
        }
        return component;
    }

    public void Reset()
    {
        if (this.mAnim != null)
        {
            IEnumerator enumerator = this.mAnim.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AnimationState current = (AnimationState) enumerator.Current;
                    if (this.mLastDirection == AnimationOrTween.Direction.Reverse)
                    {
                        current.time = current.length;
                    }
                    else if (this.mLastDirection == AnimationOrTween.Direction.Forward)
                    {
                        current.time = 0f;
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
        }
        else if (this.mAnimator != null)
        {
            this.mAnimator.Play(this.mClip, 0, (this.mLastDirection != AnimationOrTween.Direction.Reverse) ? 0f : 1f);
        }
    }

    private void Start()
    {
        if ((this.eventReceiver != null) && EventDelegate.IsValid(this.onFinished))
        {
            this.eventReceiver = null;
            this.callWhenFinished = null;
        }
    }

    private void Update()
    {
        float deltaTime = RealTime.deltaTime;
        if (deltaTime != 0f)
        {
            if (this.mAnimator != null)
            {
                this.mAnimator.Update((this.mLastDirection != AnimationOrTween.Direction.Reverse) ? deltaTime : -deltaTime);
                if (this.isPlaying)
                {
                    return;
                }
                this.mAnimator.enabled = false;
                base.enabled = false;
                goto Label_016C;
            }
            if (this.mAnim != null)
            {
                bool flag = false;
                IEnumerator enumerator = this.mAnim.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        AnimationState current = (AnimationState) enumerator.Current;
                        if (this.mAnim.IsPlaying(current.name))
                        {
                            float num2 = current.speed * deltaTime;
                            current.time += num2;
                            if (num2 < 0f)
                            {
                                if (current.time > 0f)
                                {
                                    flag = true;
                                }
                                else
                                {
                                    current.time = 0f;
                                }
                            }
                            else if (current.time < current.length)
                            {
                                flag = true;
                            }
                            else
                            {
                                current.time = current.length;
                            }
                        }
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable == null)
                    {
                    }
                    disposable.Dispose();
                }
                this.mAnim.Sample();
                if (flag)
                {
                    return;
                }
                base.enabled = false;
                goto Label_016C;
            }
            base.enabled = false;
        }
        return;
    Label_016C:
        if (this.mNotify)
        {
            this.mNotify = false;
            if (ActiveAnimation.current == null)
            {
                ActiveAnimation.current = this;
                EventDelegate.Execute(this.onFinished);
                if ((this.eventReceiver != null) && !string.IsNullOrEmpty(this.callWhenFinished))
                {
                    this.eventReceiver.SendMessage(this.callWhenFinished, SendMessageOptions.DontRequireReceiver);
                }
                ActiveAnimation.current = null;
            }
            if ((this.mDisableDirection != AnimationOrTween.Direction.Toggle) && (this.mLastDirection == this.mDisableDirection))
            {
                NGUITools.SetActive(base.gameObject, false);
            }
        }
    }

    public bool isPlaying
    {
        get
        {
            if (this.mAnim == null)
            {
                if (this.mAnimator == null)
                {
                    return false;
                }
                if (this.mLastDirection == AnimationOrTween.Direction.Reverse)
                {
                    if (this.playbackTime == 0f)
                    {
                        return false;
                    }
                }
                else if (this.playbackTime == 1f)
                {
                    return false;
                }
                return true;
            }
            IEnumerator enumerator = this.mAnim.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AnimationState current = (AnimationState) enumerator.Current;
                    if (this.mAnim.IsPlaying(current.name))
                    {
                        if (this.mLastDirection == AnimationOrTween.Direction.Forward)
                        {
                            if (current.time < current.length)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (this.mLastDirection == AnimationOrTween.Direction.Reverse)
                            {
                                if (current.time > 0f)
                                {
                                    return true;
                                }
                                continue;
                            }
                            return true;
                        }
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
            return false;
        }
    }

    private float playbackTime =>
        Mathf.Clamp01(this.mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
}

