using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class MapGimmickComponent : MonoBehaviour
{
    [SerializeField]
    private float mAnimTime = 0.4f;
    [SerializeField]
    private int mDepthBase = 30;
    [SerializeField]
    private string mDispSeName = string.Empty;
    [SerializeField]
    private Easing.TYPE mEasingType = Easing.TYPE.EXPONENTIAL_OUT;
    private CStateManager<MapGimmickComponent> mFSM;
    [SerializeField]
    private string mHideSeName = string.Empty;
    private clsMapCtrl_MapGimmickInfo mMapCtrl_MapGimmickInfo;
    private long mOldDispTime;
    [SerializeField]
    private float mScaleBase = 1f;
    [SerializeField]
    private UISprite mSprite;
    private System.Action mStateEndAct;

    private void Awake()
    {
        if (this.mFSM == null)
        {
            this.mFSM = new CStateManager<MapGimmickComponent>(this, 4);
            this.mFSM.add(0, new StateNone());
            this.mFSM.add(1, new StateMapMain());
            this.mFSM.add(2, new StateHideAnim());
            this.mFSM.add(3, new StateDispAnim());
            this.SetState(STATE.MAP_MAIN, null);
        }
    }

    public static string GetGobjName(int id) => 
        ("MapGimmick_" + id.ToString("00"));

    public clsMapCtrl_MapGimmickInfo GetMapCtrl_MapGimmickInfo() => 
        this.mMapCtrl_MapGimmickInfo;

    public STATE GetState() => 
        ((STATE) this.mFSM.getState());

    private void SetAlphaAnim(bool is_disp)
    {
        this.SetAlphaAnim(is_disp, this.mAnimTime, true);
    }

    private void SetAlphaAnim(bool is_disp, float time, bool is_play_se)
    {
        <SetAlphaAnim>c__AnonStoreyB3 yb = new <SetAlphaAnim>c__AnonStoreyB3 {
            <>f__this = this,
            eo = base.gameObject.SafeGetComponent<EasingObject>(),
            from = this.mSprite.alpha,
            to = !is_disp ? 0 : 1
        };
        string str = !is_disp ? this.mHideSeName : this.mDispSeName;
        System.Action endAct = new System.Action(yb.<>m__1A6);
        if (time > 0f)
        {
            yb.eo.Play(time, new System.Action(yb.<>m__1A7), endAct, 0f, this.mEasingType);
        }
        else
        {
            endAct.Call();
        }
        if (is_play_se && !string.IsNullOrEmpty(str))
        {
            SoundManager.playSe(str);
        }
    }

    public void SetAlphaAnimQuick(bool is_disp)
    {
        this.SetAlphaAnim(is_disp, 0f, false);
    }

    public void SetState(STATE state, System.Action end_act = null)
    {
        this.mStateEndAct = end_act;
        this.mFSM.setState((int) state);
    }

    public void Setup(clsMapCtrl_MapGimmickInfo mg_inf, UIAtlas atlas)
    {
        MapGimmickEntity entity = mg_inf.mfGetMine();
        this.mMapCtrl_MapGimmickInfo = mg_inf;
        this.mSprite.atlas = atlas;
        this.mSprite.spriteName = "gimmick_" + entity.imageId.ToString("000000");
        this.mSprite.MakePixelPerfect();
        this.mSprite.depth = this.mDepthBase + entity.depthOffset;
        this.mSprite.alpha = (mg_inf.mfGetDispType() != clsMapCtrl_MapGimmickInfo.enDispType.None) ? 1f : 0f;
        float f = this.mScaleBase * (((float) entity.scale) / 1000f);
        this.mSprite.gameObject.SetLocalScale(f);
    }

    private void StateAlphaAnimEnd()
    {
        this.mStateEndAct.Call();
        this.SetState(STATE.MAP_MAIN, null);
    }

    private void Update()
    {
        if (this.mFSM != null)
        {
            this.mFSM.update();
        }
    }

    [CompilerGenerated]
    private sealed class <SetAlphaAnim>c__AnonStoreyB3
    {
        internal MapGimmickComponent <>f__this;
        internal EasingObject eo;
        internal float from;
        internal int to;

        internal void <>m__1A6()
        {
            this.<>f__this.mSprite.alpha = this.to;
            this.<>f__this.StateAlphaAnimEnd();
        }

        internal void <>m__1A7()
        {
            float num = (this.to - this.from) * this.eo.Now();
            float num2 = this.from + num;
            this.<>f__this.mSprite.alpha = num2;
        }
    }

    public enum STATE
    {
        NONE,
        MAP_MAIN,
        HIDE_ANIM,
        DISP_ANIM,
        SIZEOF
    }

    private class StateDispAnim : IState<MapGimmickComponent>
    {
        public void begin(MapGimmickComponent that)
        {
            switch (that.mMapCtrl_MapGimmickInfo.mfGetDispType())
            {
                case clsMapCtrl_MapGimmickInfo.enDispType.None:
                    that.SetAlphaAnim(true);
                    break;

                case clsMapCtrl_MapGimmickInfo.enDispType.Normal:
                    that.StateAlphaAnimEnd();
                    break;
            }
        }

        public void end(MapGimmickComponent that)
        {
        }

        public void update(MapGimmickComponent that)
        {
        }
    }

    private class StateHideAnim : IState<MapGimmickComponent>
    {
        public void begin(MapGimmickComponent that)
        {
            switch (that.mMapCtrl_MapGimmickInfo.mfGetDispType())
            {
                case clsMapCtrl_MapGimmickInfo.enDispType.None:
                    that.StateAlphaAnimEnd();
                    break;

                case clsMapCtrl_MapGimmickInfo.enDispType.Normal:
                    that.SetAlphaAnim(false);
                    break;
            }
        }

        public void end(MapGimmickComponent that)
        {
        }

        public void update(MapGimmickComponent that)
        {
        }
    }

    private class StateMapMain : IState<MapGimmickComponent>
    {
        public static readonly int CHECK_DISP_ITVL_SEC = 60;
        private MapGimmickComponent mThat;

        public void begin(MapGimmickComponent that)
        {
            this.mThat = that;
        }

        private void CheckDispTime()
        {
            if (!SingletonMonoBehaviour<QuestAfterAction>.Instance.IsActiveCommand())
            {
                long num = NetworkManager.getTime();
                long num2 = num - this.mThat.mOldDispTime;
                if (num2 >= CHECK_DISP_ITVL_SEC)
                {
                    if (SingletonTemplate<QuestTree>.Instance.CheckMapGimmickCond(this.mThat.mMapCtrl_MapGimmickInfo))
                    {
                        this.mThat.SetState(MapGimmickComponent.STATE.DISP_ANIM, null);
                    }
                    else
                    {
                        this.mThat.SetState(MapGimmickComponent.STATE.HIDE_ANIM, null);
                    }
                    this.mThat.mOldDispTime = num;
                }
            }
        }

        public void end(MapGimmickComponent that)
        {
        }

        public void update(MapGimmickComponent that)
        {
            this.CheckDispTime();
        }
    }

    private class StateNone : IState<MapGimmickComponent>
    {
        public void begin(MapGimmickComponent that)
        {
        }

        public void end(MapGimmickComponent that)
        {
        }

        public void update(MapGimmickComponent that)
        {
        }
    }
}

