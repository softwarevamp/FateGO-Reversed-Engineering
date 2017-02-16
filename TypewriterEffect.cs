using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(UILabel)), AddComponentMenu("NGUI/Interaction/Typewriter Effect")]
public class TypewriterEffect : MonoBehaviour
{
    public int charsPerSecond = 20;
    public static TypewriterEffect current;
    public float delayOnNewLine;
    public float delayOnPeriod;
    public float fadeInTime;
    public bool keepFullDimensions;
    private bool mActive;
    private int mCurrentOffset;
    private BetterList<FadeEntry> mFade = new BetterList<FadeEntry>();
    private string mFullText = string.Empty;
    private UILabel mLabel;
    private float mNextChar;
    private bool mReset = true;
    public List<EventDelegate> onFinished = new List<EventDelegate>();
    public UIScrollView scrollView;

    public void Finish()
    {
        if (this.mActive)
        {
            this.mActive = false;
            if (!this.mReset)
            {
                this.mCurrentOffset = this.mFullText.Length;
                this.mFade.Clear();
                this.mLabel.text = this.mFullText;
            }
            if (this.keepFullDimensions && (this.scrollView != null))
            {
                this.scrollView.UpdatePosition();
            }
            current = this;
            EventDelegate.Execute(this.onFinished);
            current = null;
        }
    }

    private void OnEnable()
    {
        this.mReset = true;
        this.mActive = true;
    }

    public void ResetToBeginning()
    {
        this.Finish();
        this.mReset = true;
        this.mActive = true;
        this.mNextChar = 0f;
        this.mCurrentOffset = 0;
        this.Update();
    }

    private void Update()
    {
        if (this.mActive)
        {
            if (this.mReset)
            {
                this.mCurrentOffset = 0;
                this.mReset = false;
                this.mLabel = base.GetComponent<UILabel>();
                this.mFullText = this.mLabel.processedText;
                this.mFade.Clear();
                if (this.keepFullDimensions && (this.scrollView != null))
                {
                    this.scrollView.UpdatePosition();
                }
            }
            while ((this.mCurrentOffset < this.mFullText.Length) && (this.mNextChar <= RealTime.time))
            {
                int mCurrentOffset = this.mCurrentOffset;
                this.charsPerSecond = Mathf.Max(1, this.charsPerSecond);
                while (NGUIText.ParseSymbol(this.mFullText, ref this.mCurrentOffset))
                {
                }
                this.mCurrentOffset++;
                if (this.mCurrentOffset > this.mFullText.Length)
                {
                    break;
                }
                float num2 = 1f / ((float) this.charsPerSecond);
                char ch = (mCurrentOffset >= this.mFullText.Length) ? '\n' : this.mFullText[mCurrentOffset];
                if (ch == '\n')
                {
                    num2 += this.delayOnNewLine;
                }
                else if (((mCurrentOffset + 1) == this.mFullText.Length) || (this.mFullText[mCurrentOffset + 1] <= ' '))
                {
                    switch (ch)
                    {
                        case '.':
                            if ((((mCurrentOffset + 2) < this.mFullText.Length) && (this.mFullText[mCurrentOffset + 1] == '.')) && (this.mFullText[mCurrentOffset + 2] == '.'))
                            {
                                num2 += this.delayOnPeriod * 3f;
                                mCurrentOffset += 2;
                            }
                            else
                            {
                                num2 += this.delayOnPeriod;
                            }
                            break;

                        case '!':
                        case '?':
                            num2 += this.delayOnPeriod;
                            break;
                    }
                }
                if (this.mNextChar == 0f)
                {
                    this.mNextChar = RealTime.time + num2;
                }
                else
                {
                    this.mNextChar += num2;
                }
                if (this.fadeInTime != 0f)
                {
                    FadeEntry item = new FadeEntry {
                        index = mCurrentOffset,
                        alpha = 0f,
                        text = this.mFullText.Substring(mCurrentOffset, this.mCurrentOffset - mCurrentOffset)
                    };
                    this.mFade.Add(item);
                }
                else
                {
                    this.mLabel.text = !this.keepFullDimensions ? this.mFullText.Substring(0, this.mCurrentOffset) : (this.mFullText.Substring(0, this.mCurrentOffset) + "[00]" + this.mFullText.Substring(this.mCurrentOffset));
                    if (!this.keepFullDimensions && (this.scrollView != null))
                    {
                        this.scrollView.UpdatePosition();
                    }
                }
            }
            if (this.mFade.size != 0)
            {
                int index = 0;
                while (index < this.mFade.size)
                {
                    FadeEntry entry2 = this.mFade[index];
                    entry2.alpha += RealTime.deltaTime / this.fadeInTime;
                    if (entry2.alpha < 1f)
                    {
                        this.mFade[index] = entry2;
                        index++;
                    }
                    else
                    {
                        this.mFade.RemoveAt(index);
                    }
                }
                if (this.mFade.size == 0)
                {
                    if (this.keepFullDimensions)
                    {
                        this.mLabel.text = this.mFullText.Substring(0, this.mCurrentOffset) + "[00]" + this.mFullText.Substring(this.mCurrentOffset);
                    }
                    else
                    {
                        this.mLabel.text = this.mFullText.Substring(0, this.mCurrentOffset);
                    }
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < this.mFade.size; i++)
                    {
                        FadeEntry entry3 = this.mFade[i];
                        if (i == 0)
                        {
                            builder.Append(this.mFullText.Substring(0, entry3.index));
                        }
                        builder.Append('[');
                        builder.Append(NGUIText.EncodeAlpha(entry3.alpha));
                        builder.Append(']');
                        builder.Append(entry3.text);
                    }
                    if (this.keepFullDimensions)
                    {
                        builder.Append("[00]");
                        builder.Append(this.mFullText.Substring(this.mCurrentOffset));
                    }
                    this.mLabel.text = builder.ToString();
                }
            }
            else if (this.mCurrentOffset == this.mFullText.Length)
            {
                current = this;
                EventDelegate.Execute(this.onFinished);
                current = null;
                this.mActive = false;
            }
        }
    }

    public bool isActive =>
        this.mActive;

    [StructLayout(LayoutKind.Sequential)]
    private struct FadeEntry
    {
        public int index;
        public string text;
        public float alpha;
    }
}

