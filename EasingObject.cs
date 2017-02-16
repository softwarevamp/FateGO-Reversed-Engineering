using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class EasingObject : MonoBehaviour
{
    private float mDelay;
    private Easing.TYPE mEasingType;
    private System.Action mEndAct;
    private float mFrom;
    private bool mIsMoving;
    private bool mIsPause;
    private bool mIsSkip;
    private float mNow;
    private float mPauseStartTime;
    private System.Action mProcessAct;
    private float mStartTime;
    private float mTime;
    private float mTo;

    private void Awake()
    {
        this.mIsMoving = false;
        this.mIsSkip = false;
    }

    public float Now() => 
        this.mNow;

    public void Pause()
    {
        if (!this.mIsPause)
        {
            this.mIsPause = true;
            this.mPauseStartTime = Time.time;
        }
    }

    public void Play(float sec, System.Action procAct = null, System.Action endAct = null, float delay = 0, Easing.TYPE easingType = 0)
    {
        this.Play(0f, 1f, sec, procAct, endAct, delay, easingType);
    }

    public void Play(float from, float to, float sec, System.Action procAct = null, System.Action endAct = null, float delay = 0, Easing.TYPE easingType = 0)
    {
        if (sec <= 0f)
        {
            sec = 0.0001f;
        }
        this.mIsMoving = true;
        this.mFrom = from;
        this.mTo = to;
        this.mStartTime = Time.time;
        this.mTime = sec;
        this.mEndAct = endAct;
        this.mNow = from;
        this.mProcessAct = procAct;
        this.mEasingType = easingType;
        this.mDelay = delay;
        this.mIsSkip = false;
        this.mProcessAct.Call();
    }

    public void Resume()
    {
        if (this.mIsPause)
        {
            this.mIsPause = false;
            float num2 = Time.time - this.mPauseStartTime;
            this.mStartTime += num2;
        }
    }

    public void SetPause(bool isPause)
    {
        if (isPause)
        {
            this.Pause();
        }
        else
        {
            this.Resume();
        }
    }

    public void Skip()
    {
        this.mIsSkip = true;
    }

    public void Stop()
    {
        this.mIsMoving = false;
    }

    private void Update()
    {
        if ((this.mIsMoving && !this.mIsPause) && (this.mIsSkip || ((this.mStartTime + this.mDelay) <= Time.time)))
        {
            float t = Time.time - (this.mStartTime + this.mDelay);
            t = Mathf.Clamp01(t / this.mTime);
            if (this.mIsSkip)
            {
                t = 1f;
            }
            this.mNow = Easing.Func(this.mFrom, this.mTo, t, this.mEasingType);
            if (this.mProcessAct != null)
            {
                this.mProcessAct();
            }
            if (t >= 1f)
            {
                this.mIsMoving = false;
                if (this.mEndAct != null)
                {
                    this.mEndAct();
                }
            }
        }
    }

    public bool IsMoving =>
        this.mIsMoving;

    public bool IsPause =>
        this.mIsPause;
}

