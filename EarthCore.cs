using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class EarthCore : MonoBehaviour
{
    public const float CORE_SCL = 350f;
    public static readonly Vector3 FOCUS_IN_POS = new Vector3(-265f, -170f, 0f);
    public const float FOCUS_IN_SCL = 1f;
    public static readonly Vector3 FOCUS_OUT_POS = new Vector3(-265f, -200f, 0f);
    public const float FOCUS_OUT_SCL = 0.95f;
    public const float FOCUS_SPD_RATE = 0.35f;
    [SerializeField]
    private Camera mEarthCamera;
    [SerializeField]
    private Camera mEarthEffCamera;
    [SerializeField]
    private GameObject mEarthPointRoot;
    [SerializeField]
    private GameObject mEarthRoot;
    private EarthPoint mFocusEarthPoint;
    private Vector3 mFocusTgtPos;
    private float mFocusTgtScl;
    private CStateManager<EarthCore> mFSM;
    private bool mIsFocusIn;
    private bool mIsFocusMoving;
    private bool mIsReqZoomIn;
    private bool mIsTouch;
    private MeshRenderer mMeshRenderer;
    [SerializeField]
    private GameObject mPointEffectObj;
    private Quaternion mResumeQua;
    private Quaternion mRootQua;
    private StateMain mStateMain = new StateMain();
    [SerializeField]
    private GameObject mTerminalMap;
    [SerializeField]
    private TerminalSceneComponent mTerminalScene;
    public const float ROTATE_AUTO_SPD = -0.4f;
    public const float ZOOM_IN_SCL = 6f;
    public const float ZOOM_IN_SPD_TIME = 1f;
    public const float ZOOM_IN_WHITE_FADE_TIME = 0.4f;
    public const float ZOOM_OUT_SPD_TIME = 1f;
    public const float ZOOM_OUT_WHITE_FADE_TIME = 0.4f;

    private void Awake()
    {
        if (this.mFSM == null)
        {
            this.mFSM = new CStateManager<EarthCore>(this, 4);
            this.mFSM.add(0, new StateNone());
            this.mFSM.add(1, this.mStateMain);
            this.mFSM.add(2, new StateZoomIn());
            this.mFSM.add(3, new StateZoomOut());
            this.SetState(STATE.NONE);
        }
        base.gameObject.SetLocalScale((float) 350f);
        this.mEarthRoot.SetLocalScale(Vector3.one);
        this.mMeshRenderer = base.gameObject.GetComponent<MeshRenderer>();
        this.mRootQua = this.mEarthRoot.transform.localRotation;
        this.SetDisp(true);
    }

    public void FocusIn(bool is_force = false)
    {
        this.FocusInOut(true, is_force);
    }

    public void FocusInOut(bool is_focus_in, bool is_force = false)
    {
        this.mIsFocusIn = is_focus_in;
        this.mFocusTgtPos = !this.mIsFocusIn ? FOCUS_OUT_POS : FOCUS_IN_POS;
        this.mFocusTgtScl = !this.mIsFocusIn ? 0.95f : 1f;
        if (is_force)
        {
            this.mEarthRoot.SetLocalPosition(this.mFocusTgtPos);
            this.mEarthRoot.SetLocalScale(this.mFocusTgtScl);
        }
    }

    public void FocusOut(bool is_force = false)
    {
        this.FocusInOut(false, is_force);
    }

    public EarthPoint GetEarthPoint(int war_id)
    {
        Transform transform = this.mEarthPointRoot.transform.FindChild("EarthPoint_" + war_id);
        if (transform != null)
        {
            return transform.gameObject.GetComponent<EarthPoint>();
        }
        return null;
    }

    public STATE GetState() => 
        ((STATE) this.mFSM.getState());

    public StateMain GetStateMain() => 
        this.mStateMain;

    public void mcbfStartMain()
    {
        this.SetState(STATE.MAIN);
    }

    public void mcbfStartZoomIn()
    {
        this.mIsReqZoomIn = true;
    }

    public void mcbfStartZoomOut()
    {
        this.SetState(STATE.ZOOM_OUT);
    }

    public void OnPress()
    {
        this.mIsTouch = true;
        this.mStateMain.InertialSpdOld = this.mStateMain.InertialSpd;
    }

    public void OnRelease()
    {
        this.mIsTouch = false;
    }

    public void SetDisp(bool is_disp)
    {
        this.mMeshRenderer.enabled = is_disp;
        this.mEarthPointRoot.SetActive(is_disp);
    }

    public void SetFocusEarthPoint(int war_id)
    {
        this.mFocusEarthPoint = this.GetEarthPoint(war_id);
    }

    public void SetRotateY_PointInitAngle()
    {
        Vector3 localEulerAngles = base.transform.localEulerAngles;
        localEulerAngles.y = this.PointInitAngle;
        base.transform.localEulerAngles = localEulerAngles;
    }

    public void SetState(STATE state)
    {
        this.mFSM.setState((int) state);
    }

    public void Setup(List<clsMapCtrl_WarInfo> war_infs, bool is_focus_in)
    {
        this.mEarthRoot.transform.localRotation = this.mRootQua;
        base.gameObject.SetLocalEulerAngle(Vector3.zero);
        this.FocusInOut(is_focus_in, true);
        int num = ConstantMaster.getValue("LAST_WAR_ID");
        bool flag = false;
        for (int i = 0; i < war_infs.Count; i++)
        {
            clsMapCtrl_WarInfo info = war_infs[i];
            int num3 = info.mfGetWarID();
            if (num3 != WarEntity.CALDEAGATE_ID)
            {
                bool flag2 = SingletonTemplate<clsQuestCheck>.Instance.IsWarClear(num3);
                EarthPoint earthPoint = this.GetEarthPoint(info.mfGetWarID());
                if (earthPoint != null)
                {
                    GameObject gameObject = earthPoint.gameObject;
                    if ((gameObject != null) && ((info.GetEventId() > 0) || (num3 <= num)))
                    {
                        if ((num3 == num) && !flag)
                        {
                            earthPoint.Setup(this.mPointEffectObj, this.mEarthEffCamera);
                            flag = true;
                        }
                        else
                        {
                            bool flag3 = !flag2 && !flag;
                            if (info.GetEventId() > 0)
                            {
                                flag3 = SingletonTemplate<QuestTree>.Instance.IsActiveEventWar(num3);
                            }
                            earthPoint.Setup(!flag3 ? null : this.mPointEffectObj, this.mEarthEffCamera);
                            if (SingletonTemplate<TerminalDebugWindow>.Instance.TopWin.IsEnableSafe(5))
                            {
                                gameObject.GetComponent<UISprite>().enabled = !flag3 && !flag;
                            }
                            if (flag3)
                            {
                                flag = true;
                            }
                        }
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (this.mFSM != null)
        {
            this.mFSM.update();
        }
        if (this.mIsReqZoomIn && (this.GetState() == STATE.MAIN))
        {
            this.mIsReqZoomIn = false;
            this.SetState(STATE.ZOOM_IN);
        }
    }

    public bool IsFocusIn =>
        this.mIsFocusIn;

    public bool IsFocusMoving =>
        this.mIsFocusMoving;

    public float PointInitAngle { get; set; }

    public enum STATE
    {
        NONE,
        MAIN,
        ZOOM_IN,
        ZOOM_OUT,
        SIZEOF
    }

    public class StateMain : IState<EarthCore>
    {
        private float mInertialSpd;

        public void begin(EarthCore that)
        {
            this.mInertialSpd = 0f;
        }

        public void end(EarthCore that)
        {
        }

        public void update(EarthCore that)
        {
            float y = -0.4f;
            if ((that.mIsTouch && that.IsFocusIn) && !that.IsFocusMoving)
            {
                y = 0f;
                this.mInertialSpd = 0f;
                if (CTouch.isDrag())
                {
                    float x = CTouch.getScrPosDelta().x;
                    float num6 = CTouch.getScrPosDeltaOld().x;
                    if (Mathf.Abs(x) < Mathf.Abs(num6))
                    {
                        x = num6;
                    }
                    this.mInertialSpd = x * -0.2f;
                }
            }
            else
            {
                this.mInertialSpd *= 0.88f;
                if (Mathf.Abs(this.mInertialSpd) < 0.004f)
                {
                    this.mInertialSpd = 0f;
                }
            }
            y += this.mInertialSpd;
            that.transform.Rotate(new Vector3(0f, y, 0f));
            Vector3 v = (Vector3) ((that.mFocusTgtPos - that.mEarthRoot.GetLocalPosition()) * 0.35f);
            float f = (that.mFocusTgtScl - that.mEarthRoot.GetLocalScale().x) * 0.35f;
            bool flag = (((Mathf.Abs(v.x) >= 0.25f) || (Mathf.Abs(v.y) >= 0.25f)) || (Mathf.Abs(v.z) >= 0.25f)) || (Mathf.Abs(f) >= 0.01f);
            if (flag)
            {
                that.mEarthRoot.AddLocalPosition(v);
                that.mEarthRoot.AddLocalScale(f);
            }
            else
            {
                that.mEarthRoot.SetLocalPosition(that.mFocusTgtPos);
                that.mEarthRoot.SetLocalScale(that.mFocusTgtScl);
            }
            that.mIsFocusMoving = flag;
        }

        public float InertialSpd =>
            this.mInertialSpd;

        public float InertialSpdOld { get; set; }
    }

    private class StateNone : IState<EarthCore>
    {
        public void begin(EarthCore that)
        {
        }

        public void end(EarthCore that)
        {
        }

        public void update(EarthCore that)
        {
        }
    }

    private class StateZoomIn : IState<EarthCore>
    {
        private float mSpdTime;
        private float mStartTime;
        private EarthCore mThat;

        public void begin(EarthCore that)
        {
            <begin>c__AnonStoreyAA yaa = new <begin>c__AnonStoreyAA {
                that = that,
                <>f__this = this
            };
            this.mThat = yaa.that;
            this.mSpdTime = TerminalPramsManager.GetIntpTime_AutoResume(1f);
            if (yaa.that.mFocusEarthPoint == null)
            {
                this.GoNext();
            }
            else
            {
                yaa.eo = yaa.that.gameObject.SafeGetComponent<EasingObject>();
                yaa.qua_from = yaa.that.gameObject.transform.rotation;
                yaa.qua_to = yaa.that.mFocusEarthPoint.FocusQua;
                yaa.that.mResumeQua = yaa.qua_from;
                yaa.root_qua_from = yaa.that.mEarthRoot.transform.rotation;
                yaa.root_qua_to = Quaternion.identity;
                yaa.root_pos_from = yaa.that.mEarthRoot.GetLocalPosition();
                yaa.root_pos_to = Vector3.zero;
                yaa.root_scl_from = yaa.that.mEarthRoot.gameObject.GetLocalScale();
                yaa.root_scl_to = new Vector3(6f, 6f, 6f);
                yaa.eo.Play(this.mSpdTime, new System.Action(yaa.<>m__192), new System.Action(yaa.<>m__193), 0f, Easing.TYPE.QUADRATIC_IN);
                if (!TerminalPramsManager.IsAutoResume)
                {
                    SoundManager.playSe("wo1");
                    this.mStartTime = Time.realtimeSinceStartup;
                }
            }
        }

        public void end(EarthCore that)
        {
        }

        private void GoNext()
        {
            this.mThat.SetState(EarthCore.STATE.NONE);
            this.mThat.mTerminalMap.GetComponent<PlayMakerFSM>().Fsm.Event("GO_NEXT");
        }

        public void update(EarthCore that)
        {
            if (this.mStartTime > 0f)
            {
                float num = Time.realtimeSinceStartup - this.mStartTime;
                float duration = this.mSpdTime - num;
                if (duration <= 0.4f)
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.WHITE, duration, new System.Action(this.GoNext));
                    this.mStartTime = 0f;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStoreyAA
        {
            internal EarthCore.StateZoomIn <>f__this;
            internal EasingObject eo;
            internal Quaternion qua_from;
            internal Quaternion qua_to;
            internal Vector3 root_pos_from;
            internal Vector3 root_pos_to;
            internal Quaternion root_qua_from;
            internal Quaternion root_qua_to;
            internal Vector3 root_scl_from;
            internal Vector3 root_scl_to;
            internal EarthCore that;

            internal void <>m__192()
            {
                float num = this.eo.Now();
                float t = (float) StepFunc.DecSin((double) num);
                this.that.gameObject.transform.rotation = Quaternion.Slerp(this.qua_from, this.qua_to, t);
                this.that.mEarthRoot.transform.rotation = Quaternion.Slerp(this.root_qua_from, this.root_qua_to, t);
                this.that.mEarthRoot.transform.localPosition = Vector3.Lerp(this.root_pos_from, this.root_pos_to, t);
                this.that.mEarthRoot.transform.localScale = Vector3.Lerp(this.root_scl_from, this.root_scl_to, (float) StepFunc.Acc5((double) num));
            }

            internal void <>m__193()
            {
                this.that.gameObject.transform.rotation = this.qua_to;
                this.that.mEarthRoot.transform.rotation = this.root_qua_to;
                this.that.mEarthRoot.transform.localPosition = this.root_pos_to;
                this.that.mEarthRoot.transform.localScale = this.root_scl_to;
                if (TerminalPramsManager.IsAutoResume)
                {
                    this.<>f__this.GoNext();
                }
            }
        }
    }

    private class StateZoomOut : IState<EarthCore>
    {
        [CompilerGenerated]
        private static System.Action <>f__am$cache0;

        public void begin(EarthCore that)
        {
            <begin>c__AnonStoreyAB yab = new <begin>c__AnonStoreyAB {
                that = that
            };
            float sec = TerminalPramsManager.GetIntpTime_AutoResume(1f);
            yab.eo = yab.that.gameObject.SafeGetComponent<EasingObject>();
            yab.qua_from = yab.that.gameObject.transform.rotation;
            yab.qua_to = yab.that.mResumeQua;
            yab.root_qua_from = yab.that.mEarthRoot.transform.rotation;
            yab.root_qua_to = yab.that.mRootQua;
            yab.root_pos_from = yab.that.mEarthRoot.GetLocalPosition();
            yab.root_pos_to = !yab.that.IsFocusIn ? EarthCore.FOCUS_OUT_POS : EarthCore.FOCUS_IN_POS;
            yab.root_scl_from = yab.that.mEarthRoot.gameObject.GetLocalScale();
            float x = !yab.that.IsFocusIn ? 0.95f : 1f;
            yab.root_scl_to = new Vector3(x, x, x);
            yab.eo.Play(1f, 0f, sec, new System.Action(yab.<>m__194), new System.Action(yab.<>m__195), 0f, Easing.TYPE.QUADRATIC_IN_OUT);
            yab.that.mTerminalScene.IsReq_InitEarthRotateY = false;
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate {
                };
            }
            yab.that.mTerminalScene.Fadein_WorldDisp(0.4f, <>f__am$cache0);
        }

        public void end(EarthCore that)
        {
        }

        public void update(EarthCore that)
        {
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStoreyAB
        {
            internal EasingObject eo;
            internal Quaternion qua_from;
            internal Quaternion qua_to;
            internal Vector3 root_pos_from;
            internal Vector3 root_pos_to;
            internal Quaternion root_qua_from;
            internal Quaternion root_qua_to;
            internal Vector3 root_scl_from;
            internal Vector3 root_scl_to;
            internal EarthCore that;

            internal void <>m__194()
            {
                float num = this.eo.Now();
                float t = (float) StepFunc.DecSin((double) num);
                this.that.gameObject.transform.rotation = Quaternion.Slerp(this.qua_to, this.qua_from, t);
                this.that.mEarthRoot.transform.rotation = Quaternion.Slerp(this.root_qua_to, this.root_qua_from, t);
                this.that.mEarthRoot.transform.localPosition = Vector3.Lerp(this.root_pos_to, this.root_pos_from, t);
                this.that.mEarthRoot.transform.localScale = Vector3.Lerp(this.root_scl_to, this.root_scl_from, (float) StepFunc.Acc5((double) num));
            }

            internal void <>m__195()
            {
                this.that.gameObject.transform.rotation = this.qua_to;
                this.that.mEarthRoot.transform.rotation = this.root_qua_to;
                this.that.mEarthRoot.transform.localPosition = this.root_pos_to;
                this.that.mEarthRoot.transform.localScale = this.root_scl_to;
                this.that.SetState(EarthCore.STATE.NONE);
                this.that.mTerminalMap.GetComponent<PlayMakerFSM>().Fsm.Event("GO_NEXT");
            }
        }
    }
}

