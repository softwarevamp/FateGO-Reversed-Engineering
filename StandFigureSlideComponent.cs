using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class StandFigureSlideComponent : MonoBehaviour
{
    public const float ALP_IN_SPD_RATE = 0.175f;
    public const float ALP_OUT_SPD_RATE = 0.45f;
    public const float BASE_POS_Y = 288f;
    public static readonly float FRAME_IN_POS_X = -540f;
    public static readonly float FRAME_OUT_POS_X = -ManagerConfig.WIDTH;
    protected System.Action mBtnAct;
    [SerializeField]
    protected BoxCollider mBtnColl;
    protected CStateManager<StandFigureSlideComponent> mFSM;
    protected bool mIsEnable;
    protected bool mIsFrameIn;
    protected bool mIsLoading;
    protected float mNowAlp;
    protected System.Action mSlideEndAct;
    protected float mSpdRateAlp;
    protected float mTgtAlp;
    protected float mTgtPosX;
    protected UIStandFigureR mUIStandFigureR;
    public const float POS_SPD_RATE = 0.35f;

    private void Awake()
    {
        if (this.mFSM == null)
        {
            this.mFSM = new CStateManager<StandFigureSlideComponent>(this, 3);
            this.mFSM.add(0, new StateNone());
            this.mFSM.add(1, new StateReady());
            this.mFSM.add(2, new StateSlide());
            this.SetState(STATE.NONE);
        }
        base.gameObject.SetLocalPositionY(288f);
        this.SetEnable(true);
    }

    private void Destroy()
    {
        if (this.mUIStandFigureR != null)
        {
            UnityEngine.Object.Destroy(this.mUIStandFigureR.gameObject);
            this.mUIStandFigureR = null;
            this.SetState(STATE.NONE);
        }
    }

    protected STATE GetState() => 
        ((STATE) this.mFSM.getState());

    public bool IsEnable() => 
        this.mIsEnable;

    public bool IsFrameIn() => 
        this.mIsFrameIn;

    public bool IsLoding() => 
        this.mIsLoading;

    public bool IsMoving() => 
        (this.GetState() == STATE.SLIDE);

    public void OnClickBtn()
    {
        if (this.GetState() != STATE.NONE)
        {
            this.mBtnAct.Call();
        }
    }

    private void OnDestroy()
    {
        this.Destroy();
    }

    public void SetBtnAct(System.Action btn_act)
    {
        this.mBtnAct = btn_act;
    }

    public void SetEnable(bool is_enable)
    {
        this.mIsEnable = is_enable;
        this.mBtnColl.enabled = this.mIsEnable;
    }

    protected void SetState(STATE state)
    {
        this.mFSM.setState((int) state);
    }

    public void Setup(UserServantEntity usd, int depth, System.Action end_act)
    {
        <Setup>c__AnonStoreyB0 yb = new <Setup>c__AnonStoreyB0 {
            end_act = end_act,
            <>f__this = this
        };
        yb.this_end_act = new System.Action(yb.<>m__19F);
        this.mIsLoading = true;
        this.SetState(STATE.NONE);
        bool flag = false;
        if (usd != null)
        {
            if (this.mUIStandFigureR != null)
            {
                flag = usd.getSvtId() != this.mUIStandFigureR.SvtId;
                if (!flag)
                {
                    flag = usd.getFigureImageLimitCount() != this.mUIStandFigureR.ImageLimitCountValue;
                }
            }
            else
            {
                flag = true;
            }
        }
        if (flag)
        {
            this.Destroy();
            this.SlideOut(null, true);
            this.mUIStandFigureR = StandFigureManager.CreateRenderPrefab(base.gameObject, usd.getSvtId(), usd.getLimitCount(), usd.lv, Face.Type.NORMAL, depth, new System.Action(yb.<>m__1A0));
        }
        else
        {
            yb.this_end_act.Call();
        }
    }

    public void SlideIn(System.Action end_act = null)
    {
        if (this.IsEnable() && (!this.IsMoving() || !this.mIsFrameIn))
        {
            this.mTgtPosX = FRAME_IN_POS_X;
            this.mTgtAlp = 1f;
            this.mSpdRateAlp = 0.175f;
            this.mIsFrameIn = true;
            this.mSlideEndAct = end_act;
            if (this.GetState() != STATE.NONE)
            {
                this.SetState(STATE.SLIDE);
            }
        }
    }

    public void SlideOut(System.Action end_act = null, bool is_force = false)
    {
        if (this.IsEnable() && (!this.IsMoving() || this.mIsFrameIn))
        {
            this.mTgtPosX = FRAME_OUT_POS_X;
            this.mTgtAlp = 0f;
            this.mSpdRateAlp = 0.45f;
            if (is_force)
            {
                base.gameObject.SetLocalPositionX(this.mTgtPosX);
            }
            this.mIsFrameIn = false;
            this.mSlideEndAct = end_act;
            if (this.GetState() != STATE.NONE)
            {
                this.SetState(STATE.SLIDE);
            }
        }
    }

    public void ToggleSlide()
    {
        if (this.mIsFrameIn)
        {
            this.SlideOut(null, false);
        }
        else
        {
            this.SlideIn(null);
        }
    }

    private void Update()
    {
        if (this.mFSM != null)
        {
            this.mFSM.update();
        }
    }

    [CompilerGenerated]
    private sealed class <Setup>c__AnonStoreyB0
    {
        internal StandFigureSlideComponent <>f__this;
        internal System.Action end_act;
        internal System.Action this_end_act;

        internal void <>m__19F()
        {
            this.<>f__this.SetState(StandFigureSlideComponent.STATE.READY);
            this.end_act.Call();
            this.<>f__this.mIsLoading = false;
        }

        internal void <>m__1A0()
        {
            this.<>f__this.mNowAlp = 0f;
            this.<>f__this.mUIStandFigureR.Texture.alpha = 0f;
            this.this_end_act.Call();
        }
    }

    public enum STATE
    {
        NONE,
        READY,
        SLIDE,
        SIZEOF
    }

    protected class StateNone : IState<StandFigureSlideComponent>
    {
        public void begin(StandFigureSlideComponent that)
        {
        }

        public void end(StandFigureSlideComponent that)
        {
        }

        public void update(StandFigureSlideComponent that)
        {
        }
    }

    protected class StateReady : IState<StandFigureSlideComponent>
    {
        public void begin(StandFigureSlideComponent that)
        {
        }

        public void end(StandFigureSlideComponent that)
        {
        }

        public void update(StandFigureSlideComponent that)
        {
        }
    }

    protected class StateSlide : IState<StandFigureSlideComponent>
    {
        public const float TGT_MOVE_END_DIF = 0.25f;

        public void begin(StandFigureSlideComponent that)
        {
        }

        public void end(StandFigureSlideComponent that)
        {
        }

        public void update(StandFigureSlideComponent that)
        {
            GameObject gameObject = that.gameObject;
            float x = (that.mTgtPosX - gameObject.GetLocalPositionX()) * 0.35f;
            gameObject.AddLocalPositionX(x);
            float num2 = (that.mTgtAlp - that.mNowAlp) * that.mSpdRateAlp;
            that.mNowAlp += num2;
            float num3 = Mathf.Clamp01(that.mNowAlp);
            if (that.mUIStandFigureR != null)
            {
                that.mUIStandFigureR.Texture.alpha = num3;
            }
            if (Mathf.Abs(x) < 0.25f)
            {
                gameObject.SetLocalPositionX(that.mTgtPosX);
                if (that.mUIStandFigureR != null)
                {
                    that.mUIStandFigureR.Texture.alpha = that.mTgtAlp;
                }
                that.SetState(StandFigureSlideComponent.STATE.READY);
                that.mSlideEndAct.Call();
                that.mSlideEndAct = null;
            }
        }
    }
}

