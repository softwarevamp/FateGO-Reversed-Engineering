using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    private int mCount;
    private float mDelay;
    private Easing.TYPE mEasingType;
    private System.Action mEndAct;
    private Color mFromColor;
    private bool mIsChangeColor;
    private bool mIsSkip;
    private Color mNowColor;
    private int mNowCount;
    private bool mPause;
    private float mPauseStartTime;
    private System.Action mProcessAct;
    private float mStartTime;
    private CHANGE_STYLE mStyle;
    private float mTime;
    private Color mToColor;

    private void Awake()
    {
        this.mIsChangeColor = false;
        this.mIsSkip = false;
    }

    public Color GetColor() => 
        this.mNowColor;

    public void Pause()
    {
        if (!this.mPause)
        {
            this.mPause = true;
            this.mPauseStartTime = Time.time;
        }
    }

    private Color PingPong(Color from, Color to, float time, Easing.TYPE easingType = 0)
    {
        this.mIsSkip = false;
        Color white = Color.white;
        if (time >= 0.5f)
        {
            return Easing.Func(to, from, (time - 0.5f) * 2f, easingType);
        }
        return Easing.Func(from, to, time * 2f, easingType);
    }

    public void Play(Color from, Color to, float sec, CHANGE_STYLE style = 0, int count = 1, System.Action procAct = null, System.Action endAct = null, float delay = 0, Easing.TYPE easingType = 0)
    {
        this.mIsChangeColor = true;
        this.mFromColor = from;
        this.mToColor = to;
        this.mStartTime = Time.time;
        this.mTime = sec;
        this.mStyle = style;
        this.mCount = count;
        this.mNowCount = 0;
        this.mEndAct = endAct;
        this.mNowColor = from;
        this.mProcessAct = procAct;
        this.mEasingType = easingType;
        this.mDelay = delay;
        this.mIsSkip = false;
        this.mProcessAct.Call();
    }

    public void Resume()
    {
        if (this.mPause)
        {
            this.mPause = false;
            float num2 = Time.time - this.mPauseStartTime;
            this.mStartTime += num2;
        }
    }

    public void SetColor(Color color)
    {
        this.mNowColor = color;
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
        this.mIsChangeColor = false;
    }

    private void Update()
    {
        if ((this.mIsChangeColor && !this.mPause) && (this.mIsSkip || ((this.mStartTime + this.mDelay) <= Time.time)))
        {
            float time = Time.time - (this.mStartTime + this.mDelay);
            time = Mathf.Clamp01(time / this.mTime);
            if (this.mIsSkip)
            {
                time = 1f;
            }
            if (this.mStyle == CHANGE_STYLE.PINGPONG)
            {
                this.mNowColor = this.PingPong(this.mFromColor, this.mToColor, time, this.mEasingType);
            }
            else
            {
                this.mNowColor = Easing.Func(this.mFromColor, this.mToColor, time, this.mEasingType);
            }
            if (this.mProcessAct != null)
            {
                this.mProcessAct();
            }
            if (time >= 1f)
            {
                this.mNowCount++;
                if ((this.mCount <= this.mNowCount) && (this.mCount != 0))
                {
                    this.mIsChangeColor = false;
                    if (this.mEndAct != null)
                    {
                        this.mEndAct();
                    }
                }
                else
                {
                    this.mStartTime = Time.time;
                    if (this.mStyle == CHANGE_STYLE.ONE)
                    {
                        this.mNowColor = this.mToColor;
                    }
                    else if (this.mStyle == CHANGE_STYLE.PINGPONG)
                    {
                        this.mNowColor = this.mFromColor;
                    }
                }
            }
        }
    }

    public enum CHANGE_STYLE
    {
        ONE,
        PINGPONG
    }
}

