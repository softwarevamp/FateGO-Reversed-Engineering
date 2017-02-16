using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class MissionNotifyComponent : MonoBehaviour
{
    private const int BASE_DEPTH = 150;
    [SerializeField]
    private UISprite baseSp;
    [SerializeField]
    private GameObject clearEffectPrefab;
    [SerializeField]
    private UISprite clearSp;
    private const float DRAG_DONE_ALP_SPD_RATE = 0.15f;
    private const float DRAG_DONE_MV_SPD_RATE = 0.88f;
    private const float FRAME_IN_POS_OFS_Y = 1f;
    private const float FRAME_IN_TIME = 0.5f;
    private const int FRAME_OUT_OFS_DEPTH = -50;
    private const float FRAME_OUT_TIME = 0.25f;
    private Camera mCamera;
    private Animation mClearEffectAnim;
    private System.Action mDragStartAct;
    private float mFrameInPosY;
    private Action<MissionNotifyComponent> mFrameOutEndAct;
    private float mFrameOutPosY;
    private Func<MissionNotifyComponent> mFrameOutStartFunc;
    private CStateManager<MissionNotifyComponent> mFSM;
    private Vector2 mInertiaSpd;
    private bool mIsDragDone;
    private bool mIsPress;
    [SerializeField]
    private UILabel missionNameLabel;
    [SerializeField]
    private UILabel noLabel;
    [SerializeField]
    private NoticeNumberComponent noticeNumberPrefab;
    [SerializeField]
    private UISlider progressBarNowSlider;
    [SerializeField]
    private GameObject progressBarRoot;
    [SerializeField]
    private UISlider progressBarUpSlider;
    [SerializeField]
    private UILabel progressLabel;
    private const float SHOWING_TIME = 2f;

    private void Awake()
    {
        float num = ((float) ManagerConfig.HEIGHT) / 2f;
        float num2 = ((float) this.baseSp.height) / 2f;
        this.mFrameInPosY = num - num2;
        this.mFrameOutPosY = num + num2;
        this.mFrameInPosY++;
        this.FrameOut(null, true);
        if (this.mFSM == null)
        {
            this.mFSM = new CStateManager<MissionNotifyComponent>(this, 4);
            this.mFSM.add(0, new StateNone());
            this.mFSM.add(1, new StateFrameIn());
            this.mFSM.add(2, new StateShowing());
            this.mFSM.add(3, new StateFrameOut());
            this.SetState(STATE.NONE);
        }
        foreach (UIWidget widget in base.gameObject.GetComponentsInChildren<UIWidget>())
        {
            widget.depth += 150;
        }
        this.mCamera = Camera.main;
    }

    public void FrameIn(System.Action end_act = null, bool is_force = false)
    {
        this.FrameInOut(true, end_act, is_force);
    }

    public void FrameInOut(bool is_framein, System.Action end_act, bool is_force = false)
    {
        <FrameInOut>c__AnonStorey53 storey = new <FrameInOut>c__AnonStorey53 {
            end_act = end_act,
            <>f__this = this,
            tgt_obj = base.gameObject
        };
        float sec = !is_framein ? 0.25f : 0.5f;
        Vector3 localPosition = storey.tgt_obj.GetLocalPosition();
        localPosition.y = this.mFrameInPosY;
        Vector3 vector2 = storey.tgt_obj.GetLocalPosition();
        vector2.y = this.mFrameOutPosY;
        storey.pos_from = !is_framein ? localPosition : vector2;
        storey.pos_to = !is_framein ? vector2 : localPosition;
        storey.alp_from = !is_framein ? 1 : 0;
        storey.alp_to = !is_framein ? 0 : 1;
        System.Action action = new System.Action(storey.<>m__17);
        if (is_force)
        {
            action.Call();
        }
        else
        {
            storey.eo = storey.tgt_obj.SafeGetComponent<EasingObject>();
            storey.eo.Play(sec, new System.Action(storey.<>m__18), action, 0f, Easing.TYPE.EXPONENTIAL_OUT);
        }
    }

    public void FrameOut(System.Action end_act = null, bool is_force = false)
    {
        this.FrameInOut(false, end_act, is_force);
    }

    public STATE GetState() => 
        ((STATE) this.mFSM.getState());

    public bool IsBusy() => 
        ((this.GetState() == STATE.FRAME_IN) || (this.GetState() == STATE.SHOWING));

    private void OnClick()
    {
        if (this.GetState() == STATE.SHOWING)
        {
            this.SetState(STATE.FRAME_OUT);
        }
    }

    private void OnDragStart()
    {
        if (((SingletonTemplate<TerminalDebugWindow>.Instance.MissionNotifyWin.IsEnableSafe(5) && (Input.touchCount < 2)) && (this.GetState() != STATE.FRAME_OUT)) && !this.mIsDragDone)
        {
            this.mIsDragDone = true;
            if (this.GetState() == STATE.FRAME_IN)
            {
                EasingObject component = base.gameObject.GetComponent<EasingObject>();
                if (component != null)
                {
                    component.SetPause(true);
                }
                this.SetState(STATE.SHOWING);
            }
            this.mDragStartAct.Call();
        }
    }

    private void OnPress(bool is_press)
    {
        if (Input.touchCount < 2)
        {
            this.mIsPress = is_press;
            if (this.mIsDragDone && (this.GetState() == STATE.SHOWING))
            {
                this.SetState(STATE.FRAME_OUT);
            }
        }
    }

    public void SetState(STATE state)
    {
        this.mFSM.setState((int) state);
    }

    public void SetupAndPlay(MissionNotifyDispInfo disp_info, System.Action drag_start_act, Func<MissionNotifyComponent> frame_out_start_func, Action<MissionNotifyComponent> frame_out_end_act)
    {
        this.noLabel.text = disp_info.eventMissionId.ToString();
        this.missionNameLabel.text = disp_info.message;
        this.progressBarRoot.SetActive(disp_info.condition > 0);
        if (this.progressBarRoot.activeSelf)
        {
            this.progressBarNowSlider.value = ((float) disp_info.progressFrom) / ((float) disp_info.condition);
            this.progressBarUpSlider.value = ((float) disp_info.progressTo) / ((float) disp_info.condition);
            this.progressLabel.text = disp_info.progressTo.ToString() + "/" + disp_info.condition.ToString();
        }
        bool flag = this.progressBarRoot.activeSelf && (this.progressBarUpSlider.value >= 1f);
        this.baseSp.spriteName = !flag ? "mission_board_02" : "mission_board_03";
        this.clearSp.gameObject.SetActive(flag);
        if (flag)
        {
            UISprite clearSp = this.clearSp;
            int num = clearSp.depth + 1;
            GameObject self = UnityEngine.Object.Instantiate<GameObject>(this.clearEffectPrefab);
            this.mClearEffectAnim = self.GetComponent<Animation>();
            this.mClearEffectAnim.Stop();
            self.SafeSetParent(clearSp);
            foreach (UIWidget widget in self.GetComponentsInChildren<UIWidget>())
            {
                widget.depth += num;
            }
        }
        GameObject gameObject = base.gameObject;
        gameObject.name = gameObject.name + this.noLabel.text;
        this.mDragStartAct = drag_start_act;
        this.mFrameOutStartFunc = frame_out_start_func;
        this.mFrameOutEndAct = frame_out_end_act;
        this.SetState(STATE.FRAME_IN);
    }

    private void Update()
    {
        if (this.mFSM != null)
        {
            this.mFSM.update();
        }
    }

    public NoticeNumberComponent NoticeNumberPrefab =>
        this.noticeNumberPrefab;

    [CompilerGenerated]
    private sealed class <FrameInOut>c__AnonStorey53
    {
        internal MissionNotifyComponent <>f__this;
        internal int alp_from;
        internal int alp_to;
        internal System.Action end_act;
        internal EasingObject eo;
        internal Vector3 pos_from;
        internal Vector3 pos_to;
        internal GameObject tgt_obj;

        internal void <>m__17()
        {
            this.tgt_obj.SetLocalPosition(this.pos_to);
            this.<>f__this.baseSp.alpha = this.alp_to;
            this.end_act.Call();
        }

        internal void <>m__18()
        {
            float num = this.eo.Now();
            Vector3 vector = (Vector3) ((this.pos_to - this.pos_from) * num);
            this.tgt_obj.SetLocalPosition(this.pos_from + vector);
            float num2 = (this.alp_to - this.alp_from) * num;
            this.<>f__this.baseSp.alpha = this.alp_from + num2;
        }
    }

    public enum STATE
    {
        NONE,
        FRAME_IN,
        SHOWING,
        FRAME_OUT,
        SIZEOF
    }

    private class StateFrameIn : IState<MissionNotifyComponent>
    {
        public void begin(MissionNotifyComponent that)
        {
            <begin>c__AnonStorey54 storey = new <begin>c__AnonStorey54 {
                that = that
            };
            SoundManager.playSe("mis1");
            storey.that.FrameIn(new System.Action(storey.<>m__19), false);
        }

        public void end(MissionNotifyComponent that)
        {
        }

        public void update(MissionNotifyComponent that)
        {
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStorey54
        {
            internal MissionNotifyComponent that;

            internal void <>m__19()
            {
                this.that.SetState(MissionNotifyComponent.STATE.SHOWING);
            }
        }
    }

    private class StateFrameOut : IState<MissionNotifyComponent>
    {
        private MissionNotifyComponent mNextComp;

        public void begin(MissionNotifyComponent that)
        {
            <begin>c__AnonStorey55 storey = new <begin>c__AnonStorey55 {
                that = that
            };
            this.mNextComp = storey.that.mFrameOutStartFunc();
            if (!storey.that.mIsDragDone && (this.mNextComp == null))
            {
                storey.that.FrameOut(new System.Action(storey.<>m__1A), false);
            }
            foreach (UIWidget widget in storey.that.gameObject.GetComponentsInChildren<UIWidget>())
            {
                widget.depth += -50;
            }
        }

        public void end(MissionNotifyComponent that)
        {
            that.mFrameOutEndAct.Call<MissionNotifyComponent>(that);
        }

        public void update(MissionNotifyComponent that)
        {
            <update>c__AnonStorey56 storey = new <update>c__AnonStorey56 {
                that = that
            };
            if (!storey.that.mIsDragDone)
            {
                if ((this.mNextComp != null) && (this.mNextComp.GetState() != MissionNotifyComponent.STATE.FRAME_IN))
                {
                    this.mNextComp = null;
                    storey.that.FrameOut(new System.Action(storey.<>m__1B), false);
                }
            }
            else
            {
                Vector2 v = storey.that.gameObject.GetLocalPosition() + storey.that.mInertiaSpd;
                storey.that.mInertiaSpd = (Vector2) (storey.that.mInertiaSpd * 0.88f);
                storey.that.gameObject.SetLocalPosition(v);
                storey.that.baseSp.alpha += (0f - storey.that.baseSp.alpha) * 0.15f;
                if (storey.that.baseSp.alpha <= 0.01f)
                {
                    storey.that.SetState(MissionNotifyComponent.STATE.NONE);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStorey55
        {
            internal MissionNotifyComponent that;

            internal void <>m__1A()
            {
                this.that.SetState(MissionNotifyComponent.STATE.NONE);
            }
        }

        [CompilerGenerated]
        private sealed class <update>c__AnonStorey56
        {
            internal MissionNotifyComponent that;

            internal void <>m__1B()
            {
                this.that.SetState(MissionNotifyComponent.STATE.NONE);
            }
        }
    }

    private class StateNone : IState<MissionNotifyComponent>
    {
        public void begin(MissionNotifyComponent that)
        {
        }

        public void end(MissionNotifyComponent that)
        {
        }

        public void update(MissionNotifyComponent that)
        {
        }
    }

    private class StateShowing : IState<MissionNotifyComponent>
    {
        private int mDragFrameCount;
        private float mStartTime;
        private Vector2 mTouchPosDif;
        private Vector2 mTouchPosDifOld;
        private Vector2 mTouchPosNow;
        private Vector2 mTouchPosOld;

        public void begin(MissionNotifyComponent that)
        {
            this.mStartTime = Time.realtimeSinceStartup;
            that.baseSp.alpha = 1f;
            if (that.mClearEffectAnim != null)
            {
                that.mClearEffectAnim.Play();
            }
        }

        public void end(MissionNotifyComponent that)
        {
            Vector2 mTouchPosDif = this.mTouchPosDif;
            if (mTouchPosDif.sqrMagnitude < this.mTouchPosDifOld.sqrMagnitude)
            {
                mTouchPosDif = this.mTouchPosDifOld;
            }
            that.mInertiaSpd = mTouchPosDif;
        }

        public void update(MissionNotifyComponent that)
        {
            if (!that.mIsDragDone)
            {
                if (!that.mIsPress)
                {
                    float num = Time.realtimeSinceStartup - this.mStartTime;
                    if (num >= 2f)
                    {
                        that.SetState(MissionNotifyComponent.STATE.FRAME_OUT);
                    }
                }
            }
            else
            {
                Vector2 localPosition = that.gameObject.GetLocalPosition();
                if (that.mIsDragDone)
                {
                    this.mTouchPosOld = this.mTouchPosNow;
                    this.mTouchPosNow = CTouch.getScreenPosition(that.mCamera);
                    if (this.mDragFrameCount > 0)
                    {
                        this.mTouchPosDifOld = this.mTouchPosDif;
                        this.mTouchPosDif = this.mTouchPosNow - this.mTouchPosOld;
                    }
                    localPosition += this.mTouchPosDif;
                    this.mDragFrameCount++;
                }
                that.gameObject.SetLocalPosition(localPosition);
            }
        }
    }
}

