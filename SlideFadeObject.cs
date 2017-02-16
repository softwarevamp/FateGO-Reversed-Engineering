using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class SlideFadeObject : CachableMonoBehaviour
{
    private Vector3 mInitPos;
    private Vector3 mLeftPos;
    private MoveObject mMoveObject;
    private UIPanel mPanel;
    private Vector3 mRightPos;
    private SLIDE_STATE mSlideState;

    private void Awake()
    {
        this.mMoveObject = base.gameObject.SafeGetComponent<MoveObject>();
        this.mPanel = base.gameObject.GetComponent<UIPanel>();
        if (this.mPanel == null)
        {
            this.mPanel = base.transform.GetComponentInChildren<UIPanel>();
        }
        this.Refresh();
    }

    public SLIDE_STATE GetSlideState() => 
        this.mSlideState;

    public void Refresh()
    {
        this.mInitPos = base.gameObject.GetLocalPosition();
        this.mLeftPos = new Vector3((float) -ManagerConfig.WIDTH, this.mInitPos.y, 0f);
        this.mRightPos = new Vector3((float) ManagerConfig.WIDTH, this.mInitPos.y, 0f);
        this.mSlideState = SLIDE_STATE.NONE;
    }

    public void Skip()
    {
        this.mMoveObject.Skip();
    }

    public void SlideIn(STYLE style, float time, float delay = 0, System.Action endAct = null)
    {
        Vector3 mLeftPos = this.mLeftPos;
        if (style == STYLE.LEFT)
        {
            mLeftPos = this.mRightPos;
        }
        this.SlideIn(mLeftPos, time, delay, endAct);
    }

    public void SlideIn(float out_pos_ofs_x, float time, float delay = 0, System.Action endAct = null)
    {
        Vector3 mInitPos = this.mInitPos;
        mInitPos.x += out_pos_ofs_x;
        this.SlideIn(mInitPos, time, delay, endAct);
    }

    public void SlideIn(Vector3 startPos, float time, float delay = 0, System.Action endAct = null)
    {
        <SlideIn>c__AnonStoreyAE yae = new <SlideIn>c__AnonStoreyAE {
            endAct = endAct,
            <>f__this = this
        };
        this.mSlideState = SLIDE_STATE.NONE;
        base.gameObject.SetLocalPosition(startPos);
        this.mMoveObject.Play(startPos, this.mInitPos, time, new System.Action(yae.<>m__19B), new System.Action(yae.<>m__19C), delay, Easing.TYPE.EXPONENTIAL_OUT);
    }

    public void SlideOut(STYLE style, float time, float delay = 0, System.Action endAct = null)
    {
        Vector3 mLeftPos = this.mLeftPos;
        if (style == STYLE.RIGHT)
        {
            mLeftPos = this.mRightPos;
        }
        this.SlideOut(mLeftPos, time, delay, endAct);
    }

    public void SlideOut(float out_pos_ofs_x, float time, float delay = 0, System.Action endAct = null)
    {
        Vector3 mInitPos = this.mInitPos;
        mInitPos.x += out_pos_ofs_x;
        this.SlideOut(mInitPos, time, delay, endAct);
    }

    public void SlideOut(Vector3 endPos, float time, float delay = 0, System.Action endAct = null)
    {
        <SlideOut>c__AnonStoreyAF yaf = new <SlideOut>c__AnonStoreyAF {
            endAct = endAct,
            <>f__this = this
        };
        this.mSlideState = SLIDE_STATE.NONE;
        this.mMoveObject.Play(this.mInitPos, endPos, time, new System.Action(yaf.<>m__19D), new System.Action(yaf.<>m__19E), delay, Easing.TYPE.EXPONENTIAL_OUT);
    }

    [CompilerGenerated]
    private sealed class <SlideIn>c__AnonStoreyAE
    {
        internal SlideFadeObject <>f__this;
        internal System.Action endAct;

        internal void <>m__19B()
        {
            this.<>f__this.gameObject.SetLocalPositionX(this.<>f__this.mMoveObject.Now().x);
            if (this.<>f__this.mPanel != null)
            {
                this.<>f__this.mPanel.Invalidate(true);
            }
        }

        internal void <>m__19C()
        {
            this.<>f__this.mSlideState = SlideFadeObject.SLIDE_STATE.IN_COMPLETE;
            if (this.<>f__this.mPanel != null)
            {
                this.<>f__this.mPanel.Invalidate(true);
            }
            this.endAct.Call();
        }
    }

    [CompilerGenerated]
    private sealed class <SlideOut>c__AnonStoreyAF
    {
        internal SlideFadeObject <>f__this;
        internal System.Action endAct;

        internal void <>m__19D()
        {
            this.<>f__this.gameObject.SetLocalPositionX(this.<>f__this.mMoveObject.Now().x);
            if (this.<>f__this.mPanel != null)
            {
                this.<>f__this.mPanel.Invalidate(true);
            }
        }

        internal void <>m__19E()
        {
            this.<>f__this.mSlideState = SlideFadeObject.SLIDE_STATE.OUT_COMPLETE;
            this.endAct.Call();
        }
    }

    public enum SLIDE_STATE
    {
        NONE,
        IN_COMPLETE,
        OUT_COMPLETE
    }

    public enum STYLE
    {
        LEFT,
        RIGHT
    }
}

